using Clicker.Manager;
using MyBox;
using UnityEngine;

namespace Clicker.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New item", menuName = "Item SO")]
    public class ItemSO : ScriptableObject
    {
        [Foldout("General", true)]
        public Sprite itemSprite;
        public string itemName;
        [TextArea] public string itemDescription;
    }
}
