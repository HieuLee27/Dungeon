using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GetDistance
{
    public static Vector2 GetDirection(Vector2 startPos, Vector2 endPos) //Hướng di chuyển cho đới tượng
    {
        Vector2 direction = (endPos - startPos).normalized;

        return direction;
    }

    public static float GetDistanceBetween(Vector2 playerPos, Vector2 enemyPos) //Khoảng cách giữa hai đối tượng (Player và Enemy)
    {
        float distance = Vector2.Distance(playerPos, enemyPos);

        return distance;
    }

    public static Vector2 GetDirectionOfJoystick(Joystick joystick) //Lấy hướng của Joystick hiện tại
    {
        Vector2 directionJoystick = (joystick.Direction).normalized;

        return directionJoystick;
    }

    public static Vector2 SelectNearestEnemy(Dictionary<Vector2, float> enemyInArea, Vector2 playerPos) //Chọn kẻ thù gần nhất với player
    {
        Vector2 direction = new Vector2();
        float minDistance = 3f;
        foreach (var enemy in enemyInArea)
        {
            if (enemy.Value < minDistance)
            {
                minDistance = enemy.Value;
                direction = GetDirection(enemy.Key, playerPos);
            }
        }

        return direction;
    }
}
