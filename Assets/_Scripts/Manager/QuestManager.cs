using Clicker.ScriptableObjects;
using MyBox;
using System.Collections.Generic;
using UnityEngine;

namespace Clicker.Manager
{
    public class QuestManager : MonoBehaviour
    {
        [Separator("All quests")]
        [SerializeField, ReadOnly] private List<QuestSO> activeQuestsList = new List<QuestSO>();
        [SerializeField, ReadOnly] private QuestSO[] allQuestSO;

        private void Start()
        {
            LoadAllQuestSO();
            GenerateQuest();
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
            }
            else
            {
                GameManager.Instance.OnClickCountIncreased -= SetUpClickingEvents;
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
                
                quest.questGoal.currentAmount++;

                if (quest.questGoal.IsReached())
                {
                    quest.questGoal.currentAmount = 0;
                    quest.isActive = false;
                    //activeQuestsList.Remove(quest);
                }
            }
            #endregion
        }

        [ButtonMethod]
        private void GenerateQuest()
        {
            int maxActiveQuestAmount = 3;
            for (int i = 0; i < maxActiveQuestAmount; i++)
            {
                activeQuestsList.Add(allQuestSO[i]);
                activeQuestsList[i].StartQuest();
            }
        }

        [ButtonMethod]
        private void LoadAllQuestSO()
        {
            allQuestSO = Resources.LoadAll<QuestSO>("_SOs/QuestSOs");
        }
    }
}
