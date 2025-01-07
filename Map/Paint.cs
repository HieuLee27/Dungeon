using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Paint : MonoBehaviour
{
    public Tilemap tilemap, tilemapBouder;
    public TileBase tileBase, tileBouder;
    public int walkLength, corridorLength, countOfDungeon;
    public HashSet<Vector2Int> mapPos;

    private void Awake()
    {
        mapPos = new HashSet<Vector2Int>();
    }

    private void Start()
    {
        var path = DrawDungeon.CreateListDungeon(Vector2Int.zero, walkLength, corridorLength, countOfDungeon);
        mapPos = path.Item1;
        PaintFloorTiles(path.Item1);
        PaintBouder(path.Item2);
    }

    private void PaintSingleTile(Vector2Int position, Tilemap tilemap, TileBase tile)
    {
        var positionWalk = tilemap.WorldToCell((Vector3Int)position);

        tilemap.SetTile(positionWalk, tile);
    }

    private void PaintTiles(HashSet<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(position, tilemap, tile);
        }
    }

    public void PaintFloorTiles(HashSet<Vector2Int> floorPos)
    {
        tilemap.ClearAllTiles();
        PaintTiles(floorPos, tilemap, tileBase);
    }

    public void PaintBouder(HashSet<Vector2Int> bouderPos)
    {
        tilemapBouder.ClearAllTiles();
        PaintTiles(bouderPos, tilemapBouder, tileBouder);
    }
}
