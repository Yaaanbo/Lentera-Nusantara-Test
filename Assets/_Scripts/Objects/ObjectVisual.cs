using DG.Tweening;
using MyBox;
using UnityEngine;

namespace Clicker.Gameplay
{
    public class ObjectVisual : MonoBehaviour
    {
        [Foldout("Class Reference", true)]
        [SerializeField] private ObjectClicker objectClicker;

        [Foldout("Scaling", true)]
        [SerializeField] private float bounceDuration;
        [SerializeField] private Vector3 bounceScale;
        private Vector3 initScale;

        private void Start()
        {
            //Set init scale to current local scale
            initScale = transform.localScale;

            //Subscribe to events
            SetUpEvents(true);
        }

        private void OnDisable()
        {
            //Unsubscribe from events
            SetUpEvents(false); 
        }

        //Set up visual events
        private void SetUpEvents(bool isSubscribing)
        {
            if (isSubscribing)
                objectClicker.OnObjectClicked += BounceImage;
            else
                objectClicker.OnObjectClicked -= BounceImage;
        }

        //Bounce image scale
        private void BounceImage()
        {
            //Scale image down
            this.transform.DOScale(bounceScale, bounceDuration / 2f)
                .OnComplete(() =>
                {
                    //Scale image back up
                    this.transform.DOScale(initScale, bounceDuration / 2f);
                });
        }
    }
}
