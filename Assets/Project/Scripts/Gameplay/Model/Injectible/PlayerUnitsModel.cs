namespace ReGaSLZR.Gameplay.Model
{

    using Enum;
    using Util;

    using UniRx;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Player Model", 
        menuName = "Project/Create Player Injectible Model")]
    public class PlayerUnitsModel : EnemyUnitsModel, 
        ITeam.IPlayerGetter, ITeam.IPlayerSetter
    {

        #region Private Fields

        private readonly ReactiveProperty<int> rScore = new ReactiveProperty<int>();

        #endregion

        #region Class Overrides

        protected override void InitValues()
        {
            base.InitValues();
            rScore.Value = 0;
        }

        #endregion

        #region MonoInstaller Implementations

        public override void InstallBindings()
        {
            InitValues();

            Container.Bind<ITeam.IPlayerGetter>().FromInstance(this);
            Container.Bind<ITeam.IPlayerSetter>().FromInstance(this);
        }

        #endregion

        #region Setter Implementation

        public void AddScore(int scoreToAdd)
        {
            if (scoreToAdd < 0)
            {
                LogUtil.PrintWarning(GetType(), "AddScore(): value is below 0. Skipping...");
                return;
            }

            rScore.Value += scoreToAdd;
        }

        #endregion

        #region Getter Implementation

        public IReadOnlyReactiveProperty<int> GetScore()
        {
            return rScore;
        }

        #endregion

    }

}