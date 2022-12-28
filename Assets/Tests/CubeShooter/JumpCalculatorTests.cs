using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public sealed class JumpCalculatorTests : BaseTests
{
    [Test]
    public void percentageComplete_is_zero_on_first_jump_press()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>();
        JumpCalculator jumpCalculator = new JumpCalculator(jumpStats);
        
        // Act
        jumpCalculator.Calculate(jumpIsPressed: true, jumpWasPressedPreviousFixedUpdate: false, out float percentageComplete);

        // Assert
        Assert.Zero(percentageComplete);
    }

    [Test]
    public void percentageComplete_is_positive_while_charging()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>(); // default stats are sufficient
        JumpCalculator jumpCalculator = new JumpCalculator(jumpStats);

        // Act
        // initial press
        jumpCalculator.Calculate(jumpIsPressed: true, jumpWasPressedPreviousFixedUpdate: false, out float percentageComplete);
        // hold press
        jumpCalculator.Calculate(jumpIsPressed: true, jumpWasPressedPreviousFixedUpdate: true, out  percentageComplete);

        // Assert
        Assert.Positive(percentageComplete);
    }

    [UnityTest]
    public IEnumerator percentageComplete_equals_100f_when_fully_charged()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>(); 
        JumpCalculator jumpCalculator = new JumpCalculator(jumpStats);

        float percentageComplete = 0.0f;
        // Act

        // initial jump press
        jumpCalculator.Calculate(jumpIsPressed: true, jumpWasPressedPreviousFixedUpdate: false, out percentageComplete);
        do
        {
            // holding jump
            yield return SKIP_FRAME;
            jumpCalculator.Calculate(jumpIsPressed: true, jumpWasPressedPreviousFixedUpdate: true, out percentageComplete);
        } 
        while (percentageComplete != 100.0f);

        // release jump
        jumpCalculator.Calculate(jumpIsPressed: false, jumpWasPressedPreviousFixedUpdate: true, out percentageComplete);

        // Assert
        Assert.That(percentageComplete, Is.EqualTo(100.0f)
            .Using(FloatComparer));
    }

    [Test]
    public void percentageComplete_equals_0f_without_jump_press()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>(); // default stats are sufficient
        JumpCalculator jumpCalculator = new JumpCalculator(jumpStats);
        
        // Act
        jumpCalculator.Calculate(jumpIsPressed: false, jumpWasPressedPreviousFixedUpdate: false, out float percentageComplete);

        // Assert
        Assert.Zero(percentageComplete);
    }

    [Test]
    public void percentageComplete_equals_0f_after_jump_release_when_was_not_charging()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>(); // default stats are sufficient
        JumpCalculator jumpCalculator = new JumpCalculator(jumpStats);

        // Act
        jumpCalculator.Calculate(jumpIsPressed: false, jumpWasPressedPreviousFixedUpdate: true, out float percentageComplete);

        // Assert
        Assert.Zero(percentageComplete);
    }

    [Test]
    public void jumpStrength_is_zero_after_jump_release_when_was_not_charging()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>(); // default stats are sufficient
        JumpCalculator jumpCalculator = new JumpCalculator(jumpStats);

        // Act
        float jumpStrength = jumpCalculator.Calculate(jumpIsPressed: false, jumpWasPressedPreviousFixedUpdate: true, out _);

        // Assert
        Assert.Zero(jumpStrength);
    }

    [Test]
    public void jumpStrength_is_zero_befor_jump_press()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>(); // default stats are sufficient
        JumpCalculator jumpCalculator = new JumpCalculator(jumpStats);

        // Act
        float jumpStrength = jumpCalculator.Calculate(jumpIsPressed: false, jumpWasPressedPreviousFixedUpdate: false, out _);

        // Assert
        Assert.Zero(jumpStrength);
    }

    [Test]
    public void jumpStrength_is_positive_after_initial_jump_press()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>(); // default stats are sufficient
        JumpCalculator jumpCalculator = new JumpCalculator(jumpStats);
        jumpStats.InitialStrength = 1.0f;

        // Act
        float initialJumpStrength = jumpCalculator.Calculate(jumpIsPressed: true, jumpWasPressedPreviousFixedUpdate: false, out _);

        // Assert
        Assert.Positive(initialJumpStrength);
    }

    [Test]
    public void jumpStrength_equals_InitialStrength_after_initial_jump_press()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>(); // default stats are sufficient
        JumpCalculator jumpCalculator = new JumpCalculator(jumpStats);
        jumpStats.InitialStrength = 1.0f;

        // Act
        float initialJumpStrength = jumpCalculator.Calculate(jumpIsPressed: true, jumpWasPressedPreviousFixedUpdate: false, out _);

        // Assert
        Assert.That(initialJumpStrength, Is.EqualTo(jumpStats.InitialStrength)
            .Using(FloatComparer));
    }

    [UnityTest]
    public IEnumerator jumpStrength_is_higher_than_initialJumpStrengh_while_charging()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>(); // default stats are sufficient
        JumpCalculator jumpCalculator = new JumpCalculator(jumpStats);

        // Act
        
        // initial press
        float initialJumpStrength = jumpCalculator.Calculate(jumpIsPressed: true, jumpWasPressedPreviousFixedUpdate: false, out _);
        yield return SKIP_FRAME;
        // charge
        float jumpStrength = jumpCalculator.Calculate(jumpIsPressed: true, jumpWasPressedPreviousFixedUpdate: true, out _);

        // Assert
        Assert.That(jumpStrength, Is.GreaterThan(initialJumpStrength));
    }

    [UnityTest]
    public IEnumerator jumpStrength_is_maxPossibleJumpStrength_fully_charged()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>(); // default stats are sufficient
        JumpCalculator jumpCalculator = new JumpCalculator(jumpStats);
        
        float maxPossibleJumpStrength = jumpStats.InitialStrength + jumpStats.MaxChargedAdditionalStrength;

        // Act

        float percentageComplete = 0.0f;
        // Act

        // initial jump press
        jumpCalculator.Calculate(jumpIsPressed: true, jumpWasPressedPreviousFixedUpdate: false, out percentageComplete);
        do
        {
            // holding jump
            yield return SKIP_FRAME;
            jumpCalculator.Calculate(jumpIsPressed: true, jumpWasPressedPreviousFixedUpdate: true, out percentageComplete);
        }
        while (percentageComplete != 100.0f);

        // release jump
        float jumpStrength = jumpCalculator.Calculate(jumpIsPressed: false, jumpWasPressedPreviousFixedUpdate: true, out _);

        // Assert
        Assert.That(maxPossibleJumpStrength, Is.EqualTo(jumpStrength).Using(FloatComparer));
    }

    [UnityTest]
    public IEnumerator isCharging_is_true_while_charging()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>(); // default stats are sufficient
        JumpCalculator jumpCalculator = new JumpCalculator(jumpStats);

        // Act
        float percentageComplete = 0.0f;
        // Act

        // initial jump press
        _ = jumpCalculator.Calculate(jumpIsPressed: true, jumpWasPressedPreviousFixedUpdate: false, out _);
        do
        {
            // holding jump
            yield return SKIP_FRAME;
            _ = jumpCalculator.Calculate(jumpIsPressed: true, jumpWasPressedPreviousFixedUpdate: true, out percentageComplete);
        }
        while (percentageComplete == 0.0f);

        // Assert
        Assert.That(jumpCalculator.IsCharging, Is.EqualTo(true));
    }

    [Test]
    public void isCharging_is_false_while_not_charging()
    {
        // Arrange
        JumpStats jumpStats = ScriptableObject.CreateInstance<JumpStats>(); // default stats are sufficient
        JumpCalculator jumpCalculator = new JumpCalculator(jumpStats);

        // Assert
        Assert.That(jumpCalculator.IsCharging, Is.EqualTo(false));
    }
}
