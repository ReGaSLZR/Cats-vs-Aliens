namespace ReGaSLZR.Gameplay.Presenter
{
    using Base;
    using Enum;
    using Model;
    using Util;

    using UniRx;
    using UnityEngine;
    using Zenject;

    public class UnitsMonitor : BaseReactiveMonoBehaviour
    {

        [Inject]
        private readonly ISequence.ISetter iSequenceSetter;

        [Inject]
        private readonly ISequence.IGetter iSequenceGetter;

        [Inject]
        private readonly ITeam.IEnemyGetter iEnemyGetter;

        [Inject]
        private readonly ITeam.IPlayerGetter iPlayerGetter;

        [Inject]
        private readonly ILevel.IGetter iLevelGetter;

        [Inject]
        private readonly ILevel.ISetter iLevelSetter;

        private bool isSequencingDone = false;

        protected override void RegisterObservables()
        {
            iEnemyGetter.GetStatus()
                .Where(status => status == TeamStatus.InPlay)
                .Where(_ => iPlayerGetter.GetStatus().Value == TeamStatus.InPlay)
                .Subscribe(_ => SetUpSequence())
                .AddTo(disposablesBasic);

            iPlayerGetter.GetStatus()
                .Where(status => status == TeamStatus.InPlay)
                .Where(_ => iEnemyGetter.GetStatus().Value == TeamStatus.InPlay)
                .Subscribe(_ => SetUpSequence())
                .AddTo(disposablesBasic);
        }

        private void SetUpSequence()
        {
            if (isSequencingDone)
            {
                return;
            }

            isSequencingDone = true;
            LogUtil.PrintInfo(GetType(), "SetUpSequence()");

            iSequenceSetter.OrganizeSequence();
            iLevelSetter.SetState(LevelState.InPlay);
        }

        //private void OnUnitDeath()
        //{
        //    LogUtil.PrintInfo(GetType(), "OnUnitDeath() called.");
        //    var units = rUnits.Value;
        //    var deadUnitsCount = 0;

        //    foreach (var unit in units)
        //    {
        //        if (unit != null && unit.Data.GetCurrentHp().Value <= 0)
        //        {
        //            deadUnitsCount++;
        //        }
        //    }

        //    if (deadUnitsCount == units.Count)
        //    {
        //        rStatus.Value = TeamStatus.WipedOut;

        //    }
        //}

    }

}