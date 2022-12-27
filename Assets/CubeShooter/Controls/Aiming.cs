using RibynsModules;
using RibynsModules.GameTimer;
using RibynsModules.Transformers;
using RibynsModules.Variables;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class Aiming : MonoBehaviour
{
    public static Vector2 DefaultDirection => Vector2.up;

    [SerializeField] BaseInputs _inputs = null;
    [SerializeField] Rotator _orbitingCubes = null;
    [SerializeField] FloatReference _maxShotChargeDuration = new FloatReference(1.0f);
    [SerializeField] ProjectilePool _projectilePool = null;

    Vector2 _inputAimDirection = DefaultDirection;
    GameTimer _chargeTimer = null;
    bool _shootWasPressedPreviousFrame = false;
    bool _isShooting = false;

    void Awake()
    {
        Assert.IsNotNull(_inputs, $"{nameof(_inputs)} may not be null.");
        Assert.IsNotNull(_orbitingCubes, $"{nameof(_orbitingCubes)} may not be null.");
        Assert.IsNotNull(_inputs, $"{nameof(_inputs)} may not be null.");
        Assert.IsNotNull(_projectilePool, $"{nameof(_projectilePool)} may not be null.");
        _chargeTimer = new GameTimer(0.0f, _maxShotChargeDuration);

        if (_orbitingCubes.gameObject.activeSelf)
        {
            this.LogWarning($"{nameof(_orbitingCubes)}.gameObject is supposed to be inactive.");
            _orbitingCubes.gameObject.SetActive(false);
        }
    }

    // aim in Update, rather than FixedUpdate, because Movement is in FixedUpdate, which makes the aim visual follow-along the rotation (which is not disred, when not aiming) 
    void Update()
    {
        if (_inputs.AimDirection != Vector2.zero)
            _inputAimDirection = _inputs.AimDirection;

        VisualizeAim(_inputAimDirection);

        Shoot();
    }

    private void Shoot()
    {
        bool shootIsPressed = _inputs.ShootIsPressed;

        bool isInitialShootPress() => shootIsPressed && !_shootWasPressedPreviousFrame;
        bool isHoldingChargeShot() => shootIsPressed && _shootWasPressedPreviousFrame;
        bool hasReleasedChargeShotEarly() => !shootIsPressed && _shootWasPressedPreviousFrame && _isShooting;
        bool maxChargeDurationReached() => _chargeTimer.HasReachedEnd();

        if (isInitialShootPress())
        {
            OnShotStart();
        }
        else if (isHoldingChargeShot())
        {
            _chargeTimer.Tick(Time.deltaTime);

            if (maxChargeDurationReached())
            {
                OnShotEnd();
            }
        }
        else if (hasReleasedChargeShotEarly())
        {
            OnShotEnd();
        }

        _shootWasPressedPreviousFrame = shootIsPressed;
    }

    void OnShotStart()
    {
        _isShooting = true;
        _orbitingCubes.gameObject.SetActive(true);
        _orbitingCubes.StartRotation();
    }

    void OnShotEnd()
    {
        _chargeTimer.ResetTime();
        _orbitingCubes.StopRotation();
        _orbitingCubes.gameObject.SetActive(false);
        _isShooting = false;

        Projectile projectile = _projectilePool.Get();
        projectile.Shoot(transform.position, _inputAimDirection);
    }

    void VisualizeAim(Vector2 aimDirection)
    {
        float angle = RotationHelper.GetDegreeAngleFrom(aimDirection);
        transform.eulerAngles = new Vector3(0.0f, 0.0f, angle - 90.0f); // prolly better to rotate the go by 90° lol
    }
}
