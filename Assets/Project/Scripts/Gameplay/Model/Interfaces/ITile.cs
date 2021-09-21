namespace ReGaSLZR.Gameplay.Model
{
    
    using Enum;

    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;

    public static class ITile
    {

        public interface IGetter
        {
            IReadOnlyReactiveProperty<bool> IsReady();

            ///<returns>The tile at the direction. Value is NULL if given direction is a dead-end.</returns>
            Tile GetTile(Tile origin, MoveDirection direction, bool bypassOccupied = false);
            Tile GetTileAt(Vector3 position, bool bypassOccupied = false);
            Tile GetTileWithName(string name, bool bypassOccupied = false);
            Tile GetRandomTile(Tile origin, bool preferredOccupied = false);
            bool IsTileOnCrossRange(Tile origin, Tile target);
            List<Tile> GetTilesOnCrossRange(Tile origin);

        }

    }

}
