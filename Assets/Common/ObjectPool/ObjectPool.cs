using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : PooledObject
{
    [SerializeField] protected GameObject _prefab;

    Queue<T> _inactiveObjects = new Queue<T>();

    public T GetObject()
    {
        if (_inactiveObjects.Count > 0)
        {
            T dequeuedObject = _inactiveObjects.Dequeue();
            dequeuedObject.transform.parent = null;
            dequeuedObject.gameObject.SetActive(true);
            return dequeuedObject;
        }
        else
        {
            GameObject instantiatedGameObject = Instantiate(_prefab);
            Debug.Log("new created");
            T pooledObject = instantiatedGameObject.AddComponent<T>();
            pooledObject.ObjectPool = this as ObjectPool<PooledObject>;
            pooledObject.hideFlags = HideFlags.HideInInspector;

            return pooledObject;
        }
    }

    public void Return(T pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
        pooledObject.transform.parent = transform;

        _inactiveObjects.Enqueue(pooledObject);
    }
}