using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public static class RandomCordidor
{
    public static Vector2Int endPos;

    //Chon huong di chuyen
    public static Vector2Int SelectDirection(HashSet<Vector2Int> path, Vector2Int startCorridor)
    {
        var listDirection = Direction2D.directionWalk;
        Vector2Int direcPos = new();

        foreach(var item in listDirection)
        {
            var newPos = startCorridor + item;
            if (!path.Contains(newPos))
            {
                direcPos = item;
                break;
            }
        }
        return direcPos;
    }


    //map hoan chinh
    public static (HashSet<Vector2Int>, Vector2Int) MapAddCorridor(int corridorLength, Vector2Int startPos, int walkLength)
    {
        var path = RandomMap.LargeRandom(startPos, walkLength);
        int randomLengthOfCorridor = Random.Range(5, corridorLength);
        var startCorridor = RandomMap.RandomStarttPos(path);
        var directionCor = SelectDirection(path, startCorridor);
        Vector2Int newPos = new();
        Vector2Int currentPos = startCorridor;
        path.Add(currentPos);

        for(int i = 0; i < randomLengthOfCorridor; i++)
        {
            newPos = currentPos + directionCor;
            path.Add(newPos);
            currentPos = newPos;
        }

        endPos = newPos;

        return (path, endPos);
    }
}
