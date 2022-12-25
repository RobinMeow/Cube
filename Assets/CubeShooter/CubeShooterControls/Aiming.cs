using UnityEngine;
using UnityEngine.Assertions;

public sealed class Aiming : MonoBehaviour
{
    [SerializeField] Transform _aimingTransform = null;
    [SerializeField] UserInputs _userInputs = null;

    void Awake()
    {
        Assert.IsNotNull(_aimingTransform, $"{nameof(_aimingTransform)} may not be null.");
        Assert.IsNotNull(_userInputs, $"{nameof(_userInputs)} may not be null.");
    }

    void FixedUpdate()
    {
        Aim();
    }

    void Aim()
    {
        Vector2 aimDirection = _userInputs.AimDirection;
        bool isAiming = aimDirection != Vector2.zero;
        if (isAiming)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            if (angle < 0)
                angle += 360; // apperently executes when aiming on Y negativ values 
            _aimingTransform.Rotate(new Vector3(0.0f, 0.0f, angle), Space.World);

            _aimingTransform.localEulerAngles = new Vector3(0.0f, 0.0f, angle - 90.0f);
        }
    }
}
