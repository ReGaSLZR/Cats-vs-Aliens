namespace ReGaSLZR.Base
{

    using UnityEngine;

    public interface IInstantiator
    {
        void InjectPrefab(GameObject prefab);
        GameObject InstantiateInjectPrefab(GameObject prefab, Transform parent);
    }

}