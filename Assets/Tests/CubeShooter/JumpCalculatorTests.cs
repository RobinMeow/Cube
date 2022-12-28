using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public sealed class JumpCalculatorTests : BaseTests
{
    [Test]
    public void JumpCalculatorTestsSimplePasses()
    {
        //JumpCalculator jumpCalculator = new JumpCalculator();
    }

    [UnityTest]
    public IEnumerator JumpCalculatorTestsWithEnumeratorPasses()
    {
        yield return SKIP_FRAME;
    }
}
