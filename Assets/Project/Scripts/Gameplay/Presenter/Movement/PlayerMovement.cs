namespace ReGaSLZR.Gameplay.Presenter.Movement
{

    using Enum;
    using Model;

    using UnityEngine;
    using UniRx;
    using UniRx.Triggers;
    using Zenject;
    using System.Collections;

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
                //.Where(_ => iLevelGetter.GetState().Value == LevelState.InPlay)
                .Where(_ => Input.GetButtonDown(direction.ToString()))
                .Select(_ => iTilesGetter.GetTile(
                    unitController.Unit.currentTile, direction))
                .Where(destination => (destination != null))
                .Subscribe(destination =>
                {
                    isMovementAllowed = false;
                    StopAllCoroutines();
                    StartCoroutine(CorMove(destination));
                    disposablesBasic.Clear();
                })
                .AddTo(disposablesBasic);
        }

        private IEnumerator CorMove(Tile destination)
        {
            iLevelSetter.SetLog($"<color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.LogDanger)}><color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.PlayerUnitBG)}>{unitController.Unit.Data.DisplayName}</color> moved to {destination.gameObject.name}.</color>");
            SetPosition(destination);
            yield return null;
            FinishMove();
        }

        #endregion
    }

}