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

        [Separator("Click multipliers")]
        [SerializeField] private float clickMultiplier;
        public float ClickMultiplier => clickMultiplier;

        //Events
        public Action<float> OnClickCountIncreased;

        //Add total click count
        public void AddClickCount()
        {
            //Add click count
            clickCount += MathF.Round(1 * clickMultiplier);

            //Invoke OnClickCountIncreased events for various purposes
            OnClickCountIncreased?.Invoke(clickCount);
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

        public void AddMultiplier(float multiplier)
        {
            clickMultiplier += multiplier;
        }
    }
}
