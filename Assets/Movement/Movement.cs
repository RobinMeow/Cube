using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class Movement : MonoBehaviour
{
    [SerializeField] UserInputs _userInputs = null;
    [SerializeField] Rigidbody _rigidbody = null;
    [SerializeField] CubeShooterStats _stats = null;
    [SerializeField] JumpStats _jumpStats = null;
    [SerializeField] BoxCollider _boxCollider = null;

    [Header("Ground cast check")]
    [SerializeField] float _groundCastDistance = 0.0125f;
    RaycastHit _groundHit = new RaycastHit();
    bool _hadGroundHitPreviousFrame = false;
    bool _hasGroundHit = false;

    [Header("Feedbacks")]
    [SerializeField] MMFeedbacks _jumpFeedbacks = null;
    [SerializeField] MMFeedbacks _holdJumpFeedbacks = null;
    [SerializeField] MMFeedbacks _chargedJumpFeedbacks = null;
    [SerializeField] MMFeedbacks _groundDropFeedbacks = null;
    [SerializeField] float _chargedJumpFeedbackThresholdFactor = 0.5f;

    // Movement 
    JumpCalculator _jumpCalculator = null;

    void Awake()
    {
        Assert.IsNotNull(_userInputs, $"{nameof(Movement)} requires {nameof(_userInputs)}.");
        Assert.IsNotNull(_rigidbody, $"{nameof(Movement)} requires {nameof(_rigidbody)}.");
        Assert.IsNotNull(_stats, $"{nameof(Movement)} requires {nameof(_stats)}.");
        Assert.IsNotNull(_jumpStats, $"{nameof(Movement)} requires {nameof(_jumpStats)}.");
        Assert.IsNotNull(_boxCollider, $"{nameof(Movement)} requires {nameof(_boxCollider)}.");

        Assert.IsNotNull(_jumpFeedbacks, $"{nameof(Movement)} requires {nameof(_jumpFeedbacks)}.");
        Assert.IsNotNull(_holdJumpFeedbacks, $"{nameof(Movement)} requires {nameof(_jumpFeedbacks)}.");
        Assert.IsNotNull(_chargedJumpFeedbacks, $"{nameof(Movement)} requires {nameof(_chargedJumpFeedbacks)}.");
        Assert.IsNotNull(_groundDropFeedbacks, $"{nameof(Movement)} requires {nameof(_groundDropFeedbacks)}.");

        _jumpCalculator = new JumpCalculator(_userInputs, _jumpStats, _rigidbody);
    }

    void FixedUpdate()
    {
        _hasGroundHit = CastGroundCheck();
        
        if (_hasGroundHit && !_hadGroundHitPreviousFrame)
        {
            _groundDropFeedbacks.PlayFeedbacks();
        }

        Vector3 movementForce = Vector3.zero;
        float direction = _userInputs.MoveDirection.x;

        if (direction != 0.0f && !_jumpCalculator.IsHolding())
        {
            movementForce.x = _stats.FloatStrength * direction;
        }

        float calculatedJumpStrength = _jumpCalculator.Calculate();
        movementForce.y = calculatedJumpStrength;

        if (_jumpCalculator.IsJumping)
        {
            if (_jumpCalculator.ThresholdReached(_chargedJumpFeedbackThresholdFactor, calculatedJumpStrength))
            {
                _chargedJumpFeedbacks.PlayFeedbacks();
            }
            else
            {
                _jumpFeedbacks.PlayFeedbacks();
            }
        }


        _rigidbody.AddForce(movementForce, ForceMode.Force);

        _hadGroundHitPreviousFrame = _hasGroundHit;
    }

    bool CastGroundCheck()
    {
        return Physics.BoxCast(GetGroundCheckOrigin(), GetGroundCheckExtends(), Vector3.down, out _groundHit, transform.rotation, _groundCastDistance);
    }

    Vector3 GetGroundCheckOrigin()
    {
        return _rigidbody.worldCenterOfMass;
    }

    Vector3 GetGroundCheckExtends()
    {
        return GetGroundCheckSize() / 2;
    }

    Vector3 GetGroundCheckDestination()
    {
        return _rigidbody.worldCenterOfMass - new Vector3(0.0f, _groundCastDistance);
    }

    Vector3 GetGroundCheckSize()
    {
        return _boxCollider.size * 0.95f;
    }

    void OnDrawGizmos()
    {
        var prevMatrix = Gizmos.matrix;
        var prevColor = Gizmos.color;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.green;

        Gizmos.DrawCube(transform.InverseTransformPoint(GetGroundCheckDestination()), GetGroundCheckSize());
        
        Gizmos.matrix = prevMatrix;
        Gizmos.color = prevColor;
    }
}
