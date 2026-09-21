using UnityEngine;

public static class GameUtilities
{
    public static float GetDistance(Vector2 a, Vector2 b)
    {
        return (a - b).magnitude;
    }

    public static Vector2 MoveToward(
        Vector2 currentPosition,
        Vector2 targetPosition,
        float speed,
        float deltaTime
    )
    {
        Vector2 direction =
            (targetPosition - currentPosition).normalized;

        return currentPosition +
            direction * speed * deltaTime;
    }

}