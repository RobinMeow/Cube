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
        bool isAiming = _userInputs.AimDirection != Vector2.zero;
        if (isAiming)
        {
            Vector2 aimDirection = _userInputs.AimDirection;

            Vector2 aimVisualPosition = aimDirection * 1.25f;
            _aimingTransform.localPosition = aimVisualPosition;

            float angle = Mathf.Atan2(_userInputs.AimDirection.y, _userInputs.AimDirection.x) * Mathf.Rad2Deg;
            if (angle < 0)
                angle += 360; // apperently executes when aiming on Y negativ values 

            _aimingTransform.localEulerAngles = new Vector3(0.0f, 0.0f, angle - 90.0f);
        }
    }
}
