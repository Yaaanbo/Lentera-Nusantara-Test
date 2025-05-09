using Clicker.Manager;
using MyBox;
using UnityEngine;

namespace Clicker.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Click multiplier item", menuName = "Item SO/Click multiplier")]
    public class MultiplierSO : ItemSO
    {
        [Foldout("Specific properties", true)]
        [SerializeField] private MultiplierType multiplierType;

        [ConditionalField(nameof(multiplierType), false, MultiplierType.rawMultiplier)]
        [SerializeField] private float clickMultiplier;

        [ConditionalField(nameof(multiplierType), false, MultiplierType.percentageMultiplier)]
        [SerializeField]  private float clickPercentageMultiplier;

        public override void UseItem()
        {
            base.UseItem();

            //Check which multiplier to apply
            bool isRawMultiplier = multiplierType == MultiplierType.rawMultiplier;

            //Add click multiplier from GameManager
            GameManager.Instance.AddMultiplier(isRawMultiplier ? clickMultiplier : AddPercentageMultiplier(GameManager.Instance.ClickMultiplier));
        }

        private float AddPercentageMultiplier(float baseMultiplier)
        {
            return baseMultiplier * clickPercentageMultiplier / 100;
        }
    }

    public enum MultiplierType
    {
        rawMultiplier,
        percentageMultiplier
    }
}
