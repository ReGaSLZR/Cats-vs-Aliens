namespace ReGaSLZR.Gameplay.Tool 
{
    using Enum;
    using Model;
    using Presenter.Spawner;
    using Util;

    using NaughtyAttributes;
    using UnityEngine;

    public class TileCreator : MonoBehaviour
    {

        #region Inspector Fields

        [Header("Prefab Tiles")]

        [SerializeField]
        [Required]
        private Tile prefabLevelTile;

        [SerializeField]
        [Required]
        private Tile prefabUnitTilePlayer;

        [SerializeField]
        [Required]
        private Tile prefabUnitTileEnemy;

        [Header("Parent GameObjects")]

        [SerializeField]
        [Required]
        private Transform parentLevelTiles;

        [SerializeField]
        [Required]
        private Transform parentUnitTiles;

        [Space]

        [SerializeField]
        private UnitSpawner unitSpawner;

        #endregion

        [Button("Spawn Level Tile")]
        private void CreateLevelTile()
        {
            CreateTile(parentLevelTiles, prefabLevelTile);
        }

        [Button("Spawn Player Tile")]
        private void CreatePlayerTile()
        {
            unitSpawner.AddSpawnTile(Team.Player, CreateTile(parentUnitTiles, prefabUnitTilePlayer));
        }

        [Button("Spawn Enemy Tile")]
        private void CreateEnemyTile()
        {
            unitSpawner.AddSpawnTile(Team.Enemy, CreateTile(parentUnitTiles, prefabUnitTileEnemy));
        }

        private Tile CreateTile(Transform parent, Tile prefab)
        {
            if (prefab == null)
            {
                LogUtil.PrintInfo(GetType(), "CreateTile(): " +
                    "Cannot proceed with NULL prefab.");
                return null;
            }

            var tile = Instantiate(prefab, parent);

#if UNITY_EDITOR
            UnityEditor.Selection.activeGameObject = tile.gameObject;
            UnityEditor.EditorGUIUtility.PingObject(tile.gameObject);
#endif
            return tile;
        }

        [Button("Clear Level Tiles")]
        private void ClearLevelTiles()
        {
            ClearChildren(parentLevelTiles);
        }

        [Button("Clear Spawner Tiles")]
        private void ClearSpawnerTiles()
        {
            ClearChildren(parentUnitTiles);
            unitSpawner.ClearSpawnTiles();
        }

        private void ClearChildren(Transform parent)
        {
            var childCount = parent.childCount;
            for (int x = (childCount - 1); x >= 0; x--)
            {
                DestroyImmediate(parent.GetChild(x).gameObject);
            }
        }

    }

}