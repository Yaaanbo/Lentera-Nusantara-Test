using MyBox;
using TMPro;
using UnityEngine;

namespace Clicker.Manager
{
    public class UIManager : MonoBehaviour
    {
        [Foldout("Click count UIs")]
        [SerializeField] private TMP_Text countText;

        void Start()
        {
            //Subscribe to events
            SetUpEvents(true);
        }

        private void OnDisable()
        {
            //Unsubscribe from events
            SetUpEvents(false);
        }

        //Set up UI events
        private void SetUpEvents(bool isSubscribing)
        {
            if (isSubscribing)
                GameManager.Instance.OnClickCountUIUpdated += UpdateClickCountTxt;
            else
                GameManager.Instance.OnClickCountUIUpdated -= UpdateClickCountTxt;
        }

        //Update click count text UI
        private void UpdateClickCountTxt(float clickCount)
        {
            countText.text = $"Click count : {clickCount}";
        }
    }
}
