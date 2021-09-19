namespace ReGaSLZR.Base
{
    
    using UnityEngine;
    using Zenject;

    public class BaseInstantiator : MonoInstaller, IInstantiator
    {

        public override void InstallBindings()
        {
            Container.Bind<IInstantiator>().FromInstance(this);
        }

        #region Instantiator Implementation
        public void InjectPrefab(GameObject prefab)
        {
            Container.InjectGameObject(prefab);
        }

        public GameObject InstantiateInjectPrefab(GameObject prefab, Transform parent)
        {
            GameObject instantiatedObject = Instantiate(prefab, parent.position, parent.rotation);
            InjectPrefab(instantiatedObject);
            return instantiatedObject;
        }

        #endregion
    }

}