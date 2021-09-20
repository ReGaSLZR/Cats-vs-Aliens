namespace ReGaSLZR.Gameplay.Model
{

    using Enum;
    using Util;

    using System.Collections.Generic;
    using UniRx;
    using Zenject;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Tile Model", menuName = "Project/Create Tile Model")]
    public class TileModel : ScriptableObjectInstaller<TileModel>,
        ITile.ISetter, ITile.IGetter
    {

        #region Private Fields

        private readonly ReactiveProperty<List<Tile>> rTiles 
            = new ReactiveProperty<List<Tile>>();

        #endregion

        public override void InstallBindings()
        {
            InitValues();

            Container.Bind<ITile.IGetter>().FromInstance(this);
            Container.Bind<ITile.ISetter>().FromInstance(this);
        }

        #region Class Implementation

        private void InitValues()
        {
            rTiles.Value = new List<Tile>();
        }

        #endregion

        #region Setter Implementation

        public void AddTile(Tile tile)
        {
            if (tile == null)
            {
                LogUtil.PrintWarning(GetType(), $"AddTile(): tile is NULL. Skipping...");
                return;
            }

            rTiles.Value.Add(tile);
        }

        #endregion

        #region Getter Implementation

        public IReadOnlyReactiveProperty<List<Tile>> GetTiles()
        {
            return rTiles;
        }

        public Tile GetTile(Tile origin, MoveDirection direction, bool bypassOccupied = false)
        {
            if (origin == null)
            {
                return null;
            }

            var desiredPosition = origin.Position;

            switch (direction)
            {
                case MoveDirection.Up:
                    {
                        desiredPosition.y += origin.Size.y;
                        break;
                    }
                case MoveDirection.Down:
                    {
                        desiredPosition.y -= origin.Size.y;
                        break;
                    }
                case MoveDirection.Left:
                    {
                        desiredPosition.x -= origin.Size.x;
                        break;
                    }
                case MoveDirection.Right:
                    {
                        desiredPosition.x += origin.Size.x;
                        break;
                    }
            }

            return GetTileAt(desiredPosition, bypassOccupied);
        }

        public Tile GetTileAt(Vector3 position, bool bypassOccupied = false)
        {
            var tiles = rTiles.Value;
            foreach (var tile in tiles)
            {
                if (tile.Position.Equals(position) && 
                    (bypassOccupied || (!bypassOccupied && !tile.isOccupied)))
                {
                    return tile;
                }
            }

            return null;
        }

        public Tile GetRandomTile(Tile origin, bool preferredOccupied = false)
        {
            var triedDirections = new List<MoveDirection>();
            var directions = System.Enum.GetNames(typeof(MoveDirection));
            
            while(triedDirections.Count < directions.Length)
            {
                var dir = directions[Random.Range(0, directions.Length)];
                System.Enum.TryParse(dir, out MoveDirection moveDir);

                if (!triedDirections.Contains(moveDir))
                {
                    triedDirections.Add(moveDir);

                    var tile = GetTile(origin, moveDir, preferredOccupied);
                    if (tile != null)
                    {
                        return tile;
                    }
                }
            }

            return null;
        }

        public bool IsTileOnCrossRange(Tile origin, Tile target)
        {
            if (origin == null || target == null)
            {
                return false;
            }

            var tiles = GetTilesOnCrossRange(origin);
            return tiles.Contains(target);
        }

        public List<Tile> GetTilesOnCrossRange(Tile origin)
        {
            var tiles = new List<Tile>();
            tiles.Add(GetTile(origin, MoveDirection.Up, true));
            tiles.Add(GetTile(origin, MoveDirection.Down, true));
            tiles.Add(GetTile(origin, MoveDirection.Left, true));
            tiles.Add(GetTile(origin, MoveDirection.Right, true));

            return tiles;
        }

        #endregion

    }

}