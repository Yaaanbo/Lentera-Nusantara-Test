using Clicker.Manager;
using System;
using UnityEngine;

namespace Clicker.Gameplay
{
    public class ObjectClicker : MonoBehaviour
    {
        //Events
        public Action OnObjectClicked;

        //Handle mouse clicking
        private void OnMouseDown()
        {
            ClickItem();
        }

        //Handle object clicking
        private void ClickItem()
        {
            //Add click count from GameManager
            GameManager.Instance.AddClickCount();

            //Invoke OnObjectClicked event for various purposes
            OnObjectClicked?.Invoke();
        }
    }
}
