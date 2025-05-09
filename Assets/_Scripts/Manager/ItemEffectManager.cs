using Clicker.ScriptableObjects;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

namespace Clicker.Manager
{
    public class ItemEffectManager : Utilities.Singleton<ItemEffectManager>
    {
        [Separator("Active items")]
        [SerializeField, ReadOnly] private List<ItemSO> activeItemList = new List<ItemSO>();
        
        //Apply item effect
        public void AddItemEffect(ItemSO itemSO)
        {
            activeItemList.Add(itemSO);
            itemSO.UseItem();
        }
    }
}
