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

        protected readonly ReactiveProperty<List<Unit>> rUnits = new ReactiveProperty<List<Unit>>();

        protected readonly ReactiveProperty<bool> rIsInitFinished =
            new ReactiveProperty<bool>();

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
            rIsInitFinished.Value = false;
            rStatus.Value = TeamStatus.NotReady;
            rUnits.Value = new List<Unit>();

            ClearUnits();
            foreach (var unit in startingUnits)
            {
                AddUnit(unit);
            }

            rIsInitFinished.Value = true;
        }

        private void OnUnitDeath()
        {
            LogUtil.PrintInfo(GetType(), "OnUnitDeath() called.");
            var units = rUnits.Value;
            var deadUnitsCount = 0;

            foreach (var unit in units)
            {
                if (unit != null && unit.Data.GetCurrentHp().Value <= 0)
                {
                    deadUnitsCount++;
                }
            }

            if (deadUnitsCount == units.Count)
            {
                rStatus.Value = TeamStatus.WipedOut;
            }
        }

        #endregion
        
        #region Setter Implementation

        public void AddUnit(Unit unit)
        {
            if (unit == null)
            {
                LogUtil.PrintWarning(GetType(), "AddUnit(): unit is NULL. Skipping...");
                return;
            }

            if (unit.Data.StatMaxHp <= 0)
            {
                LogUtil.PrintError(GetType(), $"AddUnit(): unit " +
                    $"{unit.Data.DisplayName} has invalid Max HP! Skipping...");
                return;
            }

            unit.Data.GetCurrentHp()
                .Where(hp => hp <= 0)
                .Subscribe(_ => OnUnitDeath())
                .AddTo(disposables);

            rUnits.Value.Add(unit);
        }

        public void ClearUnits()
        {
            LogUtil.PrintInfo(GetType(), "ClearUnits() called.");
            rUnits.Value.Clear();
        }

        public void SetStatus(TeamStatus playerStatus)
        {
            rStatus.Value = playerStatus;
        }

        #endregion

        #region Getter Implementation

        public IReadOnlyReactiveProperty<List<Unit>> GetUnits()
        {
            return rUnits;
        }

        public IReadOnlyReactiveProperty<TeamStatus> GetStatus()
        {
            return rStatus;
        }

        public IReadOnlyReactiveProperty<bool> GetIsInitFinished()
        {
            return rIsInitFinished;
        }

        #endregion

    }

}