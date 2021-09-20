namespace ReGaSLZR.Gameplay.Presenter
{
    using Base;
    using Model;

    using NaughtyAttributes;
    using UniRx;
    using UnityEngine;
    using Zenject;

    [RequireComponent(typeof(Model.Unit))]
    public class UnitTurnController : BaseReactiveMonoBehaviour
    {

        [SerializeField]
        [Required]
        private Model.Unit unit;

        [Inject]
        private readonly ISequence.IGetter iSequenceGetter;

        [Inject]
        private readonly ISequence.ISetter iSequenceSetter;

        private readonly ReactiveProperty<bool> rIsMoveFinished =
            new ReactiveProperty<bool>();

        private readonly ReactiveProperty<bool> rIsActionFinished =
            new ReactiveProperty<bool>();

        private readonly ReactiveProperty<bool> rIsActive
            = new ReactiveProperty<bool>();

        public Model.Unit Unit => unit;

        #region Unity Callbacks

        private void Start()
        {
            iSequenceGetter.GetActiveUnit()
                .Where(unit => (unit != null))
                .Subscribe(unit => 
                    rIsActive.Value = (unit.GetInstanceID() == this.unit.GetInstanceID()))
                .AddTo(disposablesTerminal);

            rIsActive.Where(isActive => isActive)
                .Subscribe(_ => 
                {
                    rIsMoveFinished.Value = false;
                    rIsActionFinished.Value = false;
                })
                .AddTo(disposablesTerminal);

            rIsActionFinished
                .Where(isActionFinished => isActionFinished)
                .Where(_ => rIsMoveFinished.Value)
                .Subscribe(_ => OnTurnFinished())
                .AddTo(disposablesTerminal);

            rIsMoveFinished
                .Where(isMoveFinished => isMoveFinished)
                .Where(_ => rIsActionFinished.Value)
                .Subscribe(_ => OnTurnFinished())
                .AddTo(disposablesTerminal);
        }

        #endregion

        private void OnTurnFinished()
        {
            iSequenceSetter.FinishSequence(Unit);
        }

        public IReadOnlyReactiveProperty<bool> GetIsActive()
        {
            return rIsActive;
        }

        public IReadOnlyReactiveProperty<bool> GetIsMoveFinished()
        {
            return rIsMoveFinished;
        }

        public IReadOnlyReactiveProperty<bool> GetIsActionFinished()
        {
            return rIsActionFinished;
        }

        public void SetIsMoveFinished()
        {
            rIsMoveFinished.Value = true;
        }

        public void SetIsActionFinished()
        {
            rIsActionFinished.Value = true;
        }

    }

}