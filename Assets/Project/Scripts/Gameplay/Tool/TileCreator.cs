namespace ReGaSLZR.Gameplay.Tool 
{

    using Model;
    using Util;

    using NaughtyAttributes;
    using UnityEngine;

    public class TileCreator : MonoBehaviour
    {

        [SerializeField]
        [Required]
        private Tile prefabTile;

        [Button("Spawn Tile")]
        private void CreateTile()
        {
            if (prefabTile == null)
            {
                LogUtil.PrintInfo(GetType(), "CreateTile(): " +
                    "Cannot proceed with NULL prefab.");
                return;
            }

            var tile = Instantiate(prefabTile, gameObject.transform);
            tile.name = "Tile #" + (gameObject.transform.childCount);

#if UNITY_EDITOR
            UnityEditor.Selection.activeGameObject = tile.gameObject;
            UnityEditor.EditorGUIUtility.PingObject(tile.gameObject);
#endif
        }

        [Button("Clear Child Tiles")]
        private void ClearChildren()
        {
            var childCount = transform.childCount;
            for(int x= (childCount - 1); x>=0; x--)
            {
                DestroyImmediate(transform.GetChild(x).gameObject);
            }
        }

    }

}