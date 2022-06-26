using NUnit.Framework;
using UnityEngine;
using NSubstitute;
using HFSM;

public class TransitionTest
{
    private StateMachine _stateMachine;
    private TestState _from = new TestState();
    private TestState _to = new TestState();

    [OneTimeSetUp]
    public void Setup ()
    {
        GameObject context = new GameObject();

        _stateMachine = context.AddComponent<StateMachine>();
    }

    [Test]
    public void ShouldStartConditions()
    {
        Condition[] conditions = new Condition[] { Substitute.For<EnterCondition>() };
        Transition transition = Substitute.For<Transition>(_from, _to, conditions, Operator.Or);

        transition.Start(_stateMachine);

        foreach (Condition condition in conditions)
        {
            condition.Received().OnStart();
        }
    }

    [Test]
    public void ShouldEnterConditions()
    {
        Condition[] conditions = new Condition[] { Substitute.For<EnterCondition>() };
        Transition transition = Substitute.For<Transition>(_from, _to, conditions, Operator.Or);

        transition.Start(_stateMachine);
        transition.Enter();

        foreach (Condition condition in conditions)
        {
            condition.Received().OnEnter();
        }
    }

    [Test]
    public void ShouldUpdateConditions()
    {
        Condition[] conditions = new Condition[] { Substitute.For<EnterCondition>() };
        Transition transition = Substitute.For<Transition>(_from, _to, conditions, Operator.Or);

        transition.Start(_stateMachine);
        transition.Enter();
        transition.Update();

        foreach (Condition condition in conditions)
        {
            condition.Received().OnUpdate();
        }
    }

    [Test]
    public void ShouldExitConditions()
    {
        Condition[] conditions = new Condition[] { Substitute.For<EnterCondition>() };
        Transition transition = Substitute.For<Transition>(_from, _to, conditions, Operator.Or);

        transition.Start(_stateMachine);
        transition.Enter();
        transition.Update();
        transition.Exit();

        foreach (Condition condition in conditions)
        {
            condition.Received().OnExit();
        }
    }

    [Test]
    public void ShouldTriggerWhenSomeOrOperatorConditionIsMet()
    {
        Condition[] conditions = new Condition[] { new EnterCondition(), new EmptyCondition() };
        Transition transition = Substitute.For<Transition>(_from, _to, conditions, Operator.Or);

        transition.Start(_stateMachine);
        transition.Enter();
        transition.Update();

        Assert.True(transition.Triggered);
    }

    [Test]
    public void ShouldTriggerWhenEveryAndOperatorConditionsAreMet()
    {
        Condition[] conditions = new Condition[] { new EnterCondition(), new EnterCondition() };
        Transition transition = Substitute.For<Transition>(_from, _to, conditions, Operator.And);

        transition.Start(_stateMachine);
        transition.Enter();
        transition.Update();

        Assert.True(transition.Triggered);
    }

    [Test]
    public void ShouldNotTriggerWhenAnyAndOperatorConditionsAreNotMet()
    {
        Condition[] conditions = new Condition[] { new EmptyCondition(), new EnterCondition() };
        Transition transition = Substitute.For<Transition>(_from, _to, conditions, Operator.And);

        transition.Start(_stateMachine);
        transition.Enter();
        transition.Update();

        Assert.False(transition.Triggered);
    }

    [TearDown]
    public void TearDown()
    { }

    public class EnterCondition : Condition
    {
        public override void OnEnter() => Trigger();
    }

    public class EmptyCondition : Condition
    { }

    public class TestState : State
    { }
}

