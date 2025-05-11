using Clicker.ScriptableObjects;
using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clicker.Manager
{
    public class QuestManager : Utilities.Singleton<QuestManager>
    {
        [Separator("All quests")]
        [SerializeField, ReadOnly] private List<QuestSO> activeQuestsList = new List<QuestSO>();
        [SerializeField, ReadOnly] private QuestSO[] allQuestSO;
        [SerializeField] private int maxActiveQuestCount = 3;

        //UI Events
        public Action<string, int> OnQuestRefreshed;
        public Action<int> OnQuestComplete;

        private void Start()
        {
            SetUpEvents(true);
        }

        private void OnDisable()
        {
            SetUpEvents(false);
        }

        private void SetUpEvents(bool isSubscribing)
        {
            if(isSubscribing)
            {
                GameManager.Instance.OnClickCountIncreased += SetUpClickingEvents;
                ItemEffectManager.Instance.OnItemBought += SetUpPurchasingEvent;
            }
            else
            {
                GameManager.Instance.OnClickCountIncreased -= SetUpClickingEvents;
                ItemEffectManager.Instance.OnItemBought -= SetUpPurchasingEvent;
            }

            #region Local functions
            void SetUpClickingEvents()
            {
                foreach (QuestSO quest in activeQuestsList)
                {
                    if (quest.isActive)
                    {
                        SetUpQuestEvent(quest, quest.questType, QuestType.Clicking);
                    }
                }
            }

            void SetUpPurchasingEvent()
            {
                foreach (QuestSO quest in activeQuestsList)
                {
                    if (quest.isActive)
                    {
                        SetUpQuestEvent(quest, quest.questType, QuestType.Purchasing);
                    }
                }
            }

            void SetUpQuestEvent(QuestSO quest, QuestType questType,QuestType requiredQuestType)
            {
                if (questType != requiredQuestType) return;

                quest.questGoal.AddCurrentAmount(() =>
                {
                    quest.isActive = false;
                    OnQuestComplete?.Invoke(activeQuestsList.FindIndex(x => x == quest));

                    List<QuestSO> completeQuestCount = activeQuestsList.FindAll(y => y == y.questGoal.IsReached());
                    Debug.Log("Completed Quest Count : " + completeQuestCount.Count);
                    if(completeQuestCount.Count >= 3)
                    {
                        RenewAllQuest();
                        completeQuestCount.Clear();
                    }
                });
            }
            #endregion
        }

        [ButtonMethod]
        private void RenewAllQuest()
        {
            activeQuestsList.Clear();

            foreach (var quest in allQuestSO)
            {
                quest.EndQuest();
            }

            for (int i = 0; i < maxActiveQuestCount; i++)
            {
                int randomQuestIdx = UnityEngine.Random.Range(0, allQuestSO.Length);
                activeQuestsList.Add(allQuestSO[randomQuestIdx]);
                activeQuestsList[i].StartQuest();
                OnQuestRefreshed?.Invoke(activeQuestsList[i].questDesc, i);
            }
        }

        [ButtonMethod]
        private void LoadAllQuestSO()
        {
            allQuestSO = Resources.LoadAll<QuestSO>("_SOs/QuestSOs");
        }
    }
}
