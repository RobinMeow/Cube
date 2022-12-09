using UnityEngine;
using UnityEngine.Assertions;

public sealed class Movement : MonoBehaviour
{
    [SerializeField] UserInputs _userInputs = null;
    [SerializeField] Rigidbody _rigidbody = null;
    [SerializeField] float _jumpStrength = 600.0f;
    [SerializeField] float _floatStrength = 200.0f;

    void Awake()
    {
        Assert.IsNotNull(_userInputs, $"{nameof(Movement)} requires {nameof(UserInputs)}.");
        Assert.IsNotNull(_rigidbody, $"{nameof(Movement)} requires {nameof(Rigidbody)}.");
    }

    void FixedUpdate()
    {
        Vector3 movementForce = Vector3.zero;
        float direction = _userInputs.MoveDirection.x;

        if (direction != 0.0f)
        {
            movementForce.x = _floatStrength * direction;
        }

        if (_userInputs.JumpWasPressed)
        {
            movementForce.y = _jumpStrength;
        }

        _rigidbody.AddForce(movementForce, ForceMode.Force);
    }
}
