using UnityEngine;

[CreateAssetMenu(menuName = "CubeShooter/Stats")]
public sealed class CubeShooterStats : ScriptableObject
{
    [SerializeField] float _jumpStrength = 600.0f;
    [SerializeField] float _floatStrength = 300.0f;

    public float JumpStrength { get => _jumpStrength; }
    public float FloatStrength { get => _floatStrength; }
}
