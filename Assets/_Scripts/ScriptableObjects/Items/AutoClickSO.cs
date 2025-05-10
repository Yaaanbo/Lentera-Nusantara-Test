using Clicker.Manager;
using MyBox;
using System.Collections;
using UnityEngine;

namespace Clicker.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Auto clicker item", menuName = "Item SO/Auto clicker")]
    public class AutoClickSO : ItemSO
    {
        [Foldout("Specific properties", true)]
        [SerializeField] private float autoClickDelay;

        public override void UseItem()
        {
            //Add auto click rate in GameManager
            GameManager.Instance.ActivateAutoClick(autoClickDelay);
        }
    }
}
