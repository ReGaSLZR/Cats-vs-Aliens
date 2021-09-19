﻿namespace ReGaSLZR.Gameplay.Presenter.Movement
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

        private bool isMovementAllowed;

        #endregion

        #region Class Overrides

        protected override void OnMove()
        {
            RegisterMovement(MoveDirection.Up);
            RegisterMovement(MoveDirection.Down);
            RegisterMovement(MoveDirection.Left);
            RegisterMovement(MoveDirection.Right);

            isMovementAllowed = true;
        }

        #endregion

        #region Class Implementation

        private void RegisterMovement(MoveDirection direction)
        {
            this.UpdateAsObservable()
                .Where(_ => isMovementAllowed)
                .Where(_ => Input.GetButtonDown(direction.ToString()))
                .Select(_ => iTilesGetter.GetTile(
                    currentTile, direction))
                .Where(destination => (destination != null))
                .Subscribe(destination =>
                {
                    isMovementAllowed = false;
                    SetPosition(destination);
                    FinishMove();
                })
                .AddTo(disposablesBasic);
        }

        #endregion
    }

}