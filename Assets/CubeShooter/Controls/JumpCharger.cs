using RibynsModules.GameLogger;
using RibynsModules.GameTimer;
using System;
using UnityEngine.Assertions;

public sealed class JumpCharger 
{
    readonly JumpStats _stats = null;
    readonly GameTimer _chargeTime = null;
    static GameLogger _loggerInstance = new GameLogger("JumpCalculator");
    static GameLogger _logger => _loggerInstance;
    bool _isCharging = false;
    public bool IsCharging { get => _isCharging; }

    public JumpCharger(JumpStats stats)
    {
        Assert.IsNotNull(stats, $"{nameof(stats)} may not be null");

        _stats = stats;
        _chargeTime = new GameTimer(stats.MaxChargeDurationInSeconds);
    }

    float CalculateAdditionalStrength()
    {
        return _chargeTime.GetCompletedFactor() * _stats.MaxChargedAdditionalStrength;
    }

    /// <exception cref="InvalidOperationException">do not call twice without calling <see cref="End"/> inbetween</exception>
    public void Start()
    {
        if (_isCharging)
            throw new InvalidOperationException($"Cannot {nameof(Start)}, because it has already started.");

        _isCharging = true;
    }

    /// <summary>
    /// returns the percentage of the amount charged.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public float Charge(float deltaTime)
    {
        if (!_isCharging)
            throw new InvalidOperationException($"Cannot {nameof(Charge)}, because it hasnt started yet.");

        _chargeTime.Tick(deltaTime);
        float percentageCharged = _chargeTime.GetCompletedFactor() * 100.0f;
        return percentageCharged;
    }

    /// <summary>
    /// returns the resulting jump force, based of charge.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public float End()
    {
        if (!_isCharging)
            throw new InvalidOperationException($"Cannot {nameof(End)}, because it hasnt started yet.");

        float additionalStrength = CalculateAdditionalStrength();
        float calculatedStrength = _stats.InitialStrength + additionalStrength;

        _chargeTime.Reset();
        _isCharging = false;

        return calculatedStrength;
    }
}
