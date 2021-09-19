namespace ReGaSLZR.Gameplay.Presenter.Movement
{

    using Enum;
    using Model;

    using UnityEngine;
    using UniRx;
    using UniRx.Triggers;
    using Zenject;
    
    public class PlayerMovement : BaseMovement
    {

        #region Private Fields

        [Inject]
        private readonly ITile.IGetter iTilesGetter;

        #endregion

        #region Class Overrides

        protected override void RegisterObservables()
        {
            RegisterMovement(MoveDirection.Up);
            RegisterMovement(MoveDirection.Down);
            RegisterMovement(MoveDirection.Left);
            RegisterMovement(MoveDirection.Right);
        }

        #endregion

        #region Class Implementation

        private void RegisterMovement(MoveDirection direction)
        {
            this.UpdateAsObservable()
                .Where(_ => Input.GetButtonDown(
                    direction.ToString()))
                .Select(_ => iTilesGetter.GetTile(
                    currentTile, direction))
                .Where(destination => (destination != null))
                .Subscribe(SetPosition)
                .AddTo(disposablesBasic);
        }

        #endregion
    }

}