using UnityEngine;
using UnityEngine.Assertions;

public sealed class Aiming : MonoBehaviour
{
    [SerializeField] UserInputs _userInputs = null;
    Vector2 _userAimDirection = Vector2.up;

    void Awake()
    {
        Assert.IsNotNull(_userInputs, $"{nameof(_userInputs)} may not be null.");
    }

    // aim in Update, rather than FixedUpdate, because Movement is in FixedUpdate, which makes the aim visual follow-along the rotation (which is not disred, when not aiming) 
    void Update()
    {
        if (_userInputs.AimDirection != Vector2.zero)
            _userAimDirection = _userInputs.AimDirection;
        ApplyAim(_userAimDirection);
    }

    void ApplyAim(Vector2 aimDirection)
    {
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle += 360; // apperently executes when aiming on Y negativ values 

        transform.eulerAngles = new Vector3(0.0f, 0.0f, angle - 90.0f);
    }
}
