namespace ReGaSLZR.Gameplay.Model
{
    
    using Enum;

    using System.Collections.Generic;
    using UniRx;

    public static class ITiles
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
        }

    }

}
