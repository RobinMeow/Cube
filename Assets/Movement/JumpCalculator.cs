using SeedWork.GameLogs;
using System;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class JumpCalculator 
{
    readonly UserInputs _userInputs = null;
    readonly JumpStats _stats = null;
    readonly Rigidbody _rigidbody;
    readonly GameTimer _holdingTime = null;
    readonly GameLogger _logger = null;
    bool _isJumping = false;

    public bool IsJumping { get => _isJumping; }
    //public bool JumpStarted { get; private set; }

    public JumpCalculator(UserInputs userInputs, JumpStats stats, Rigidbody rigidbody)
    {
        Assert.IsNotNull(userInputs, $"{nameof(JumpCalculator)} requires {nameof(userInputs)} typeof {nameof(UserInputs)}");
        Assert.IsNotNull(stats, $"{nameof(JumpCalculator)} requires {nameof(stats)} typeof {nameof(JumpStats)}");
        Assert.IsNotNull(rigidbody, $"{nameof(JumpCalculator)} requires {nameof(rigidbody)} typeof {nameof(Rigidbody)}");

        _userInputs = userInputs;
        _stats = stats;
        _rigidbody = rigidbody;
        _holdingTime = new GameTimer(_stats.MaxAccumulationDurationInSeconds);

        _logger = new GameLogger("JumpCalculator");
    }

    /// <summary>
    /// this should be called each frame
    /// </summary>
    public float Calculate()
    {
        float calculatedStrength = 0.0f;
        
        //bool StartedPressing() => _userInputs.JumpIsPressed && !_userInputs.JumpWasPressedPreviousFixedUpdate;
        bool IsHolding() => _userInputs.JumpIsPressed && _userInputs.JumpWasPressedPreviousFixedUpdate;
        bool IsReleased() => !_userInputs.JumpIsPressed && _userInputs.JumpWasPressedPreviousFixedUpdate;

        if (IsHolding() && !_isJumping) 
        {
            _holdingTime.Tick(Time.deltaTime);
            _logger.Log($"{nameof(IsHolding)} calcedStrength: {calculatedStrength}");
            
            if (_rigidbody.useGravity)
            {
                _rigidbody.useGravity = false;
                _rigidbody.velocity = Vector3.zero;
            }
        }
        else if (IsReleased() && !_isJumping) 
        {
            float additionalStrength = CalculateAdditionalStrength(_holdingTime.MaxAllowedCurrent);
            calculatedStrength = _stats.InitialStrength + additionalStrength;
            _holdingTime.ResetTime();
            _logger.Log($"{nameof(IsReleased)} calcedStrength: {calculatedStrength}");
            _isJumping = true;
            //JumpStarted = true;
        }
        else
        {
            // leave default: Zero 
            _isJumping = false;
            if (!_rigidbody.useGravity)
                _rigidbody.useGravity = true;
        }

        return calculatedStrength;
    }

    private float CalculateAdditionalStrength(float maximum)
    {
        return (maximum / _stats.AccumulationStepInSeconds) * _stats.AccumulatingStrength;
    }

    public bool ThresholdReached(float chargedJumpFeedbackThresholdFactor, float calculatedJumpStrength)
    {
        float maxPossibleStrength = CalculateAdditionalStrength(_stats.MaxAccumulationDurationInSeconds) + _stats.InitialStrength;
        float threshold = maxPossibleStrength * chargedJumpFeedbackThresholdFactor;
        _logger.Log($"maxPossibleStrength '{maxPossibleStrength}' | calcedStrength '{calculatedJumpStrength}' > 'threshold '{threshold}'");
        return calculatedJumpStrength > threshold; 
    }
}
