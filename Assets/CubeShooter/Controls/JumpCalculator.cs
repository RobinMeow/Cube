using RibynsModules.GameLogger;
using RibynsModules.GameTimer;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class JumpCalculator 
{
    readonly JumpStats _stats = null;
    readonly GameTimer _holdingTime = null;
    static GameLogger _loggerInstance = new GameLogger("JumpCalculator");
    static GameLogger _logger => _loggerInstance;
    bool _isCharging = false;
    public bool IsCharging { get => _isCharging; }
    
    //public bool JumpStarted { get; private set; }

    public JumpCalculator(JumpStats stats)
    {
        Assert.IsNotNull(stats, $"{nameof(stats)} may not be null");

        _stats = stats;
        _holdingTime = new GameTimer(stats.MaxAccumulationDurationInSeconds);
    }

    /// <summary>
    /// this should be called each frame
    /// </summary>
    public float Calculate(bool jumpIsPressed, bool jumpWasPressedPreviousFixedUpdate, out float percentageComplete)
    {
        float calculatedStrength = 0.0f;
        percentageComplete = 0.0f;

        bool hasReleasedJumpButton() => !jumpIsPressed && jumpWasPressedPreviousFixedUpdate;
        bool hasStartedPressingDownJumpButton() => jumpIsPressed && !jumpWasPressedPreviousFixedUpdate;
        bool isHoldingDownJumpButton() => jumpIsPressed && jumpWasPressedPreviousFixedUpdate;

        if (hasStartedPressingDownJumpButton()) 
        {
            _isCharging = true;
            calculatedStrength = _stats.InitialStrength;
            percentageComplete = 0.0f;
        }
        else if (isHoldingDownJumpButton() && _isCharging)
        {
            _holdingTime.Tick(Time.deltaTime);
            float additionalStrength = CalculateAdditionalStrength(_holdingTime.Current);
            calculatedStrength = _stats.InitialStrength + additionalStrength;
            percentageComplete = _holdingTime.GetCompletedFactor() * 100.0f;
        }
        else if (hasReleasedJumpButton() && _isCharging) 
        {
            float additionalStrength = CalculateAdditionalStrength(_holdingTime.Current);
            calculatedStrength = _stats.InitialStrength + additionalStrength;
            percentageComplete = _holdingTime.GetCompletedFactor() * 100.0f;

            _isCharging = false;
            _holdingTime.ResetTime();
            _logger.Log($"{nameof(hasReleasedJumpButton)} calcedStrength: {calculatedStrength}");
        }
        else
        {
            _isCharging = false;
        }

        return calculatedStrength;
    }

    float CalculateAdditionalStrength(float maximum)
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
