namespace ReGaSLZR.Gameplay.Model
{
    using Enum;
    using UnityEngine;

    [System.Serializable]
    public class ThemeColors
    {

        #region Inspector Fields

        [SerializeField]
        private Color playerUnitBG;

        [SerializeField]
        private Color enemyUnitBG;

        #endregion

        #region Accessors

        public Color PlayerUnitBG => playerUnitBG;
        public Color EnemyUnitBG => enemyUnitBG;

        #endregion

        #region Class Implementation

        public Color GetBGColor(Team team)
        {
            return (team == Team.Player) ? playerUnitBG : enemyUnitBG;
        }

        #endregion
    }

}