using RibynsModules;
using UnityEngine;

public sealed class Projectile : PooledObject
{
    [SerializeField] float _speed = 0.1f;
    [SerializeField] float _lifeTime = 2.5f;

    bool _isFlying = false;
    Vector2 _flyDirection = Aiming.DefaultDirection;
    float _timeAlive = 0.0f;

    public void Shoot(Vector2 spawnPosition, Vector2 aimDirection, Quaternion rotation)
    {
        _isFlying = true;
        
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle += 360; // apperently executes when aiming on Y negativ values 

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
                //ReturnToPool();
            }
        }
    }
}
