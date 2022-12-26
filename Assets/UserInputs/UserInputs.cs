using RibynsModules;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class UserInputs : MonoBehaviour
{
    DeviceInputs _deviceInputs;

    public bool JumpIsPressed { get; private set; }
    public bool ShootIsPressed { get; private set; }
    public bool JumpWasPressedPreviousFixedUpdate { get; private set; }
    public Vector3 MoveDirection { get; private set; }
    public Vector2 AimDirection { get; private set; }

    void Awake()
    {
        _deviceInputs = new DeviceInputs();
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

    void SetMove(InputAction.CallbackContext context)
    {
        float x = context.ReadValue<float>();
        MoveDirection = new Vector3(x, 0.0f, 0.0f);
    }

    void SetAim(InputAction.CallbackContext context)
    {
        AimDirection = context.ReadValue<Vector2>();
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
