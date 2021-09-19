namespace ReGaSLZR.Gameplay.Presenter.Movement
{

    using Base;
    using Model;

    using UnityEngine;

    public abstract class BaseMovement : BaseReactiveMonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        protected Tile currentTile;

        #endregion

        #region Class Implementation

        public void SetPosition(Tile tile)
        {
            if (tile == null)
            {
                return;
            }

            if (currentTile != null)
            {
                currentTile.isOccupied = false;
            }
            
            transform.position = tile.Position;
            currentTile = tile;
            tile.isOccupied = true;
        }

        #endregion
    }

}