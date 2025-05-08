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
                GameManager.Instance.OnClickCountIncreased += UpdateClickCountTxt;
            else
                GameManager.Instance.OnClickCountIncreased -= UpdateClickCountTxt;
        }

        //Update click count text UI
        private void UpdateClickCountTxt(int clickCount)
        {
            countText.text = $"Click count : {clickCount}";
        }
    }
}
