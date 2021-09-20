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
        protected Tile currentTile;

        [SerializeField]
        [ReadOnly]
        protected UnitTurnController unitController;

        #endregion

        #region Private Fields

        [Inject]
        protected readonly ISequence.IGetter iSequenceGetter;

        [Inject]
        protected readonly ISequence.ISetter iSequenceSetter;

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

            if (currentTile != null)
            {
                currentTile.isOccupied = false;
            }
            
            transform.position = tile.Position;
            currentTile = tile;
            tile.isOccupied = true;
        }

        #endregion
    }

}