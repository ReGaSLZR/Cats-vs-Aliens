namespace ReGaSLZR.Gameplay.Model
{

	using Base;

	using NaughtyAttributes;
    using UnityEngine;
	using Zenject;

	[RequireComponent(typeof(SpriteRenderer))]
    public class Tile : BaseReactiveMonoBehaviour
    {

		public bool isOccupied;

		#region Inspector Fields

		[SerializeField]
        private Vector3 gridSize = new Vector3(1,1,1);

		[SerializeField]
		[Required]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private Color spriteColor;

		[SerializeField]
		private bool showInRuntime;

		#endregion

		#region Accessors

		public Vector3 Size => gridSize;

		public Vector3 Position => gameObject.transform.position;

		public bool ShowInRuntime => showInRuntime;

        #endregion

        #region Private Fields

        [Inject]
		private readonly ITile.ISetter iTilesSetter;

        #endregion

        #region Unity Callbacks

        private void OnDrawGizmos()
		{
			if (!Application.isPlaying && transform.hasChanged)
			{
				spriteRenderer.color = spriteColor;
				SnapToGrid();
			}
		}

        private void Start()
        {
			spriteRenderer.enabled = showInRuntime;
		}

        #endregion

        #region Class Overrides

        protected override void RegisterObservables()
		{
			if (showInRuntime)
			{ 
				iTilesSetter.AddTile(this);
			}
		}


		#endregion

		#region Class Implementation

		protected virtual void SnapToGrid()
		{
			var position = new Vector3(
				Mathf.Round(transform.position.x / gridSize.x) * gridSize.x,
				Mathf.Round(transform.position.y / gridSize.y) * gridSize.y,
				Mathf.Round(transform.position.z / gridSize.z) * gridSize.z
			);

			transform.position = position;
			gameObject.name = "Tile " + position.x + ":" + position.y;
		}

        #endregion

    }

}