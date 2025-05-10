using Clicker.ScriptableObjects;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using System;

namespace Clicker.Manager
{
    public class ItemEffectManager : Utilities.Singleton<ItemEffectManager>
    {
        [Separator("Active items")]
        [SerializeField, ReadOnly] private List<ItemSO> activeItemList = new List<ItemSO>();
        [SerializeField] private ItemSO[] allItem;
        public List<ItemSO> ActiveItemList => activeItemList;
        public ItemSO[] AllItem => allItem;

        //Events
        public Action OnItemBought;

        //Apply item effect
        public void AddItemEffect(ItemSO itemSO)
        {
            activeItemList.Add(itemSO);
            itemSO.UseItem();

            GameManager.Instance.SubtractClickCount(itemSO.itemPrice);

            OnItemBought?.Invoke();
        }
    }
}
