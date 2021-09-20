namespace ReGaSLZR.Gameplay.Model
{
    using Enum;
    using Util;

    using UniRx;
    using Zenject;

    public class LevelModel : MonoInstaller<LevelModel>,
        ILevel.IGetter, ILevel.ISetter
    {

        #region Private Fields

        protected readonly CompositeDisposable disposables
            = new CompositeDisposable();

        private readonly ReactiveProperty<LevelState> rState 
            = new ReactiveProperty<LevelState>();

        private readonly ReactiveProperty<string> rCurrentLog =
            new ReactiveProperty<string>();

        private readonly ReactiveProperty<Unit> rSelectedUnit
            = new ReactiveProperty<Unit>();

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            InitValues();
        }

        private void OnDestroy()
        {
            disposables.Clear();
        }

        #endregion

        #region Class Overrides

        public override void InstallBindings()
        {
            Container.Bind<ILevel.IGetter>().FromInstance(this);
            Container.Bind<ILevel.ISetter>().FromInstance(this);
        }

        #endregion

        #region Class Implementation

        private void InitValues()
        {
            rState.Value = LevelState.NotStarted;
            rCurrentLog.Value = string.Empty;
        }

        #endregion

        #region Setter Implementation
        public void SetLog(string log)
        {
            if (string.IsNullOrEmpty(log))
            {
                LogUtil.PrintWarning(GetType(), "SetLog(): Invalid log!");
                return;
            }

            rCurrentLog.Value = log;
        }

        public void SetState(LevelState state)
        {
            rState.Value = state;
        }

        public void SetSelectedUnit(Unit unit)
        {
            rSelectedUnit.SetValueAndForceNotify(unit);
        }

        #endregion

        #region Getter Implementation

        public IReadOnlyReactiveProperty<string> GetCurrentLog()
        {
            return rCurrentLog;
        }

        public IReadOnlyReactiveProperty<LevelState> GetState()
        {
            return rState;
        }
        public IReadOnlyReactiveProperty<Unit> GetSelectedUnit()
        {
            return rSelectedUnit;
        }

        #endregion

    }

}