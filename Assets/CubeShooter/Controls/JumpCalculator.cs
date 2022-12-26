using RibynsModules.GameLogger;
using RibynsModules.GameTimer;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class JumpCalculator 
{
    readonly UserInputs _userInputs = null;
    readonly JumpStats _stats = null;
    readonly Rigidbody _rigidbody;
    readonly TextMeshPro _holdJumpPercentage;
    readonly GameTimer _holdingTime = null;
    static GameLogger _loggerInstance = new GameLogger("JumpCalculator");
    static GameLogger _logger => _loggerInstance;
    bool _isJumping = false;

    public bool IsJumping { get => _isJumping; }
    //public bool JumpStarted { get; private set; }

    public JumpCalculator(UserInputs userInputs, JumpStats stats, Rigidbody rigidbody, TextMeshPro holdJumpPercentage)
    {
        Assert.IsNotNull(userInputs, $"{nameof(JumpCalculator)} requires {nameof(userInputs)} typeof {nameof(UserInputs)}");
        Assert.IsNotNull(stats, $"{nameof(JumpCalculator)} requires {nameof(stats)} typeof {nameof(JumpStats)}");
        Assert.IsNotNull(rigidbody, $"{nameof(JumpCalculator)} requires {nameof(rigidbody)} typeof {nameof(Rigidbody)}");

        _userInputs = userInputs;
        _stats = stats;
        _rigidbody = rigidbody;
        _holdJumpPercentage = holdJumpPercentage;
        _holdingTime = new GameTimer(_stats.MaxAccumulationDurationInSeconds);
    }

    public bool IsHolding() => _userInputs.JumpIsPressed && _userInputs.JumpWasPressedPreviousFixedUpdate;

    /// <summary>
    /// this should be called each frame
    /// </summary>
    public float Calculate()
    {
        float calculatedStrength = 0.0f;
        
        bool IsReleased() => !_userInputs.JumpIsPressed && _userInputs.JumpWasPressedPreviousFixedUpdate;

        if (IsHolding() && !_isJumping) 
        {
            _holdingTime.Tick(Time.deltaTime);
            float percentageComplete = _holdingTime.GetCompletedFactor() * 100.0f;
            _holdJumpPercentage.text = $"{percentageComplete:00} %";

            DisableGravity();
        }
        else if (IsReleased() && !_isJumping) 
        {
            float additionalStrength = CalculateAdditionalStrength(_holdingTime.Current);
            calculatedStrength = _stats.InitialStrength + additionalStrength;
            _holdingTime.ResetTime();
            _logger.Log($"{nameof(IsReleased)} calcedStrength: {calculatedStrength}");
            _isJumping = true;
        }
        else
        {
            // leave default: Zero 
            _isJumping = false;
            EnableGravity();

            _holdJumpPercentage.text = String.Empty;
        }

        return calculatedStrength;
    }

    void EnableGravity()
    {
        if (!_rigidbody.useGravity)
            _rigidbody.useGravity = true;
    }

    void DisableGravity()
    {
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
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
