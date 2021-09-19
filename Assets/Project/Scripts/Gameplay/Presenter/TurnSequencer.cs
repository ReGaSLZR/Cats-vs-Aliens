namespace ReGaSLZR.Gameplay.Presenter
{
    using Base;
    using Enum;
    using Model;
    using Util;

    using UniRx;
    using UnityEngine;
    using Zenject;

    public class TurnSequencer : BaseReactiveMonoBehaviour
    {

        [Inject]
        private readonly ISequence.ISetter iSequenceSetter;

        [Inject]
        private readonly ITeam.IEnemyGetter iEnemyGetter;

        [Inject]
        private readonly ITeam.IPlayerGetter iPlayerGetter;

        [Inject]
        private readonly ILevel.IGetter iLevelGetter;

        [Inject]
        private readonly ILevel.ISetter iLevelSetter;

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
            LogUtil.PrintInfo(GetType(), "SetUpSequence()");

            iSequenceSetter.OrganizeSequence();
            iLevelSetter.SetState(LevelState.InPlay);
        }

    }

}