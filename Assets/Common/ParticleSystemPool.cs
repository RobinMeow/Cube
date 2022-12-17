using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ParticleSystemPool : MonoBehaviour
{
    readonly Queue<ParticleSystem> _particleSystems = new Queue<ParticleSystem>();
    [SerializeField] byte _instantiationCount = 8;
    [SerializeField] ParticleSystem _particleSystem = null;

    void Awake()
    {
        StartCoroutine(InstantiatePooledObjects());
        IEnumerator InstantiatePooledObjects()
        {
            for (byte i = 0; i < _instantiationCount; i++)
            {
                ParticleSystem particleSystem = Instantiate(_particleSystem);
                Return(particleSystem);
            }
            yield break;
        }
    }

    public ParticleSystem Get()
    {
        ParticleSystem particleSystem = _particleSystems.Dequeue();
        particleSystem.transform.parent = null;
        particleSystem.gameObject.SetActive(true);
        return particleSystem;
    }

    public void Return(ParticleSystem particleSystem)
    {
        particleSystem.transform.SetParent(transform);
        particleSystem.gameObject.SetActive(false);
        _particleSystems.Enqueue(particleSystem);
    }
}

//namespace UnityEngine
//{
//    public partial class ParticleSystem
//    {

//    }
//}
