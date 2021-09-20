namespace ReGaSLZR.Gameplay.Presenter.Action
{

    using Base;

    using NaughtyAttributes;
    using UniRx;
    using UnityEngine;

    [RequireComponent(typeof(Model.Unit))]
    [RequireComponent(typeof(UnitTurnController))]
    public abstract class BaseAction : BaseReactiveMonoBehaviour
    {
        
        [SerializeField]
        [ReadOnly]
        protected UnitTurnController unitController;

        protected abstract void OnAct();

        private void Awake()
        {
            unitController = GetComponent<UnitTurnController>();
        }

        private void Start()
        {
            unitController.GetIsActive()
                .Where(isActive => isActive)
                .Where(_ => !unitController.GetIsActionFinished().Value)
                .Subscribe(_ => OnAct())
                .AddTo(disposablesTerminal);
        }

        protected void FinishAct()
        {
            unitController.SetIsActionFinished();
        }

    }

}