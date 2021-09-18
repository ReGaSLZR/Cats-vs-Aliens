namespace ReGaSLZR.Base
{

    using UnityEngine;
    using UniRx;

    /// <summary>
    /// My base MonoBehaviour class that I use for
    /// reactive components. - Ren
    /// </summary>
    public abstract class BaseReactiveMonoBehaviour : MonoBehaviour
    {

        #region Protected Variables

        protected CompositeDisposable disposablesBasic
            = new CompositeDisposable();

        protected CompositeDisposable disposablesTerminal
            = new CompositeDisposable();

        #endregion

        #region Unity Callbacks

        protected virtual void OnEnable()
        {
            RegisterObservables();
        }

        protected virtual void OnDisable()
        {
            disposablesBasic.Clear();
        }

        protected virtual void OnDestroy()
        {
            disposablesTerminal.Clear();
        }

        #endregion

        protected abstract void RegisterObservables();

    }

}