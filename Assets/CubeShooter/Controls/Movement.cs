using Common.Modules;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public sealed class Movement : MonoBehaviour
{
    [SerializeField] BaseInputs _inputs = null;
    [SerializeField] Rigidbody _rigidbody = null;
    [SerializeField] CubeShooterStats _stats = null;
    [SerializeField] JumpStats _jumpStats = null;
    [SerializeField] BoxCollider _boxCollider = null;

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
    [FormerlySerializedAs("_holdJumpPercentage")]
    [SerializeField] TextMeshPro _tmProJumpChargePercentage = null;
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
        Assert.IsNotNull(_inputs, $"{nameof(Movement)} requires {nameof(_inputs)}.");
        Assert.IsNotNull(_rigidbody, $"{nameof(Movement)} requires {nameof(_rigidbody)}.");
        Assert.IsNotNull(_stats, $"{nameof(Movement)} requires {nameof(_stats)}.");
        Assert.IsNotNull(_jumpStats, $"{nameof(Movement)} requires {nameof(_jumpStats)}.");
        Assert.IsNotNull(_boxCollider, $"{nameof(Movement)} requires {nameof(_boxCollider)}.");

        Assert.IsNotNull(_tmProJumpChargePercentage, $"{nameof(Movement)} requires {nameof(_tmProJumpChargePercentage)}.");
        Assert.IsNotNull(_holdJumpPercentageRectTransform, $"{nameof(Movement)} requires {nameof(_holdJumpPercentageRectTransform)}.");
        Assert.IsNotNull(_audioSource, $"{nameof(Movement)} requires {nameof(_audioSource)}.");
        Assert.IsNotNull(_groundDropClip, $"{nameof(Movement)} requires {nameof(_groundDropClip)}.");
        Assert.IsNotNull(_chargedJumpClip, $"{nameof(Movement)} requires {nameof(_chargedJumpClip)}.");
        Assert.IsNotNull(_jumpClip, $"{nameof(Movement)} requires {nameof(_chargedJumpClip)}.");
        Assert.IsNotNull(_groundDropParticles, $"{nameof(Movement)} requires {nameof(_groundDropParticles)}.");
        Assert.IsNotNull(_chargedJumpParticles, $"{nameof(Movement)} requires {nameof(_chargedJumpParticles)}.");
        
        _jumpCalculator = new JumpCalculator(_jumpStats);
        _tmProJumpChargePercentage.color = _meshRenderer.material.color;
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
        float direction = _inputs.MoveDirection.x;

        if (direction != 0.0f && !_jumpCalculator.IsCharging)
        {
            movementForce.x = _stats.FloatStrength * direction;
        }

        float calculatedJumpStrength = _jumpCalculator.Calculate(_inputs.JumpIsPressed, _inputs.JumpWasPressedPreviousFixedUpdate, out float percentageComplete);
        if (percentageComplete != 0.0f)
            _tmProJumpChargePercentage.text = $"{percentageComplete:00} %";
        
        bool isChargingJump = calculatedJumpStrength > 0.0f;
        bool isJumping = _inputs.JumpWasPressedPreviousFixedUpdate && !_inputs.JumpIsPressed;
        if (isJumping)
        {
            movementForce.y = calculatedJumpStrength;
            if (ChargedJumpFeedbackThresholdReached(calculatedJumpStrength))
            {
                PlayChargedJumpFeedbacks();
            }
            else
            {
                _audioSource.PlayOneShot(_jumpClip);
            }
        }
        else if (isChargingJump)
        {
            // disable gravity 
            _rigidbody.useGravity = false;
            _rigidbody.velocity = Vector3.zero;
            SetHoldJumpTextPosition();
        }
        else if (!isChargingJump && !_rigidbody.useGravity)
        {
            // re-enable gravity 
            _rigidbody.useGravity = true;

            // blend out text mesh pro
            _tmProJumpChargePercentage.text = String.Empty;
        }

        _rigidbody.AddForce(movementForce, ForceMode.Force);
    }

    bool ChargedJumpFeedbackThresholdReached(float calculatedJumpStrength)
    {
        float maxPossibleStrength = _jumpStats.MaxChargedAdditionalStrength + _jumpStats.InitialStrength;
        float threshold = maxPossibleStrength * _chargedJumpFeedbackThresholdFactor;
        return calculatedJumpStrength > threshold;
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
