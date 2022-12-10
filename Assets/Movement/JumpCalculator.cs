using SeedWork.GameLogs;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class JumpCalculator 
{
    readonly UserInputs _userInputs = null;
    readonly JumpStats _stats = null;
    readonly GameTimer _holdingTime = null;
    bool _isJumping = false;
    readonly GameLogger _logger = null;

    public JumpCalculator(UserInputs userInputs, JumpStats stats)
    {
        Assert.IsNotNull(userInputs, $"{nameof(JumpCalculator)} requires {nameof(userInputs)} typeof {nameof(UserInputs)}");
        Assert.IsNotNull(stats, $"{nameof(JumpCalculator)} requires {nameof(stats)} typeof {nameof(JumpStats)}");

        _userInputs = userInputs;
        _stats = stats;
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
        }
        else if (IsReleased() && !_isJumping) 
        {
            float additionalStrength = (_holdingTime.MaxAllowedCurrent / _stats.AccumulationStepInSeconds) * _stats.AccumulatingStrength;
            calculatedStrength = _stats.InitialStrength + additionalStrength;
            _holdingTime.ResetTime();
            _logger.Log($"{nameof(IsReleased)} calcedStrength: {calculatedStrength}");
            _isJumping = true;
        }
        else
        {
            // leave default: Zero 
            _isJumping = false;
        }

        return calculatedStrength;
    }
}
