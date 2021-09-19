﻿namespace ReGaSLZR.Gameplay.Model
{

    using Enum;
    using Util;

    using System.Collections.Generic;
    using UniRx;
    using Zenject;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Tile Model", menuName = "Project/Create Tile Model")]
    public class TileModel : ScriptableObjectInstaller<TileModel>,
        ITiles.ISetter, ITiles.IGetter
    {

        #region Private Fields

        private readonly ReactiveProperty<List<Tile>> rTiles 
            = new ReactiveProperty<List<Tile>>();

        #endregion

        public override void InstallBindings()
        {
            InitValues();

            Container.Bind<ITiles.IGetter>().FromInstance(this);
            Container.Bind<ITiles.ISetter>().FromInstance(this);
        }

        #region Class Implementation

        private void InitValues()
        {
            rTiles.Value = new List<Tile>();
        }

        private Tile HasTileAt(Vector3 position)
        {
            var tiles = rTiles.Value;
            foreach (var tile in tiles)
            {
                if (tile.Position.Equals(position))
                {
                    return tile;
                }
            }

            return null;
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

        public Tile GetTile(Tile origin, MoveDirection direction)
        {
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

            return HasTileAt(desiredPosition);
        }

        #endregion

    }

}