namespace ReGaSLZR.Gameplay.Model
{

    using NaughtyAttributes;
    using UnityEngine;
    using UniRx;

    public class Unit : MonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        private UnitData data;

        [SerializeField]
        [Required]
        private SpriteRenderer background;

        [SerializeField]
        [Required]
        private SpriteRenderer visual;

        #endregion

        #region Private Fields

        private readonly ReactiveProperty<bool> rIsFocused =
            new ReactiveProperty<bool>();

        #endregion

        #region Accessor

        public UnitData Data => data;

        #endregion

        #region Class Implementation

        public void SetBGColor(Color colorBG)
        {
            background.color = colorBG;
        }

        public void SetFocus(bool isFocused)
        {
            rIsFocused.Value = isFocused;
        }

        #endregion
    }

}