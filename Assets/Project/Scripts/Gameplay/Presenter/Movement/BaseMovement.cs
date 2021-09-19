namespace ReGaSLZR.Gameplay.Presenter.Movement
{

    using Base;
    using Model;
    using Util;

    using UniRx;
    using NaughtyAttributes;
    using UnityEngine;
    using Zenject;

    [RequireComponent(typeof(Model.Unit))]
    public abstract class BaseMovement : BaseReactiveMonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        protected Tile currentTile;

        [SerializeField]
        [ReadOnly]
        protected Model.Unit unit;

        #endregion

        #region Private Fields

        [Inject]
        private ISequence.IGetter iSequenceGetter;

        [Inject]
        private ISequence.ISetter iSequenceSetter;

        #endregion

        protected abstract void OnMove();

        #region Unity Callbacks

        private void Awake()
        {
            unit = GetComponent<Model.Unit>();
        }

        #endregion

        #region Class Overrides

        private void Start()
        {
            iSequenceGetter.GetSequencedUnit()
                .Where(unit => (unit != null) &&
                        (unit.GetInstanceID() == this.unit.GetInstanceID()))
                .Subscribe(_ => OnMove())
                .AddTo(disposablesBasic);
        }

        #endregion

        #region Class Implementation

        protected void FinishMove()
        {
            iSequenceSetter.FinishSequence(unit);
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