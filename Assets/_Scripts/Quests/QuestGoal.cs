using Clicker.Manager;
using MyBox;
using System;
using UnityEngine;

namespace Clicker.Gameplay
{
    [System.Serializable]
    public class QuestGoal
    {
        public float targetAmount;
        [ReadOnly] public float currentAmount;

        public void AddCurrentAmount(Action OnQuestComplete)
        {
            currentAmount++;

            if(IsReached())
            {
                currentAmount = 0f;
                OnQuestComplete?.Invoke();

                //Save player data
                PlayfabManager.Instance.SavePlayerData();
            }
        }
        public bool IsReached() => currentAmount >= targetAmount;
    }
}
