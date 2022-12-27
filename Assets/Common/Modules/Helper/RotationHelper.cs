using UnityEngine;

public static class RotationHelper 
{
    public static float GetDegreeAngleFrom(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle += 360; // apperently executes when aiming on Y negativ values 
        return angle;
    }
}
