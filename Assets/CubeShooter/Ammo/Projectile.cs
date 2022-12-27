using RibynsModules;
using UnityEngine;

public sealed class Projectile : PooledObject
{
    [SerializeField] float _speed = 0.1f;
    [SerializeField] float _lifeTime = 2.5f;

    bool _isFlying = false;
    Vector2 _flyDirection = Aiming.DefaultDirection;
    float _timeAlive = 0.0f;

    public void Shoot(Vector2 from, Vector2 to)
    {
        _isFlying = true;
        transform.position = from;
        transform.LookAt(to, Vector3.up);
    }

    void FixedUpdate()
    {
        if (_isFlying)
        {
            transform.Translate(Vector3.forward * _speed, Space.Self);
            _timeAlive += Time.deltaTime;

            if (_timeAlive > _lifeTime)
            {
                //ReturnToPool();
            }
        }
    }
}
