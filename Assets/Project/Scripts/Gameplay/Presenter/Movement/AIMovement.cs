namespace ReGaSLZR.Gameplay.Presenter.Movement
{

    using Enum;
    using Model;
    using Util;

    using System.Collections;
    using UnityEngine;
    using Zenject;

    public class AIMovement : BaseMovement
    {

        [Inject]
        private readonly ITile.IGetter iTileGetter;

        #region Class Overrides
        protected override void OnMove()
        {
            StopAllCoroutines();

            switch (unitController.Unit.Data.AIMove)
            {
                case MoveOptionAI.Random:
                    {
                        StartCoroutine(CorMoveRandom());
                        break;
                    }
                case MoveOptionAI.Stationary:
                default:
                    {
                        StartCoroutine(CorMoveStationary());
                        break;
                    }
            }
        }

        #endregion

        #region Class Implementation

        private IEnumerator CorMoveRandom()
        {
            yield return new WaitForSeconds(unitController.Unit.Data.AIMoveDelay);
            SetPosition(iTileGetter.GetRandomTile(unitController.Unit.currentTile));
            iLevelSetter.SetLog($"<color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.LogCritical)}>" +
                $"<color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.EnemyUnitBG)}>{unitController.Unit.Data.DisplayName}</color> " +
                $"moved randomly to {unitController.Unit.currentTile.gameObject.name}.</color>");
            yield return new WaitForSeconds(unitController.Unit.Data.AIMoveDelay);
            LogUtil.PrintInfo(GetType(), "CorMoveRandom()");
            FinishMove();
        }

        private IEnumerator CorMoveStationary()
        {
            yield return new WaitForSeconds(unitController.Unit.Data.AIMoveDelay);
            LogUtil.PrintInfo(GetType(), "CorMoveStationary()");
            iLevelSetter.SetLog($"<color=#{ColorUtility.ToHtmlStringRGB(iThemeColors.EnemyUnitBG)}>{unitController.Unit.Data.DisplayName}</color> didn't move at all.");
            FinishMove();
        }

        #endregion
    }

}