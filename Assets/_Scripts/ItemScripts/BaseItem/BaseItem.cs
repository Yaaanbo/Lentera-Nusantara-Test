using Clicker.ScriptableObjects;
using MyBox;
using UnityEngine;

namespace Clicker.Gameplay
{
    public abstract class BaseItem : MonoBehaviour
    {
        [Separator("Item Scriptable Object")]
        [SerializeField] protected ItemSO itemSO;

        public abstract void UseItem();
    }
}
