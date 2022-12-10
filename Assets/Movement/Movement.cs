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
    [SerializeField] MMFeedbacks _groundDropFeedbacks = null;
    //[SerializeField] MMFeedback _jumpFeedback = null;
    //[SerializeField] MMFeedback _chargedJumpFeedback = null;

    // Movement 
    JumpCalculator _jumpCalculator = null;

    void Awake()
    {
        Assert.IsNotNull(_userInputs, $"{nameof(Movement)} requires {nameof(_userInputs)}.");
        Assert.IsNotNull(_rigidbody, $"{nameof(Movement)} requires {nameof(_rigidbody)}.");
        Assert.IsNotNull(_stats, $"{nameof(Movement)} requires {nameof(_stats)}.");
        Assert.IsNotNull(_jumpStats, $"{nameof(Movement)} requires {nameof(_jumpStats)}.");
        Assert.IsNotNull(_boxCollider, $"{nameof(Movement)} requires {nameof(_boxCollider)}.");
        Assert.IsNotNull(_groundDropFeedbacks, $"{nameof(Movement)} requires {nameof(_groundDropFeedbacks)}.");

        _jumpCalculator = new JumpCalculator(_userInputs, _jumpStats);
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

        if (direction != 0.0f)
        {
            movementForce.x = _stats.FloatStrength * direction;
        }

        movementForce.y = _jumpCalculator.Calculate();

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
