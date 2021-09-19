namespace ReGaSLZR.Gameplay.Presenter.Movement
{

    using Util;

    using System.Collections;
    using UnityEngine;

    public class AIMovement : BaseMovement
    {

        #region Class Overrides
        protected override void OnMove()
        {
            LogUtil.PrintInfo(GetType(), $"OnMove()");
            StartCoroutine(CorMove());
        }

        #endregion

        #region Class Implementation

        private IEnumerator CorMove()
        {
            yield return new WaitForSeconds(1f);
            FinishMove();
        }

        #endregion
    }

}