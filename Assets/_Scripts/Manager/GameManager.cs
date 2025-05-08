using UnityEngine;
using MyBox;
using System;

namespace Clicker.Manager
{
    public class GameManager : Utilities.Singleton<GameManager>
    {
        [Foldout("General")]
        [SerializeField, ReadOnly] private int clickCount;

        //Events
        public Action<int> OnClickCountIncreased;

        //Add total click count
        public void AddClickCount(int count)
        {
            //Add click count
            clickCount += count;

            //Invoke OnClickCountIncreased events for various purposes
            OnClickCountIncreased?.Invoke(clickCount);
        }
    }
}
