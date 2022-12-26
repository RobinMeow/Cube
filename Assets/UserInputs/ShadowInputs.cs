using RibynsModules;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public sealed class ShadowInputs : BaseInputs
{
    DeviceInputs _deviceInputs;

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

    static float invert(float val) => val * -1;
    void SetMove(InputAction.CallbackContext context)
    {
        float x = context.ReadValue<float>();
        x = invert(x);
        MoveDirection = new Vector3(x, 0.0f, 0.0f);
    }

    void SetAim(InputAction.CallbackContext context)
    {
        Vector2 inputDirection = context.ReadValue<Vector2>();
        inputDirection.y = invert(inputDirection.y);
        inputDirection.x = invert(inputDirection.x);
        AimDirection = inputDirection;
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
