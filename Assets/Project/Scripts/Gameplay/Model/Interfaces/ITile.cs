namespace ReGaSLZR.Gameplay.Model
{
    
    using Enum;

    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;

    public static class ITile
    {

        public interface ISetter
        {
            void AddTile(Tile tile);

        }

        public interface IGetter
        {
            IReadOnlyReactiveProperty<List<Tile>> GetTiles();

            ///<returns>The tile at the direction. Value is NULL if given direction is a dead-end.</returns>
            Tile GetTile(Tile origin, MoveDirection direction);
            Tile GetTileAt(Vector3 position);

            Tile GetRandomTile(Tile origin);
        }

    }

}
