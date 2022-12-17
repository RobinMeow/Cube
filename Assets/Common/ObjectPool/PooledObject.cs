using UnityEngine;

public abstract class PooledObject : MonoBehaviour 
{
    public ObjectPool<PooledObject> ObjectPool { get; set; }

    public void ReturnToPool()
    {
        ObjectPool.Return(this);
    }
}
