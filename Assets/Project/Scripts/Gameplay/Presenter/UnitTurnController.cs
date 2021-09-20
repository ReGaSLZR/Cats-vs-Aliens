namespace ReGaSLZR.Gameplay.Presenter
{
    using Base;
    using Model;
    using Util;

    using NaughtyAttributes;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    [RequireComponent(typeof(Model.Unit))]
    public class UnitTurnController : BaseReactiveMonoBehaviour
    {

        [SerializeField]
        [Required]
        private Model.Unit unit;

        [SerializeField]
        [Required]
        private Button button;

        [Inject]
        private readonly ISequence.IGetter iSequenceGetter;

        [Inject]
        private readonly ISequence.ISetter iSequenceSetter;

        [Inject]
        private readonly ILevel.ISetter iLevelSetter;

        private readonly ReactiveProperty<bool> rIsMoveAllowed =
            new ReactiveProperty<bool>();

        private readonly ReactiveProperty<bool> rIsActionAllowed =
            new ReactiveProperty<bool>();

        private readonly ReactiveProperty<bool> rIsActive
            = new ReactiveProperty<bool>();

        public Model.Unit Unit => unit;

        #region Unity Callbacks

        private void Start()
        {
            InitTerminalObservers();
            InitButtonObservers();

            var activeUnit = iSequenceGetter.GetActiveUnit().Value;
            OnIsActive((activeUnit != null) && unit.GetInstanceID() == 
                activeUnit.GetInstanceID());
        }

        #endregion

        private void InitButtonObservers()
        {
            button.onClick.AddListener(() => {
                iLevelSetter.SetSelectedUnit(Unit);
                
                if (rIsActionAllowed.Value)
                {
                    LogUtil.PrintInfo(gameObject, GetType(), 
                        "On Self ButtonClick(): Skipped Action.");
                    SetIsActionFinished();
                }
            });
        }

        private void InitTerminalObservers()
        {
            unit.Data.GetCurrentHp()
                .Where(hp => hp <= 0)
                .Subscribe(_ =>
                {
                    unit.currentTile.isOccupied = false;
                    Destroy(gameObject, 0.25f);
                })
                .AddTo(disposablesTerminal);

            rIsActive.Subscribe(OnIsActive)
                    .AddTo(disposablesTerminal);

            iSequenceGetter.GetActiveUnit()
                .Where(unit => (unit != null))
                .Subscribe(unit =>
                    rIsActive.Value = (unit.GetInstanceID() == this.unit.GetInstanceID()))
                .AddTo(disposablesTerminal);

            rIsActionAllowed
                .Where(actionAllowed => !actionAllowed)
                .Where(_ => !rIsMoveAllowed.Value)
                .Subscribe(_ => OnTurnFinished())
                .AddTo(disposablesTerminal);

            rIsMoveAllowed
                .Where(moveAllowed => !moveAllowed)
                .Where(_ => !rIsActionAllowed.Value)
                .Subscribe(_ => OnTurnFinished())
                .AddTo(disposablesTerminal);
        }

        private void OnIsActive(bool isActive)
        {
            if (isActive)
            {
                rIsMoveAllowed.Value = true;
                rIsActionAllowed.Value = true;
                //rIsActionAllowed.Value = (unit.Data.Team == Enum.Team.Player);
            }
        }

        private void OnTurnFinished()
        {
            rIsActive.Value = false;
            rIsMoveAllowed.Value = false;
            rIsActionAllowed.Value = false;

            iSequenceSetter.FinishSequence(Unit);
        }

        public IReadOnlyReactiveProperty<bool> GetIsActive()
        {
            return rIsActive;
        }

        public IReadOnlyReactiveProperty<bool> GetIsMoveAllowed()
        {
            return rIsMoveAllowed;
        }

        public IReadOnlyReactiveProperty<bool> GetIsActionAllowed()
        {
            return rIsActionAllowed;
        }

        public void SetIsMoveFinished()
        {
            rIsMoveAllowed.Value = false;

            //if ((unit.Data.Team != Enum.Team.Player))
            //{
            //    rIsActionAllowed.Value = true;
            //}
        }

        public void SetIsActionFinished()
        {
            rIsActionAllowed.Value = false;
        }

    }

}