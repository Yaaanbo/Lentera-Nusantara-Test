using Clicker.Manager;
using MyBox;
using System;
using UnityEngine;

namespace Clicker.ScriptableObjects
{
    public abstract class ItemSO : ScriptableObject
    {
        [Foldout("Inherited properties", true)]
        public Sprite itemSprite;
        public string itemName;
        [TextArea] public string itemDescription;

        //Events when item is used (Can be used for future design updates)
        public Action OnItemUsed;

        //Handle item usage
        public virtual void UseItem()
        {
            OnItemUsed?.Invoke();
        }
    }
}
