using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class UserInputs : MonoBehaviour
{
    DeviceInputs _deviceInputs;

    public bool JumpWasPressed { get; private set; }
    public Vector3 MoveDirection { get; private set; }

    void Awake()
    {
        _deviceInputs = new DeviceInputs();
    }

    void OnEnable()
    {
        _deviceInputs.Enable();
        _deviceInputs.CubeShooter.Move.performed += OnMove;
        _deviceInputs.CubeShooter.Move.canceled += OnMove;
        _deviceInputs.CubeShooter.Jump.started += OnJump;
        _deviceInputs.CubeShooter.Jump.canceled += OnJump;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        float x = context.ReadValue<float>();
        MoveDirection = new Vector3(x, 0.0f, 0.0f);
    }

    void OnJump(InputAction.CallbackContext context)
    {
        JumpWasPressed = context.started;
    }

    void OnDisable()
    {
        _deviceInputs.Disable();
        _deviceInputs.CubeShooter.Move.performed -= OnMove;
        _deviceInputs.CubeShooter.Move.canceled -= OnMove;
        _deviceInputs.CubeShooter.Jump.started -= OnJump;
        _deviceInputs.CubeShooter.Jump.canceled -= OnJump;
    }
}
