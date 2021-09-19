namespace ReGaSLZR.Gameplay.Model
{
    
    using UnityEngine;

    public static class ICamera
    {

        public interface ISetter
        {
            void SetFocusTarget(Transform focus);
        }

        public interface IGetter
        {
            Transform GetFocusTarget();
        }
        
    }

}