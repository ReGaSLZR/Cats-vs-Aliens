namespace ReGaSLZR.Gameplay.Model
{

	using Base;
    using UnityEngine;
	using Zenject;

    public class Tile : BaseReactiveMonoBehaviour
    {

		#region Inspector Fields

		[SerializeField]
        private Vector3 gridSize = new Vector3(1,1,1);

		#endregion

		#region Accessors

		public Vector3 Size => gridSize;

		public Vector3 Position => gameObject.transform.position;

        #endregion

        #region Private Fields

        [Inject]
		private readonly ITiles.ISetter iTilesSetter;

		#endregion

		#region Unity Callbacks

		private void OnDrawGizmos()
		{
			if (!Application.isPlaying && transform.hasChanged)
			{
				SnapToGrid();
			}
		}

		#endregion

		#region Class Overrides

		protected override void RegisterObservables()
		{
			if (iTilesSetter == null)
			{
				Debug.Log($"REN");
				return;
			}
			iTilesSetter.AddTile(this);
		}


		#endregion

		#region Class Implementation

		private void SnapToGrid()
		{
			var grid = gridSize;
			var position = new Vector3(
				Mathf.Round(transform.position.x / grid.x) * grid.x,
				Mathf.Round(transform.position.y / grid.y) * grid.y,
				Mathf.Round(transform.position.z / grid.z) * grid.z
			);

			transform.position = position;
		}

        #endregion

    }

}