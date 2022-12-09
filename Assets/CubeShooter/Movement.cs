using UnityEngine;
using UnityEngine.Assertions;

public sealed class Movement : MonoBehaviour
{
    [SerializeField] UserInputs _userInputs = null;
    [SerializeField] Rigidbody _rigidbody = null;
    [SerializeField] CubeShooterStats _stats = null;

    void Awake()
    {
        Assert.IsNotNull(_userInputs, $"{nameof(Movement)} requires {nameof(UserInputs)}.");
        Assert.IsNotNull(_rigidbody, $"{nameof(Movement)} requires {nameof(Rigidbody)}.");
        Assert.IsNotNull(_stats, $"{nameof(Movement)} requires {nameof(CubeShooterStats)}.");
    }

    void FixedUpdate()
    {
        Vector3 movementForce = Vector3.zero;
        float direction = _userInputs.MoveDirection.x;

        if (direction != 0.0f)
        {
            movementForce.x = _stats.FloatStrength * direction;
        }

        if (_userInputs.JumpWasPressed)
        {
            movementForce.y = _stats.JumpStrength;
        }

        _rigidbody.AddForce(movementForce, ForceMode.Force);
    }
}
