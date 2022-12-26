using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class OrbitAround : MonoBehaviour
{
    [SerializeField] Transform _target = null;
    [SerializeField] float _speed = 1.0f;

    void Awake()
    {
        Assert.IsNotNull(_target, $"{nameof(_target)} may not be null.");
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
