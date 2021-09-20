namespace ReGaSLZR.Gameplay.View
{

    using NaughtyAttributes;
    using UnityEngine;

    public class CanvasSetter : MonoBehaviour
    {

        [SerializeField]
        [Required]
        private Canvas canvas;

        private void Start()
        {
            canvas.worldCamera = Camera.main;
        }

    }

}