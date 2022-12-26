using Common.Modules;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Assertions;

public sealed class Movement : MonoBehaviour
{
    [SerializeField] UserInputs _userInputs = null;
    [SerializeField] Rigidbody _rigidbody = null;
    [SerializeField] CubeShooterStats _stats = null;
    [SerializeField] JumpStats _jumpStats = null;
    [SerializeField] BoxCollider _boxCollider = null;

    [SerializeField] bool _useShadowedMovement = false;
    [SerializeField] MeshRenderer _meshRenderer = null;
    [SerializeField] RectTransform _holdJumpPercentageRectTransform = null;
    [SerializeField] Vector3 _textPositionOffset = new Vector3(1.488f, -0.3f, -0.56f);

    [Header("Ground cast check")]
    [SerializeField] float _groundCastDistance = 0.0125f;
    RaycastHit _groundHit = new RaycastHit();
    bool _hadGroundHitPreviousFrame = false;
    bool _hasGroundHit = false;

    [Header("Feedbacks")]
    [SerializeField] AudioSource _audioSource = null;
    [SerializeField] TextMeshPro _holdJumpPercentage = null;
    [SerializeField] float _chargedJumpFeedbackThresholdFactor = 0.9f;

    [Header("Jump")]
    [SerializeField] AudioClip _jumpClip = null;

    [Header("Charged Jump")]
    [SerializeField] AudioClip _chargedJumpClip = null;
    [SerializeField] ComponentPoolNonAlloc _chargedJumpParticles = null;
    Vector3 _chargedJumpParticlePositionOffset = new Vector3(0.0f, -0.5f, 0.0f);

    [Header("Ground-Drop / Landing")]
    [SerializeField] AudioClip _groundDropClip = null;
    [SerializeField] ComponentPoolNonAlloc _groundDropParticles = null;

    // Movement 
    JumpCalculator _jumpCalculator = null;

    void Awake()
    {
        Assert.IsNotNull(_userInputs, $"{nameof(Movement)} requires {nameof(_userInputs)}.");
        Assert.IsNotNull(_rigidbody, $"{nameof(Movement)} requires {nameof(_rigidbody)}.");
        Assert.IsNotNull(_stats, $"{nameof(Movement)} requires {nameof(_stats)}.");
        Assert.IsNotNull(_jumpStats, $"{nameof(Movement)} requires {nameof(_jumpStats)}.");
        Assert.IsNotNull(_boxCollider, $"{nameof(Movement)} requires {nameof(_boxCollider)}.");

        Assert.IsNotNull(_holdJumpPercentage, $"{nameof(Movement)} requires {nameof(_holdJumpPercentage)}.");
        Assert.IsNotNull(_holdJumpPercentageRectTransform, $"{nameof(Movement)} requires {nameof(_holdJumpPercentageRectTransform)}.");
        Assert.IsNotNull(_audioSource, $"{nameof(Movement)} requires {nameof(_audioSource)}.");
        Assert.IsNotNull(_groundDropClip, $"{nameof(Movement)} requires {nameof(_groundDropClip)}.");
        Assert.IsNotNull(_chargedJumpClip, $"{nameof(Movement)} requires {nameof(_chargedJumpClip)}.");
        Assert.IsNotNull(_jumpClip, $"{nameof(Movement)} requires {nameof(_chargedJumpClip)}.");
        Assert.IsNotNull(_groundDropParticles, $"{nameof(Movement)} requires {nameof(_groundDropParticles)}.");
        Assert.IsNotNull(_chargedJumpParticles, $"{nameof(Movement)} requires {nameof(_chargedJumpParticles)}.");
        
        _jumpCalculator = new JumpCalculator(_userInputs, _jumpStats, _rigidbody, _holdJumpPercentage);
        _holdJumpPercentage.color = _meshRenderer.material.color;
    }

    void FixedUpdate()
    {
        _hasGroundHit = CastGroundCheck();
        
        if (_hasGroundHit)
            Debug.DrawRay(_groundHit.point, _groundHit.normal, Color.green, 0.1f);
        
        if (_hasGroundHit && !_hadGroundHitPreviousFrame)
        {
            PlayGroundDropFeedback();
        }

        Move();

        _hadGroundHitPreviousFrame = _hasGroundHit;
    }

    void Move()
    {
        Vector3 movementForce = Vector3.zero;
        float direction = _userInputs.MoveDirection.x;
        if (_useShadowedMovement)
            direction *= -1.0f;

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
                PlayChargedJumpFeedbacks();
            }
            else
            {
                _audioSource.PlayOneShot(_jumpClip);
            }
        }

        _rigidbody.AddForce(movementForce, ForceMode.Force);

        SetHoldJumpTextPosition();
    }

    void PlayChargedJumpFeedbacks()
    {
        StartCoroutine(play());
        IEnumerator play()
        {
            ParticleSystem particleSystem = _chargedJumpParticles.Get<ParticleSystem>();
            particleSystem.transform.parent = transform;
            particleSystem.transform.SetPositionAndRotation(transform.position + _chargedJumpParticlePositionOffset, Quaternion.identity);
            
            particleSystem.Play();
            _audioSource.PlayOneShot(_chargedJumpClip);

            yield return new WaitForSeconds(particleSystem.main.duration);

            _chargedJumpParticles.Return(particleSystem);
        }
    }

    void PlayGroundDropFeedback()
    {
        StartCoroutine(play(_groundHit));
        IEnumerator play(RaycastHit groundHit)
        {
            ParticleSystem particleSystem = _groundDropParticles.Get<ParticleSystem>();
            particleSystem.transform.SetPositionAndRotation(groundHit.point, groundHit.transform.rotation);

            particleSystem.Play();
            _audioSource.PlayOneShot(_groundDropClip);
            
            yield return new WaitForSeconds(particleSystem.main.duration);
                
            _groundDropParticles.Return(particleSystem);
        }
    }

    void SetHoldJumpTextPosition()
    {
        _holdJumpPercentageRectTransform.SetPositionAndRotation(transform.position + _textPositionOffset, Quaternion.identity);
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