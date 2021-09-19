namespace ReGaSLZR.Gameplay.Model
{
    using UnityEngine;
    using Zenject;

    [CreateAssetMenu(fileName = "Theme Color Model", menuName = "Project/Create Theme Color Model")]
    public class ThemeColorModel : ScriptableObjectInstaller<ThemeColorModel>
    {

        [SerializeField]
        private ThemeColors color;

        public override void InstallBindings()
        {
            Container.BindInstances(color);
        }

    }

}