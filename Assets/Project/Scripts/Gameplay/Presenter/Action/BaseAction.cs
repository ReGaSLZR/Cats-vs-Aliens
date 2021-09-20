namespace ReGaSLZR.Gameplay.Presenter.Action
{
    using Base;
    using UnityEngine;

    [RequireComponent(typeof(Model.Unit))]
    [RequireComponent(typeof(UnitTurnController))]
    public abstract class BaseAction : BaseReactiveMonoBehaviour
    {
        
    }

}