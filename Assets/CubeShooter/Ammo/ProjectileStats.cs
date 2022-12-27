using RibynsModules.Variables;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "NewProjectileStats", menuName = CubeShooterStats.FolderName + "/ProjectileStats")]
public sealed class ProjectileStats : ScriptableObject
{
    [SerializeField] FloatReference _minSpeed = new FloatReference(0.1f);
    public FloatReference MinSpeed { get => _minSpeed; }

    [SerializeField] FloatReference _maxSpeed = new FloatReference(0.6f);
    public FloatReference MaxSpeed { get => _maxSpeed; }

    [SerializeField] FloatReference _lifeTime = new FloatReference(0.8f);
    public FloatReference LifeTime { get => _lifeTime; }

    [SerializeField] FloatReference _reloadTime = new FloatReference(0.8f);
    public FloatReference ReloadTime { get => _reloadTime; }

    void Awake()
    {
        Assert.IsNotNull(_minSpeed, $"{nameof(_minSpeed)} may not be empty.");
        Assert.IsNotNull(_maxSpeed, $"{nameof(_maxSpeed)} may not be empty.");
        Assert.IsNotNull(_lifeTime, $"{nameof(_lifeTime)} may not be empty.");
        Assert.IsNotNull(_reloadTime, $"{nameof(_reloadTime)} may not be empty.");
    }
}
