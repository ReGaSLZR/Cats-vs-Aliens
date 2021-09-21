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

        #region Inspector Fields

        [SerializeField]
        [ReadOnly]
        private Model.Unit unit;

        [SerializeField]
        [Required]
        private Button button;

        #endregion

        #region Private Fields

        [Inject]
        private readonly ISequence.IGetter iSequenceGetter;

        [Inject]
        private readonly ISequence.ISetter iSequenceSetter;

        [Inject]
        private readonly ILevel.IGetter iLevelGetter;

        [Inject]
        private readonly ILevel.ISetter iLevelSetter;

        private readonly ReactiveProperty<bool> rIsMoveAllowed =
            new ReactiveProperty<bool>();

        private readonly ReactiveProperty<bool> rIsActionAllowed =
            new ReactiveProperty<bool>();

        private readonly ReactiveProperty<bool> rIsActive
            = new ReactiveProperty<bool>();

        [Inject]
        private readonly ThemeColors iThemeColors;

        #endregion

        public Model.Unit Unit => unit;

        #region Unity Callbacks

        private void Awake()
        {
            unit = GetComponent<Model.Unit>();
        }

        private void Start()
        {
            InitTerminalObservers();
            InitButtonObservers();

            var activeUnit = iSequenceGetter.GetActiveUnit().Value;
            //OnIsActive((activeUnit != null) && unit.GetInstanceID() == 
            //    activeUnit.GetInstanceID());
        }

        #endregion

        #region Class Overrides

        protected override void RegisterObservables()
        {
            unit.Data.GetCurrentHp()
                .Where(hp => hp <= 0)
                .Subscribe(_ =>
                {
                    unit.currentTile.isOccupied = false;
                    iLevelSetter.SetLog($"<b><color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.LogCritical)}>" +
                        $"{Unit.Data.DisplayName} died!</color></b>");
                    Destroy(gameObject, 0.25f);
                })
                .AddTo(disposablesBasic);
        }

        #endregion

        #region Class Implementation

        private void InitButtonObservers()
        {
            button.onClick.AddListener(() => {
                iLevelSetter.SetSelectedUnit(Unit);
                
                if (rIsActionAllowed.Value && Unit.Data.Team == Enum.Team.Player)
                {
                    LogUtil.PrintInfo(gameObject, GetType(), 
                        "On Self ButtonClick(): Skipped Action.");
                    iLevelSetter.SetLog($"<color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.PlayerUnitBG)}>" +
                        $"{Unit.Data.DisplayName}</color> finished turn.");
                    OnTurnFinished();
                }
            });
        }

        private void InitTerminalObservers()
        {
            rIsActive
                .Where(isActive => iLevelGetter.GetState().Value != Enum.LevelState.Ended)
                .Subscribe(OnIsActive)
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
                iLevelSetter.SetLog($"--------------------");
                iLevelSetter.SetLog($"<color=#" +
                    $"{ColorUtility.ToHtmlStringRGB(iThemeColors.GetTeamColor(Unit.Data.Team))}>" +
                    $"{Unit.Data.DisplayName}</color>'s turn.");
            }

            rIsMoveAllowed.Value = isActive;
            rIsActionAllowed.Value = isActive;   
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

        public void FinishMove()
        {
            rIsMoveAllowed.Value = false;
            
            if ((Unit.Data.Team == Enum.Team.Player) 
                && rIsActionAllowed.Value
                && iLevelGetter.GetState().Value != Enum.LevelState.Ended)
            {
                iLevelSetter.SetLog($"Awaiting action...");
            }
        }

        public void FinishAction()
        {
            rIsActionAllowed.Value = false;

            if ((Unit.Data.Team == Enum.Team.Player) 
                && rIsMoveAllowed.Value
                && iLevelGetter.GetState().Value != Enum.LevelState.Ended)
            {
                iLevelSetter.SetLog($"Awaiting move...");
            }
        }

        #endregion

    }

}