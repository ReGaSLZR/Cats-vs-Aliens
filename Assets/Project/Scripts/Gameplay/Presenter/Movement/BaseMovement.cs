namespace ReGaSLZR.Gameplay.Presenter.Movement
{

    using Base;
    using Model;

    using NaughtyAttributes;
    using UniRx;
    using UnityEngine;
    using Zenject;

    [RequireComponent(typeof(UnitTurnController))]
    public abstract class BaseMovement : BaseReactiveMonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        [ReadOnly]
        protected UnitTurnController unitController;

        #endregion

        #region Private Fields

        [Inject]
        protected readonly ISequence.IGetter iSequenceGetter;

        [Inject]
        protected readonly ISequence.ISetter iSequenceSetter;

        [Inject]
        protected readonly ILevel.IGetter iLevelGettter;

        [Inject]
        protected readonly ILevel.ISetter iLevelSetter;

        [Inject]
        protected readonly ThemeColors iThemeColors;

        #endregion

        protected abstract void OnMove();

        #region Unity Callbacks

        private void Awake()
        {
            unitController = GetComponent<UnitTurnController>();
        }

        private void Start()
        {
            unitController.GetIsActive()
                .Where(isActive => isActive)
                .Where(_ => unitController.GetIsMoveAllowed().Value)
                .Subscribe(_ => OnMove())
                .AddTo(disposablesTerminal);

            iLevelGettter.GetState()
                .Where(state => state == Enum.LevelState.Ended)
                .Subscribe(_ => Destroy(this))
                .AddTo(disposablesTerminal);
        }

        #endregion

        #region Class Implementation

        protected void FinishMove()
        {
            unitController.SetIsMoveFinished();
        }

        public void SetPosition(Tile tile)
        {
            if (tile == null)
            {
                return;
            }

            if (unitController.Unit.currentTile != null)
            {
                unitController.Unit.currentTile.isOccupied = false;
            }
            
            transform.position = tile.Position;
            unitController.Unit.currentTile = tile;
            tile.isOccupied = true;
        }

        #endregion
    }

}