using NUnit.Framework;

public class ConditionTest
{
    [OneTimeSetUp]
    public void Setup ()
    { }

    [Test]
    public void ShouldTrigger()
    { }

    [Test]
    public void ShouldNotTrigger()
    { }

    [Test]
    public void ShouldReset()
    { }

    [Test]
    public void ShouldResetOnExit()
    { }

    [TearDown]
    public void TearDown()
    { }
}

