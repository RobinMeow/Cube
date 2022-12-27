using System.Collections.Generic;
using UnityEngine;

public sealed class ProjectilePool : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] byte _instantiationCount = 8;

    readonly Queue<Projectile> _inactiveProjectiles = new Queue<Projectile>();

    void Awake()
    {
        for (byte i = 0; i < _instantiationCount; i++)
        {
            GameObject instantiatedGameObject = Instantiate(_prefab);
            Projectile pooledObject = instantiatedGameObject.GetComponent<Projectile>();
            pooledObject.ProjectilePool = this;
            pooledObject.hideFlags = HideFlags.HideInInspector;
            Return(pooledObject);
        }
    }

    public Projectile Get()
    {
        Projectile projectile = _inactiveProjectiles.Dequeue();
        projectile.transform.parent = null;
        projectile.gameObject.SetActive(true);
        return projectile;
    }

    public void Return(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectile.transform.parent = transform;

        _inactiveProjectiles.Enqueue(projectile);
    }
}
