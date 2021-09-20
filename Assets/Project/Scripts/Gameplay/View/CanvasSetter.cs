namespace ReGaSLZR.Gameplay.View
{

    using NaughtyAttributes;
    using UnityEngine;

    public class CanvasSetter : MonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        [Required]
        private Canvas canvas;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            canvas.worldCamera = Camera.main;
        }

        #endregion

    }

}