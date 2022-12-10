using System;
using UnityEngine;

namespace SeedWork
{
    [Serializable]
    public sealed class FloatReference
    {
        [Tooltip("Use a different Value, and not the FloatVariable Value.")]
        public bool UseConstant;

        [Tooltip("The different Value.")] 
        public float ConstantValue;
        [Tooltip("The FloatVariable Value.")] 
        public FloatVariable Variable;

        public FloatReference(float value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public float Value => UseConstant
            ? ConstantValue
            : Variable.Value;

        public static implicit operator float(FloatReference reference) => reference.Value;
    }
}
