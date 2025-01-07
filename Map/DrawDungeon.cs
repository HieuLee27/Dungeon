using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class DrawDungeon
{
    public static (HashSet<Vector2Int>, HashSet<Vector2Int>) CreateListDungeon(Vector2Int startPos, int walkLength, int corridorLength, int countOfDungeon)
    {
        var path = RandomCordidor.MapAddCorridor(corridorLength, startPos, walkLength);
        HashSet<Vector2Int> pathDungeon = new();
        pathDungeon.AddRange(path.Item1);
        Vector2Int endPos = path.Item2;

        for(int i = 0; i <= countOfDungeon; i++)
        {
            var newPath = RandomCordidor.MapAddCorridor(corridorLength, endPos, walkLength);
            pathDungeon.AddRange(newPath.Item1);
            endPos = newPath.Item2;
        }

        HashSet<Vector2Int> pathBouder = new();

        foreach(var item in pathDungeon)
        {
            for (int i = 0; i < Direction2D.directionWalkBouder.Count; i++) 
            {
                var newItem = item + Direction2D.directionWalkBouder[i];
                bool isContain = pathDungeon.Contains(newItem);
                if (!isContain)
                {
                    pathBouder.Add(newItem);
                }
            }
        }

        return (pathDungeon, pathBouder);
    }
}
