namespace ReGaSLZR.Gameplay.Model
{

    using NaughtyAttributes;
    using UnityEngine;

    public class UnitHolder : MonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        [Required]
        private SpriteRenderer background;

        [SerializeField]
        [Required]
        private SpriteRenderer visual;

        #endregion

        #region Private Fields

        [SerializeField]
        [ReadOnly]
        private Unit unit;

        #endregion

        #region Class Implementation

        public void SetUpUnit(Unit unit)
        {
            this.unit = unit;
        }

        public void SetBGColor(Color colorBG)
        {
            background.color = colorBG;
        }

        #endregion
    }

}