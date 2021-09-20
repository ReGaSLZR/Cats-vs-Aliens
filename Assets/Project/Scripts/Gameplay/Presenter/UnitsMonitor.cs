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
        private readonly ITeam.IEnemySetter iEnemySetter;

        [Inject]
        private readonly ITeam.IPlayerGetter iPlayerGetter;

        [Inject]
        private readonly ITeam.IPlayerSetter iPlayerSetter;

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
            iEnemySetter.SetStatus(TeamStatus.InPlay);
            iPlayerSetter.SetStatus(TeamStatus.InPlay);
            iLevelSetter.SetState(LevelState.InPlay);

            SetUpUnitsObserver();
        }

        private void SetUpUnitsObserver()
        {
            var units = iSequenceGetter.GetUnits();
            foreach (var unit in units)
            {
                if (unit != null)
                {
                    unit.Data.GetCurrentHp()
                        .Where(hp => hp <= 0)
                        .Subscribe(_ => OnUnitDeath())
                        .AddTo(disposablesBasic);
                }
            }
        }

        private void OnUnitDeath()
        {
            LogUtil.PrintInfo(GetType(), "OnUnitDeath() called.");
            var units = iSequenceGetter.GetUnits();
            var alivePlayers = 0;
            var aliveEnemies = 0;

            foreach (var unit in units)
            {
                if (unit != null && unit.Data.GetCurrentHp().Value > 0)
                {
                    if (unit.Data.Team == Team.Player)
                    {
                        alivePlayers++;
                    }
                    else 
                    {
                        aliveEnemies++;
                    }
                }
            }

            iEnemySetter.SetStatus(aliveEnemies == 0 
                ? TeamStatus.WipedOut : TeamStatus.InPlay);
            iPlayerSetter.SetStatus(alivePlayers == 0
                ? TeamStatus.WipedOut : TeamStatus.InPlay);

            if (aliveEnemies == 0 || alivePlayers == 0)
            {
                iLevelSetter.SetState(LevelState.Ended);
            }
        }

    }

}