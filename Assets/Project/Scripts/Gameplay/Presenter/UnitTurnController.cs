namespace ReGaSLZR.Gameplay.Presenter
{
    using Base;
    using Model;

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

        //[Space]

        //[SerializeField]
        //[Required]
        //private GameObject parentButtons;

        //[SerializeField]
        //[Required]
        //private Button buttonMove;

        //[SerializeField]
        //[Required]
        //private Button buttonAttack;

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

            //parentButtons.SetActive(false);
            var activeUnit = iSequenceGetter.GetActiveUnit().Value;
            OnIsActive((activeUnit != null) && unit.GetInstanceID() == 
                activeUnit.GetInstanceID());
        }

        #endregion

        private void InitButtonObservers()
        {
            button.onClick.AddListener(() => iLevelSetter.SetSelectedUnit(Unit));

            //if (Unit.Data.Team != Enum.Team.Player)
            //{
            //    return;
            //}

            //buttonAttack.onClick.AddListener(() =>
            //    rIsActionAllowed.Value = false);
            //buttonMove.onClick.AddListener(() =>
            //    rIsMoveAllowed.Value = false);
        }

        private void InitTerminalObservers()
        {
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
            }

            //if (Unit.Data.Team == Enum.Team.Player)
            //{
            //    parentButtons.SetActive(isActive);
            //    buttonMove.gameObject.SetActive(isActive);
            //    buttonAttack.gameObject.SetActive(isActive);
            //}
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
            //buttonMove.gameObject.SetActive(false);
        }

        public void SetIsActionFinished()
        {
            rIsActionAllowed.Value = false;
            //buttonAttack.gameObject.SetActive(false);
        }

    }

}