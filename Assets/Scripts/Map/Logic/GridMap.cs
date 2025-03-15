using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    public MapData_SO mapData;

    public GridType gridType;

    private Tilemap currentTileMap;

    private void OnEnable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTileMap = GetComponent<Tilemap>();

            if(mapData != null) 
                mapData.tileProperties.Clear();
        }
    }

    private void OnDisable()
    {
        if(!Application.IsPlaying(this))
        {
            currentTileMap = GetComponent<Tilemap>();

            UpdateTileProperties();
#if UNITY_EDITOR
            if(mapData != null)
                EditorUtility.SetDirty(mapData);
#endif
        }
    }

    private void UpdateTileProperties()
    {
        currentTileMap.CompressBounds();

        if(mapData != null)
        {
            //已绘制范围的左下角坐标
            Vector3Int startPos = currentTileMap.cellBounds.min;
            //已绘制范围的右上角坐标
            Vector3Int endPos = currentTileMap.cellBounds.max;

            for(int x = startPos.x; x < endPos.x; x++)
            {
                for(int y = startPos.y; y < endPos.y; y++)
                {
                    TileBase tile = currentTileMap.GetTile(new Vector3Int(x, y, 0));

                    if(tile != null)
                    {
                        TileProperty newTile = new TileProperty
                        {
                            tileCoordinate = new Vector2Int(x, y),
                            gridType = this.gridType,
                            boolTypeValue = true,
                        };

                        mapData.tileProperties.Add(newTile);
                    }
                }
            }

        }
    }
}
