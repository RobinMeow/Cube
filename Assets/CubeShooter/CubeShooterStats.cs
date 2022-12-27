using UnityEngine;

[CreateAssetMenu(menuName = FolderName + "/CubeShooterStats")]
public sealed class CubeShooterStats : ScriptableObject
{
    public const string FolderName = "CubeShooter";

    [SerializeField] float _jumpStrength = 600.0f;
    public float JumpStrength { get => _jumpStrength; }

    [SerializeField] float _floatStrength = 300.0f;
    public float FloatStrength { get => _floatStrength; }
}
