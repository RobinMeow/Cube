using UnityEngine;

[CreateAssetMenu(fileName = "NewJumpStats",menuName = "Movement/JumpStats")]
public sealed class JumpStats : ScriptableObject
{
    [SerializeField] float _initialStrength = 400.0f;

    [Tooltip("The amount of force which is applied additionaly to the initial force, per accumulation step.")]
    [SerializeField] float _accumulatingStrength = 75.0f;
    [SerializeField] float _accumulationStepInSeconds = 0.1f;
    [SerializeField] float _maxAccumulationDurationInSeconds = 1.5f;

    public float InitialStrength => _initialStrength;
    public float AccumulatingStrength => _accumulatingStrength;
    public float AccumulationStepInSeconds => _accumulationStepInSeconds;
    public float MaxAccumulationDurationInSeconds => _maxAccumulationDurationInSeconds;
}
