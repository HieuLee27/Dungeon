using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class RandomMap
{

    //Map nhỏ
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength)
    {
        HashSet<Vector2Int> path = new() { startPos};
        var currentPos = startPos;

        for (int i = 0; i < walkLength; i++)
        {
            var newPos = currentPos + Direction2D.RandomDic();
            path.Add(newPos);
            currentPos = newPos;
        }

        return path;
    }

    //Map lớn
    public static HashSet<Vector2Int> LargeRandom(Vector2Int startPos, int walkLength)
    {
        var path = SimpleRandomWalk(startPos, walkLength);
        HashSet<Vector2Int> path2 = new();
        path2.AddRange(path);
        foreach (var item in path)
        {
            HashSet<Vector2Int> newPath = SimpleRandomWalk(item, walkLength);
            path2.AddRange(newPath);
        }
        return path2;
    }

    //Vi tri bat dau hanh lang
    public static Vector2Int RandomStarttPos(HashSet<Vector2Int> path) //Chọn vị trí bắt đầu hành lang
    {
        int randomItem = UnityEngine.Random.Range(0, path.Count);
        Vector2Int startCorridor = new();
        var randomPos = path.ToList<Vector2Int>()[randomItem];
        var currentPos = randomPos;
        HashSet<Vector2Int> pathToSelect = new();

        foreach (var item in path)
        {
            if (item.x == currentPos.x || item.y == currentPos.y)
            {
                pathToSelect.Add(item);
            }
        }

        foreach (var item in pathToSelect)
        {
            if (item.x > currentPos.x || item.y > currentPos.y)
            {
                startCorridor = item;
            }
        }
        return startCorridor;
    }

    public static (List<Vector2Int>, List<Vector2Int>) MapFightingWithBoss(int width, int height) //Map đánh boss
    {
        List<Vector2Int> listPos = new();
        List<Vector2Int> listBounder = new();
        for (int x = -width; x < width; x++)
        {
            for (int y = -height; y < height; y++)
            {
                listPos.Add(new Vector2Int(x, y));
            }
        }

        for (int x = -width -1; x < width + 1; x++)
        {
            for (int y = -height - 1; y < height + 1; y++)
            {
                if(!listPos.Contains(new Vector2Int(x, y)))
                {
                    listBounder.Add(new Vector2Int(x, y));
                }
            }
        }

        return (listPos, listBounder);
    }
}
public static class Direction2D
{
    public static List<Vector2Int> directionWalk = new()
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    public static List<Vector2Int> directionWalkBouder = new()
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
        new Vector2Int(1, 1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(1, -1)
    };

    public static Vector2Int RandomDic()
    {
        return directionWalk[UnityEngine.Random.Range(0, directionWalk.Count)];
    }
}