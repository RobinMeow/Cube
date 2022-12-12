using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class RotateFeedback : MonoBehaviour
{
    [SerializeField] Transform _target;

    [SerializeField] Vector3Int _multipliers = new Vector3Int(180, 1, 1);
    
    [SerializeField] AnimationCurve _curveX = null;
    [SerializeField] AnimationCurve _curveY = null;
    [SerializeField] AnimationCurve _curveZ = null;

    [SerializeField] float _duration = 0.5f;
    [SerializeField] bool _loop = false;
    [SerializeField] bool _restorePrevious = true;

    public bool IsRotating { get; private set; }

    void Awake()
    {
        Assert.IsNotNull(_target, $"{_target} required for {nameof(RotateFeedback)}");
    }

    public IEnumerator Rotating()
    {
        IsRotating = true;

        do
        {
            DateTime start = DateTime.Now;
            float timePassed = 0.0f;

            Vector3 previousRotation = _target.localEulerAngles;

            Vector3 rotation = _target.localEulerAngles;

            while (timePassed < _duration)
            {
                float factor = Mathf.Clamp01(timePassed / _duration);
                RotateBy(factor);
                _target.localEulerAngles = rotation;
                timePassed = (float)(DateTime.Now - start).TotalSeconds;
                yield return null;
            }

            void RotateBy(float factor)
            {
                rotation.x = _curveX.Evaluate(factor) * _multipliers.x;
                rotation.y = _curveY.Evaluate(factor) * _multipliers.y;
                rotation.z = _curveZ.Evaluate(factor) * _multipliers.z;
            }

            if (_restorePrevious)
                _target.localEulerAngles = previousRotation;
        } 
        while (_loop);

        IsRotating = false;

        yield break;
    }
}
