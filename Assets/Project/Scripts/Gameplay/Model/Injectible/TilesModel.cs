namespace ReGaSLZR.Gameplay.Model
{

    using Enum;
    using Util;

    using System.Collections.Generic;
    using UniRx;
    using Zenject;
    using UnityEngine;
    using System.Linq;

    [CreateAssetMenu(fileName = "Tiles Model", menuName = "Project/Create Tiles Model")]
    public class TilesModel : ScriptableObjectInstaller<TilesModel>, ITile.IGetter
    {

        #region Private Fields

        private readonly ReactiveDictionary<string, Tile> rDicTiles
            = new ReactiveDictionary<string, Tile>();

        private readonly ReactiveProperty<bool> rIsReady
            = new ReactiveProperty<bool>();

        #endregion

        public override void InstallBindings()
        {
            InitValues();
            Container.Bind<ITile.IGetter>().FromInstance(this);
        }

        #region Class Implementation

        private void InitValues()
        {
            rIsReady.Value = false;
            rDicTiles.Clear();

            var tilesVisibleInScene = FindObjectsOfType<Tile>().ToList()
                .Where(tile => tile.ShowInRuntime);

            foreach (var tile in tilesVisibleInScene)
            {
                AddTile(tile);
            }

            rIsReady.Value = true;
        }

        private void AddTile(Tile tile)
        {
            if (tile == null || rDicTiles.ContainsKey(tile.gameObject.name))
            {
                LogUtil.PrintWarning(GetType(), $"AddTile(): tile is NULL or " +
                    $"already registered. Skipping...");
                return;
            }

            rDicTiles.Add(tile.gameObject.name, tile);
        }

        #endregion

        #region Getter Implementation

        public IReadOnlyReactiveProperty<bool> IsReady()
        {
            return rIsReady;
        }

        public Tile GetTile(Tile origin, MoveDirection direction, 
            bool bypassOccupied = false)
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
            foreach (var tile in rDicTiles)
            {
                if (tile.Value.Position.Equals(position) && 
                    (bypassOccupied || (!bypassOccupied && !tile.Value.isOccupied)))
                {
                    return tile.Value;
                }
            }

            return null;
        }

        public Tile GetTileWithName(string name, bool bypassOccupied = false)
        {
            if (string.IsNullOrEmpty(name)) 
            {
                return null;
            }

            foreach (var tile in rDicTiles)
            {
                if (name.Equals(tile.Key) 
                    && (bypassOccupied || (!bypassOccupied && !tile.Value.isOccupied)))
                {
                    return tile.Value;
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