namespace ReGaSLZR.Base
{

    using UnityEngine;

    /// <summary>
    /// Used for ensuring that dependencies models marked with [Inject] 
    /// are injected into GameObjects/MonoBehaviours instantiated/added
    /// during runtime.
    /// </summary>
    public interface IInstantiator
    {
        void InjectPrefab(GameObject prefab);
        GameObject InstantiateInjectPrefab(GameObject prefab, Transform parent);
    }

}