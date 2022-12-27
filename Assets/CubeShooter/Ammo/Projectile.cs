using RibynsModules;
using RibynsModules.GameLogger;
using System;
using UnityEngine;

public sealed class Projectile : MonoBehaviour
{
    static readonly GameLogger _logger = new GameLogger("Projectiles");
#if UNITY_EDITOR
    static Projectile()
    {
        _logger.Subscribe();
    }
#endif

    bool _isFlying = false;
    Vector2 _flyDirection = Aiming.DefaultDirection;
    float _timeAlive = 0.0f;
    ProjectileStats _stats = null;
    AimingProps _aim = default;

    public void Shoot(AimingProps aim, ProjectileStats stats)
    {
        if (aim.Equals(default(AimingProps)))
            throw new ArgumentNullException(nameof(stats));
        if (stats is null) 
            throw new ArgumentNullException(nameof(stats));
        _stats = stats;
        _aim = aim;

        _isFlying = true;

        Vector2 direction = aim.Direction;
        float angle = RotationHelper.GetDegreeAngleFrom(direction);

        transform.SetLocalPositionAndRotation(
            aim.ProjectileStartPosition + direction, 
            Quaternion.Euler(0.0f, 0.0f, angle));
        
        _flyDirection = direction;
    }

    void FixedUpdate()
    {
        if (_isFlying)
        {
            float speedFactor = _stats.MinSpeed + ((_stats.MaxSpeed - _stats.MinSpeed) * _aim.ChargedShotCompletedFactor);
            _logger.Log($"{nameof(speedFactor)}: {speedFactor}");
            transform.Translate(_flyDirection * speedFactor, Space.World);
            _timeAlive += Time.deltaTime;

            if (_timeAlive > _stats.LifeTime)
            {
                ReturnToPool();
            }
        }
    }

    #region ObjectPool 

    public ProjectilePool ProjectilePool { get; set; }

    public void ReturnToPool()
    {
        _isFlying = false;
        _timeAlive = 0.0f;
        ProjectilePool.Return(this);
    }

    #endregion
}
