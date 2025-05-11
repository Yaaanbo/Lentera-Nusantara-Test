using Clicker.Gameplay;
using MyBox;
using System;
using UnityEngine;

namespace Clicker.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New quest", menuName = "Quest SO")]
    public class QuestSO : ScriptableObject
    {
        [Foldout("General config", true)]
        public QuestType questType;
        public bool isActive = false;
        public string questName;
        [TextArea] public string questDesc;

        [Foldout("Reward config", true)]
        public float reward;

        [Foldout("Goal", true)]
        public QuestGoal questGoal;

        public void StartQuest()
        {
            isActive = true;
            questGoal.currentAmount = 0;
        }

        public void EndQuest() => isActive = false;
    }

    public enum QuestType
    {
        Clicking,
        Purchasing,
    }
}
