using MyBox;
using UnityEngine;

namespace Clicker.Gameplay
{
    [System.Serializable]
    public class QuestGoal
    {
        public float targetAmount;
        [ReadOnly] public float currentAmount;

        public bool IsReached() => currentAmount >= targetAmount;
    }
}
