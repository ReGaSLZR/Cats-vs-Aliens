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

        [SerializeField]
        private Color logInfo;

        [SerializeField]
        private Color logInvalid;

        [SerializeField]
        private Color logCritical;

        [SerializeField]
        private Color logDanger;

        #endregion

        #region Accessors

        public Color PlayerUnitBG => playerUnitBG;
        public Color EnemyUnitBG => enemyUnitBG;
        public Color LogInfo => logInfo;
        public Color LogInvalid => logInvalid;
        public Color LogCritical => logCritical;
        public Color LogDanger => logDanger;

        #endregion

        #region Class Implementation

        public Color GetTeamColor(Team team)
        {
            return (team == Team.Player) ? playerUnitBG : enemyUnitBG;
        }

        #endregion
    }

}