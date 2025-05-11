using Clicker.ScriptableObjects;
using MyBox;
using NUnit.Framework;
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
        private List<QuestSO> completedQuestList = new List<QuestSO>();
        private List<int> indexPool = new List<int>();

        //UI Events
        public Action<string, int> OnQuestRefreshed;
        public Action<int> OnQuestComplete;

        private void Start()
        {
            InitializeQuests();
            SetUpIndexPool();

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

                    completedQuestList.Add(quest);
                    if(completedQuestList.Count >= 3)
                    {
                        RenewAllQuest();
                        completedQuestList.Clear();
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
                int randomIndex = UnityEngine.Random.Range(0, indexPool.Count);
                int chosenIndex = indexPool[randomIndex];
                activeQuestsList.Add(allQuestSO[chosenIndex]);
                indexPool.RemoveAt(chosenIndex);

                if(indexPool.Count <= 3)
                {
                    indexPool.Clear();
                    SetUpIndexPool();
                }

                activeQuestsList[i].StartQuest();
                OnQuestRefreshed?.Invoke(activeQuestsList[i].questDesc, i);
            }
        }

        private void SetUpIndexPool()
        {
            for (int i = 0; i < allQuestSO.Length; i++)
            {
                indexPool.Add(i);
            }
        }

        private void InitializeQuests()
        {
            foreach (var quest in activeQuestsList)
            {
                quest.isActive = true;
            }
        }

        [ButtonMethod]
        private void LoadAllQuestSO()
        {
            allQuestSO = Resources.LoadAll<QuestSO>("_SOs/QuestSOs");
        }
    }
}
