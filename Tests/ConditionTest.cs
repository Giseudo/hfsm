using UnityEngine;
using NUnit.Framework;
using HFSM;
using NSubstitute;

public class ConditionTest
{
    private Condition _condition;
    private StateMachine _stateMachine;

    [OneTimeSetUp]
    public void Setup ()
    {
        GameObject context = new GameObject();
        _stateMachine = new TestStateMachine();
        _condition = Substitute.For<Condition>();
    }

    [Test]
    public void ShouldCallOnStart()
    {
        _condition.Start(_stateMachine);
        _condition.Received().OnStart();
    }

    [Test]
    public void ShouldCallOnEnter()
    {
        _condition.Enter();
        _condition.Received().OnEnter();
    }

    [Test]
    public void ShouldCallOnUpdate()
    {
        _condition.Update();
        _condition.Received().OnUpdate();
    }

    [Test]
    public void ShouldCallOnExit()
    {
        _condition.Trigger();
        _condition.Exit();
        _condition.Received().OnExit();
    }

    [Test]
    public void ShouldTrigger()
    {
        _condition.Trigger();
        Assert.True(_condition.Triggered);
    }

    [Test]
    public void ShouldResetOnExit()
    {
        _condition.Trigger();
        _condition.Exit();
        Assert.False(_condition.Triggered);
    }

    [TearDown]
    public void TearDown()
    { }

    public class TestCondition : Condition
    { }

    public class TestStateMachine : StateMachine
    { }
}