using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class ShadowInputs : UserInputs
{
    protected override void Update()
    {
        base.Update();
        InvertAim();
    }

    protected override void SetMove(InputAction.CallbackContext context)
    {
        base.SetMove(context);
        Vector2 moveDirection = MoveDirection;
        moveDirection.x = Invert(moveDirection.x);
        MoveDirection = moveDirection;
    }

    protected override void SetAim(InputAction.CallbackContext context)
    {
        base.SetAim(context);
        InvertAim();
    }

    void InvertAim()
    {
        Vector2 aimDirection = AimDirection;
        aimDirection.x = Invert(aimDirection.x);
        aimDirection.y = Invert(aimDirection.y);
        AimDirection = aimDirection;
    }
}
