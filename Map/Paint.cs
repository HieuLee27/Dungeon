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
    public int widght, height;
    [SerializeField] private GameObject managerGame;

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

    private void Update()
    {
        PainMapFightingWithBoss();
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

    public void PainMapFightingWithBoss()
    {
        var mode = managerGame.GetComponent<ManagerGame>().mode;
        if (mode == ManagerGame.ModeGame.FightingWithBoss)
        {
            tilemap.ClearAllTiles();
            tilemapBouder.ClearAllTiles();
            List<Vector2Int> listPos = RandomMap.MapFightingWithBoss(widght, height).Item1;
            HashSet<Vector2Int> pathGrid = new HashSet<Vector2Int>(listPos);
            HashSet<Vector2Int> pathBounder = new HashSet<Vector2Int>(RandomMap.MapFightingWithBoss(widght, height).Item2);
            PaintTiles(pathGrid, tilemap, tileBase);
            PaintTiles(pathBounder, tilemapBouder, tileBouder);
        }  
    }
}
