using RibynsModules.Variables;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class Projectile : MonoBehaviour
{
    [SerializeField] FloatVariable _speed = null;
    [SerializeField] FloatVariable _lifeTime = null;

    bool _isFlying = false;
    Vector2 _flyDirection = Aiming.DefaultDirection;
    float _timeAlive = 0.0f;

    void Awake()
    {
        Assert.IsNotNull(_speed, $"{nameof(_speed)} may not be null.");
        Assert.IsNotNull(_lifeTime, $"{nameof(_lifeTime)} may not be null.");
    }

    public void Shoot(Vector2 spawnPosition, Vector2 aimDirection)
    {
        _isFlying = true;

        float angle = RotationHelper.GetDegreeAngleFrom(aimDirection);

        transform.SetLocalPositionAndRotation(
            spawnPosition + aimDirection, 
            Quaternion.Euler(0.0f, 0.0f, angle));
        
        _flyDirection = aimDirection;
    }

    void FixedUpdate()
    {
        if (_isFlying)
        {
            transform.Translate(_flyDirection * _speed, Space.World);
            _timeAlive += Time.deltaTime;

            if (_timeAlive > _lifeTime)
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
