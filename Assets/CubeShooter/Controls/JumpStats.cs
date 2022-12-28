using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewJumpStats",menuName = "Movement/JumpStats")]
public sealed class JumpStats : ScriptableObject
{
    [field: SerializeField]
    public float InitialStrength { get; set; } = 8000.0f;

    [field: SerializeField]
    [Tooltip("The amount of force which is applied additionaly to the initial force, per accumulation step.")]
    public float AccumulatingStrength { get; set; } = 12500.0f;

    [field: SerializeField]
    public float AccumulationStepInSeconds { get; set; } = 0.2f;

    [field: SerializeField]
    public float MaxAccumulationDurationInSeconds { get; set; } = 0.5f;
}
