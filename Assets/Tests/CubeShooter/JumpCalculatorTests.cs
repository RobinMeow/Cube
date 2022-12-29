using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;
using UnityEngine;
using UnityEngine.TestTools;

public sealed class JumpCalculatorTests : BaseTests
{
    const float DELTA_TIME_ZERO = 0.0f;

    [Test]
    public void IsCharging_equals_false_before_Start()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);

        // Act
        // the act, is, to not call Start.

        // Assert
        Assert.That(chargedJump.IsCharging, Is.EqualTo(false));
    }

    [Test]
    public void IsCharging_equals_true_after_Start()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);
        
        // Act
        chargedJump.Start();

        // Assert
        Assert.That(chargedJump.IsCharging, Is.EqualTo(true));
    }

    [Test]
    public void IsCharging_equals_true_after_Charge()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);
        chargedJump.Start();

        // Act
        _ = chargedJump.Charge(DELTA_TIME_ZERO);

        // Assert
        Assert.That(chargedJump.IsCharging, Is.EqualTo(true));
    }

    [Test]
    public void IsCharging_equals_false_after_End()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);
        chargedJump.Start();
        // Act
        _ = chargedJump.End();

        // Assert
        Assert.That(chargedJump.IsCharging, Is.EqualTo(false));
    }

    [Test]
    public void percentage_is_zero_after_zero_Charge()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);
        chargedJump.Start();

        // Act
        float percentage = chargedJump.Charge(DELTA_TIME_ZERO);

        // Assert
        Assert.Zero(percentage);
    }

    [Test]
    public void percentage_is_positive_after_Charge()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        jumpStats.MaxChargeDurationInSeconds = 1.0f;
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);
        chargedJump.Start();

        // Act
        float percentage = chargedJump.Charge(Time.deltaTime);

        // Assert
        Assert.Positive(percentage);
    }

    [UnityTest]
    public IEnumerator percentage_is_positive_after_each_Charge()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);
        chargedJump.Start();

        float percentage = 0.0f;
        do
        {
            // Act
            yield return SKIP_FRAME;
            percentage = chargedJump.Charge(Time.deltaTime);
            
            // Assert
            Assert.Positive(percentage);
        }
        while (percentage != 100.0f);
    }

    [Test]
    public void percentage_cannot_exceed_100()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);
        chargedJump.Start();

        float CHARG_TIME_DOUBLE = jumpStats.MaxChargeDurationInSeconds * 2;

        // Act
        float percentage = chargedJump.Charge(CHARG_TIME_DOUBLE);

        // Assert
        Assert.That(percentage, Is.EqualTo(100.0f).Using(FloatComparer));
    }

    [UnityTest]
    public IEnumerator percentage_is_higher_than_the_percantage_previous_frame_after_each_Charge()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        jumpStats.MaxChargeDurationInSeconds = 0.1f; // make test quicker c:
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);
        chargedJump.Start();

        float previousPercentage = 0.0f;
        do
        {
            // Act
            yield return SKIP_FRAME;
            float currentPercentage = chargedJump.Charge(Time.deltaTime);

            // Assert
            Assert.That(currentPercentage, Is.GreaterThan(previousPercentage));

            previousPercentage = currentPercentage;
        }
        while (previousPercentage != 100.0f);
    }

    [UnityTest]
    public IEnumerator percentage_is_100_when_fully_charged()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        jumpStats.MaxChargeDurationInSeconds = 0.1f; // make test quicker c:
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);
        
        DateTime start = DateTime.Now;
        chargedJump.Start();

        float percentage = 0.0f;
        do
        {
            // Act
            yield return SKIP_FRAME;
            percentage = chargedJump.Charge(Time.deltaTime);
            Debug.Log($"Runtime: {(DateTime.Now - start).TotalSeconds}");
        }
        while ((DateTime.Now - start).TotalSeconds < jumpStats.MaxChargeDurationInSeconds);

        // Assert
        Assert.That(percentage, Is.EqualTo(100.0f).Using(FloatComparer));
    }

    [Test]
    public void jumpStrength_equals_InitialStrength_after_End_with_no_Charge_call()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        const float initialStrength = 10.0f;
        jumpStats.InitialStrength = initialStrength;
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);
        chargedJump.Start();

        // Act
        // dont Charge
        float jumpStregth = chargedJump.End();

        // Assert
        Assert.That(jumpStregth, Is.EqualTo(initialStrength));
    }

    [Test]
    public void jumpStrength_is_positive_after_End_when_charged()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);
        chargedJump.Start();

        // Act
        chargedJump.Charge(Time.deltaTime);

        float jumpStregth = chargedJump.End();

        // Assert
        Assert.Positive(jumpStregth);
    }

    [Test]
    public void jumpStrength_is_greater_than_InitialStrength_after_End_when_fully_charged()
    {
        // Arrange
        const float TEN_SECONDS = 10.0f;

        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        jumpStats.InitialStrength = 50.0f;
        jumpStats.MaxChargedAdditionalStrength = 500.0f;
        jumpStats.MaxChargeDurationInSeconds = TEN_SECONDS;
        
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);
        chargedJump.Start();

        // Act
        chargedJump.Charge(TEN_SECONDS);

        float jumpStregth = chargedJump.End();

        // Assert
        Assert.That(jumpStregth, Is.GreaterThan(jumpStats.InitialStrength));
    }

    [Test]
    public void jumpStrength_equals_maxPossibleJumpStrength_when_fully_charged()
    {
        // Arrange
        const float TEN_SECONDS = 10.0f;

        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        jumpStats.InitialStrength = 50.0f;
        jumpStats.MaxChargedAdditionalStrength = 500.0f;
        jumpStats.MaxChargeDurationInSeconds = TEN_SECONDS;
        float maxPossibleStrength = jumpStats.InitialStrength + jumpStats.MaxChargedAdditionalStrength;

        JumpCalculator chargedJump = new JumpCalculator(jumpStats);
        chargedJump.Start();

        // Act
        chargedJump.Charge(TEN_SECONDS);

        float jumpStregth = chargedJump.End();

        // Assert
        Assert.That(jumpStregth, Is.EqualTo(maxPossibleStrength));
    }

    [Test]
    public void throws_exception_on_calling_Charge_before_Start()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);

        // Act
        void chargeBeforeStart() => chargedJump.Charge(DELTA_TIME_ZERO);

        // Assert
        Assert.Throws<InvalidOperationException>(chargeBeforeStart);
    }

    [Test]
    public void throws_exception_on_calling_End_before_Start()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);

        // Act
        void endBeforeStart() => chargedJump.End();

        // Assert
        Assert.Throws<InvalidOperationException>(endBeforeStart);
    }

    [Test]
    public void throws_exception_on_calling_Start_twice() // (without calling End inbetween)
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        JumpCalculator chargedJump = new JumpCalculator(jumpStats);

        // Act
        chargedJump.Start();

        // Assert
        Assert.Throws<InvalidOperationException>(chargedJump.Start);
    }
}
