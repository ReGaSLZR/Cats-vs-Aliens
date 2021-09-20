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
            StartCoroutine(CorMoveStationary());
        }

        #endregion

        #region Class Implementation

        private IEnumerator CorMoveRandom()
        {
            yield return new WaitForSeconds(unitController.Unit.Data.AIMoveDelay);
            SetPosition(iTileGetter.GetRandomTile(currentTile));
            yield return new WaitForSeconds(unitController.Unit.Data.AIMoveDelay);
            FinishMove();
        }

        private IEnumerator CorMoveStationary()
        {
            yield return new WaitForSeconds(unitController.Unit.Data.AIMoveDelay);
            FinishMove();
        }

        #endregion
    }

}