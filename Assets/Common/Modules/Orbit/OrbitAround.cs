using RibynsModules.Variables;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class OrbitAround : MonoBehaviour
{
    [SerializeField] Transform _target = null;
    [SerializeField] FloatVariable _speed = null;

    void Awake()
    {
        Assert.IsNotNull(_target, $"{nameof(_target)} may not be null.");
        Assert.IsNotNull(_speed, $"{nameof(_speed)} may not be null.");
    }

    void FixedUpdate()
    {
        Orbit();
    }

    void Orbit()
    {
        transform.RotateAround(_target.position, Vector3.forward, _speed);
    }
}
