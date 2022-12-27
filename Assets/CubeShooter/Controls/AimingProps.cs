using UnityEngine;

public struct AimingProps 
{
    public Vector2 Direction;
    public Vector2 ProjectileStartPosition;
    public float ChargedShotCompletedFactor;

    public AimingProps(Vector2 direction, Vector2 projectileStartPosition, float chargedShotCompletedFactor)
    {
        Direction = direction;
        ProjectileStartPosition = projectileStartPosition;
        ChargedShotCompletedFactor = chargedShotCompletedFactor;
    }
}
