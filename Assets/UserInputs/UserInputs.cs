using RibynsModules;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class UserInputs : BaseInputs
{
    DeviceInputs _deviceInputs;

    [Tooltip("Mouse Aiming requires CubeShooter's position, to get the aim, direction.")]
    [SerializeField] protected bool _useMouseAiming = true;
    [SerializeField] protected Transform _cubeShooter = null;

    void Awake()
    {
        _deviceInputs = new DeviceInputs();
        Assert.IsTrue(!_useMouseAiming || (_useMouseAiming && _cubeShooter != null)); // since when do I do ninja code? nin-nin °^°
    }

    void Start()
    {
        StartCoroutine(LateFixedUpdate());
        IEnumerator LateFixedUpdate()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                JumpWasPressedPreviousFixedUpdate = JumpIsPressed;
            }
        }
    }

    void OnEnable()
    {
        _deviceInputs.Enable();
        _deviceInputs.CubeShooter.Move.performed += SetMove;
        _deviceInputs.CubeShooter.Move.canceled += SetMove;
        _deviceInputs.CubeShooter.Jump.started += SetJump;
        _deviceInputs.CubeShooter.Jump.canceled += SetJump;
        _deviceInputs.CubeShooter.Aim.performed += SetAim;
        _deviceInputs.CubeShooter.Aim.canceled += SetAim;
        _deviceInputs.CubeShooter.Shoot.started += SetShoot;
        _deviceInputs.CubeShooter.Shoot.canceled += SetShoot;
    }

    protected virtual void Update()
    {
        if (_useMouseAiming)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 cubeShooterPosition = (Vector2)Camera.main.WorldToScreenPoint(_cubeShooter.position); 
            Vector2 distance = cubeShooterPosition - mousePosition;
            Vector2 aimDirection = distance.normalized;
            aimDirection.x = Invert(aimDirection.x);
            aimDirection.y = Invert(aimDirection.y);
            AimDirection = aimDirection;
        }
    }
    protected static float Invert(float val) => val * -1;

    protected override void SetMove(InputAction.CallbackContext context)
    {
        float x = context.ReadValue<float>();
        MoveDirection = new Vector3(x, 0.0f, 0.0f);
    }

    protected override void SetAim(InputAction.CallbackContext context)
    {
        if (!_useMouseAiming)
        {
            AimDirection = context.ReadValue<Vector2>();
        }
    }

    void SetJump(InputAction.CallbackContext context)
    {
        JumpIsPressed = context.started;
    }

    void SetShoot(InputAction.CallbackContext context)
    {
        ShootIsPressed = context.started;
    }

    void OnDisable()
    {
        _deviceInputs.Disable();
        _deviceInputs.CubeShooter.Move.performed -= SetMove;
        _deviceInputs.CubeShooter.Move.canceled -= SetMove;
        _deviceInputs.CubeShooter.Jump.started -= SetJump;
        _deviceInputs.CubeShooter.Jump.canceled -= SetJump;
        _deviceInputs.CubeShooter.Aim.performed -= SetAim;
        _deviceInputs.CubeShooter.Aim.canceled -= SetAim;
        _deviceInputs.CubeShooter.Shoot.started -= SetShoot;
        _deviceInputs.CubeShooter.Shoot.canceled -= SetShoot;
    }
}
