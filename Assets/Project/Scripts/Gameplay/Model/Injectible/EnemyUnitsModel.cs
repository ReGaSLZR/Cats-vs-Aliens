namespace ReGaSLZR.Gameplay.Model
{
    using Enum;
    using Util;

    using System.Collections.Generic;
    using UniRx;
    using Zenject;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Enemy Model",
        menuName = "Project/Create Enemy Injectible Model")]
    public class EnemyUnitsModel : ScriptableObjectInstaller<EnemyUnitsModel>,
        ITeam.IEnemyGetter, ITeam.IEnemySetter
    {

        #region Inspector Fields

        [SerializeField]
        private List<Unit> startingUnits = new List<Unit>();

        #endregion

        #region Private Fields

        protected readonly CompositeDisposable disposables = new CompositeDisposable();

        protected readonly ReactiveProperty<TeamStatus> rStatus
            = new ReactiveProperty<TeamStatus>();

        #endregion

        #region MonoInstaller Implementations

        public override void InstallBindings()
        {
            InitValues(); 

            Container.Bind<ITeam.IEnemyGetter>().FromInstance(this);
            Container.Bind<ITeam.IEnemySetter>().FromInstance(this);
        }

        #endregion

        #region Unity Callbacks

        private void OnDestroy()
        {
            disposables.Clear();
        }

        #endregion

        #region Class Implementation

        protected virtual void InitValues()
        {
            rStatus.Value = TeamStatus.NotReady;
        }

        #endregion
        
        #region Setter Implementation

        public void SetStatus(TeamStatus playerStatus)
        {
            rStatus.Value = playerStatus;
        }

        #endregion

        #region Getter Implementation

        public List<Unit> GetRawStartingUnits()
        {
            return startingUnits;
        }

        public IReadOnlyReactiveProperty<TeamStatus> GetStatus()
        {
            return rStatus;
        }

        #endregion

    }

}