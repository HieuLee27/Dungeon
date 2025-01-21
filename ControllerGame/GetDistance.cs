using System;
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

    public static Vector2 GetRandomDirection()
    {
    RandomX:
        float x = (float)Math.Round(UnityEngine.Random.Range(-0.9f, 0.9f), 1);
        if (x == 0.0f) goto RandomX;
    RandomY:
        float y = (float)Math.Round(UnityEngine.Random.Range(-0.9f, 0.9f), 1);
        if (y == 0.0f) goto RandomY;
        Vector2 randomDirection = new Vector2(x, y);
        return randomDirection;
    }
}
