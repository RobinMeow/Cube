using RibynsModules;
using RibynsModules.GameTimer;
using RibynsModules.Transformers;
using RibynsModules.Variables;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class Aiming : MonoBehaviour
{
    [SerializeField] UserInputs _userInputs = null;
    [SerializeField] Rotator _orbitingCubes = null;
    [SerializeField] FloatReference _maxShotChargeDuration = new FloatReference(1.0f);
    
    Vector2 _userAimDirection = Vector2.up;
    GameTimer _chargeTimer = null;
    bool _shootWasPressedPreviousFrame = false;
    bool _isShooting = false;

    void Awake()
    {
        Assert.IsNotNull(_userInputs, $"{nameof(_userInputs)} may not be null.");
        Assert.IsNotNull(_orbitingCubes, $"{nameof(_orbitingCubes)} may not be null.");
        Assert.IsNotNull(_userInputs, $"{nameof(_userInputs)} may not be null.");
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
        if (_userInputs.AimDirection != Vector2.zero)
            _userAimDirection = _userInputs.AimDirection;

        VisualizeAim(_userAimDirection);

        Shoot();
    }

    private void Shoot()
    {
        bool shootIsPressed = _userInputs.ShootIsPressed;

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
                this.Log($"{nameof(maxChargeDurationReached)} + {nameof(OnShotEnd)}");
                OnShotEnd();
            }
        }
        else if (hasReleasedChargeShotEarly())
        {
            this.Log($"{nameof(hasReleasedChargeShotEarly)} + {nameof(OnShotEnd)}");
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
    }

    void VisualizeAim(Vector2 aimDirection)
    {
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle += 360; // apperently executes when aiming on Y negativ values 

        transform.eulerAngles = new Vector3(0.0f, 0.0f, angle - 90.0f); // prolly better to rotate the go by 90° lol
    }
}
