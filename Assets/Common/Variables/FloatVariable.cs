using UnityEngine;

namespace SeedWork
{
    [CreateAssetMenu(menuName = "Variables/Float")]
    public sealed class FloatVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline] public string DeveloperDescription = "";
#endif

        public float Value;

        public void SetValue(float value)
        {
            Value = value;
        }

        public void SetValue(FloatVariable value)
        {
            Value = value.Value;
        }

        public void ApplyChange(float amount)
        {
            Value += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            Value += amount.Value;
        }

        public static implicit operator float(FloatVariable variable) => variable.Value;
    }
}
