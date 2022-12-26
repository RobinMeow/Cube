using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseInputs : MonoBehaviour
{
    public bool JumpIsPressed { get; protected set; }
    public bool ShootIsPressed { get; protected set; }
    public bool JumpWasPressedPreviousFixedUpdate { get; protected set; }
    public Vector3 MoveDirection { get; protected set; }
    public Vector2 AimDirection { get; protected set; }

    protected abstract void SetMove(InputAction.CallbackContext context);

    protected abstract void SetAim(InputAction.CallbackContext context);
}
