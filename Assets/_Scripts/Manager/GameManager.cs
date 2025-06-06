using UnityEngine;
using MyBox;
using System;
using System.Collections;

namespace Clicker.Manager
{
    public class GameManager : Utilities.Singleton<GameManager>
    {
        [Separator("General")]
        [SerializeField, ReadOnly] private float clickCount;
        public float ClickCount => clickCount;

        [Separator("Click multipliers")]
        [SerializeField] private float clickMultiplier;
        public float ClickMultiplier
        {
            get => clickMultiplier;
            set => clickMultiplier = value;
        }

        //Events
        public Action<float> OnClickCountUIUpdated;
        public Action<float> OnMultipliertUIUpdated;
        public Action OnClickCountIncreased;

        //Add total click count
        public void AddClickCount()
        {
            //Add click count
            clickCount += clickMultiplier;

            //Invoke OnClickCountIncreased events for various purposes
            OnClickCountIncreased?.Invoke();
            OnClickCountUIUpdated?.Invoke(clickCount);

            //Save player data
            PlayfabManager.Instance.SavePlayerData();
        }

        public void SubtractClickCount(float count)
        {
            //Subtract click count
            clickCount -= count;

            //Invoke OnClickCountIncreased events to update UI
            OnClickCountUIUpdated?.Invoke(clickCount);

            //Save player data
            PlayfabManager.Instance.SavePlayerData();
        }

        public void SetClickCount(float count)
        {
            clickCount = count;
            OnClickCountUIUpdated?.Invoke(clickCount);
        }
        
        public void ActivateAutoClick(float delay)
        {
            StartCoroutine(AutoClickCoroutine(delay));

            IEnumerator AutoClickCoroutine(float delay)
            {
                while (true)
                {
                    yield return new WaitForSeconds(delay);
                    AddClickCount();
                }
            }
        }

        public void SetClickMultiplier(float multiplier)
        {
            clickMultiplier = multiplier;
            OnMultipliertUIUpdated?.Invoke(clickMultiplier);
        }

        public void AddMultiplier(float multiplier)
        {
            clickMultiplier += multiplier;
            OnMultipliertUIUpdated?.Invoke(clickMultiplier);

            //Save player data
            PlayfabManager.Instance.SavePlayerData();
        }
    }
}
