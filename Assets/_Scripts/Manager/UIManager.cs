using Clicker.ScriptableObjects;
using MyBox;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.Manager
{
    public class UIManager : MonoBehaviour
    {
        [Foldout("Click count UIs", true)]
        [SerializeField] private TMP_Text countTxt;
        [SerializeField] private TMP_Text multiplierTxt;

        [Foldout("Shop UIs", true)]
        [SerializeField] private RectTransform shopUIPrefab;
        [SerializeField] private RectTransform shopUIParent;
        private List<RectTransform> spawnedShopUIPrefab = new List<RectTransform>();

        [Foldout("Active item UIs", true)]
        [SerializeField] private RectTransform activeItemUIPrefab;
        [SerializeField] private RectTransform activeItemUIParent;

        [Foldout("Quest UIs", true)]
        [SerializeField] private TMP_Text[] questTxts;
        [SerializeField] private Color activeQuestTxtColor;
        [SerializeField] private Color endedQuestTxtColor;

        void Start()
        {
            //Set multiplier text to current multiplier
            UpdateMultiplierTxt(GameManager.Instance.ClickMultiplier);

            //Set up shop UI
            SetShopUI();

            //Set up active items UI
            SetActiveItemUI();

            //Subscribe to events
            SetUpEvents(true);
        }

        private void OnDisable()
        {
            //Unsubscribe from events
            SetUpEvents(false);
        }

        //Set up UI events
        private void SetUpEvents(bool isSubscribing)
        {
            if (isSubscribing)
            {
                //Click count & multiplier text
                GameManager.Instance.OnClickCountUIUpdated += UpdateClickCount;
                GameManager.Instance.OnMultipliertUIUpdated += UpdateMultiplierTxt;

                //Quest UI
                QuestManager.Instance.OnQuestRefreshed += UpdateQuestListTxt;
                QuestManager.Instance.OnQuestComplete += OnQuestCompleted;
            }
            else
            {
                //Click count & multiplier text
                GameManager.Instance.OnClickCountUIUpdated -= UpdateClickCount;
                GameManager.Instance.OnMultipliertUIUpdated -= UpdateMultiplierTxt;

                //Quest UI
                QuestManager.Instance.OnQuestRefreshed -= UpdateQuestListTxt;
                QuestManager.Instance.OnQuestComplete -= OnQuestCompleted;
            }
        }

        //Update click count text UI
        private void UpdateClickCount(float clickCount)
        {
            countTxt.text = $"Fish egg : {clickCount}";
            UpdateItemButtonInteractable();
        }

        //Update multiplier text UI
        private void UpdateMultiplierTxt(float multiplier)
        {
            multiplierTxt.text = $"Multiplier : {multiplier:0.##}";
        }

        #region Item UIs
        //Update shop UI
        private void SetShopUI()
        {
            ItemSO[] allItem = ItemEffectManager.Instance.AllItem;

            for (int i = 0; i < allItem.Length; i++)
            {
                //Spawn item UI prefab
                RectTransform itemUI = Instantiate(shopUIPrefab, shopUIParent);
                
                //Add item UI to spawned UI prefab list
                spawnedShopUIPrefab.Add(itemUI);

                //Change button image
                Image itemImg = itemUI.GetChild(0).GetComponent<Image>();
                if (allItem[i].itemSprite != null)
                    itemImg.sprite = allItem[i].itemSprite;
            }
            SetItemButtonListener();
        }

        //Add button listener to each shop UI button
        private void SetItemButtonListener()
        {
            for (int i = 0; i < spawnedShopUIPrefab.Count; i++)
            {
                Button button = spawnedShopUIPrefab[i].GetComponent<Button>();
                ItemSO itemSO = ItemEffectManager.Instance.AllItem[i];

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    if (GameManager.Instance.ClickCount < itemSO.itemPrice) return;

                    ItemEffectManager.Instance.AddItemEffect(itemSO, true);
                    Debug.Log(itemSO.itemName);

                    SetActiveItemUI();
                });
                UpdateItemButtonInteractable();
            }
        }

        //Update shop button interactable after every click count changes
        private void UpdateItemButtonInteractable()
        {
            for (int i = 0; i < spawnedShopUIPrefab.Count; i++)
            {
                Button button = spawnedShopUIPrefab[i].GetComponent<Button>();
                ItemSO itemSO = ItemEffectManager.Instance.AllItem[i];

                button.interactable = GameManager.Instance.ClickCount >= itemSO.itemPrice;
            }
        }

        //Update active items UI
        private void SetActiveItemUI()
        {
            List<ItemSO> activeItemList = ItemEffectManager.Instance.ActiveItemList;

            //Check if active item is not null
            if (activeItemList.Count <= 0) return;

            //Spawn item UI prefab
            RectTransform itemUI = Instantiate(activeItemUIPrefab, activeItemUIParent);

            //Change button image
            Image itemImg = itemUI.GetComponent<Image>();
            int lastIndex = activeItemList.Count - 1;
            if (activeItemList[lastIndex].itemSprite != null)
                itemImg.sprite = activeItemList[lastIndex].itemSprite;
        }
        #endregion

        #region Quest UIs
        private void UpdateQuestListTxt(string questDesc, int index)
        {
            questTxts[index].text = questDesc;
            questTxts[index].color = activeQuestTxtColor;
        }

        private void OnQuestCompleted(int index)
        {
            questTxts[index].color = endedQuestTxtColor;
        }
        #endregion
    }
}
