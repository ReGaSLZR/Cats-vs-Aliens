namespace ReGaSLZR.Gameplay.Presenter.Action
{

    using Base;
    using Model;

    using NaughtyAttributes;
    using UniRx;
    using UnityEngine;
    using Zenject;

    [RequireComponent(typeof(Model.Unit))]
    [RequireComponent(typeof(UnitTurnController))]
    public abstract class BaseAction : BaseReactiveMonoBehaviour
    {
        
        [SerializeField]
        [ReadOnly]
        protected UnitTurnController unitController;

        [Inject]
        protected readonly ILevel.ISetter iLevelSetter;

        [Inject]
        private readonly ILevel.IGetter iLevelGettter;

        protected abstract void OnAct();

        private void Awake()
        {
            unitController = GetComponent<UnitTurnController>();
        }

        private void Start()
        {
            unitController.GetIsActive()
                .Where(isActive => isActive)
                .Where(_ => unitController.GetIsActionAllowed().Value)
                .Subscribe(_ => OnAct())
                .AddTo(disposablesTerminal);

            iLevelGettter.GetState()
                .Where(state => state == Enum.LevelState.Ended)
                .Subscribe(_ => Destroy(this))
                .AddTo(disposablesTerminal);
        }

        protected void FinishAct()
        {
            unitController.SetIsActionFinished();
        }

    }

}