using NUnit.Framework;
using UnityEngine;
using NSubstitute;
using HFSM;

public class StateTest
{
    private StateMachine _stateMachine;
    private TestState _state;

    [SetUp]
    public void Setup ()
    {
        GameObject context = new GameObject();

        _stateMachine = context.AddComponent<StateMachine>();
        _state = Substitute.For<TestState>();
    }

    [Test]
    public void ShouldLoadSubStates()
    {
        TestState subState = new TestState();

        _state.LoadSubState(subState);
        _state.Start(_stateMachine);
        _state.Enter();

        Assert.AreEqual(subState, _state.CurrentSubState);
    }

    [Test]
    public void ShouldNotLoadSameSubState()
    {
        TestState subStateA = new TestState();
        TestState subStateB = new TestState();

        _state.LoadSubState(subStateA);

        Assert.Throws<DuplicateSubStateException>(() =>
            _state.LoadSubState(subStateB)
        );
    }

    [Test]
    public void ShouldLoadDifferentSubState()
    {
        TestState subStateA = new TestState();
        OtherState subStateB = new OtherState();

        _state.LoadSubState(subStateA);
        _state.LoadSubState(subStateB);

        Assert.AreEqual(_state.SubStates.Values.Count, 2);
    }

    [Test]
    public void ShouldStartSubStates()
    {
        TestState subState = Substitute.For<TestState>();

        _state.LoadSubState(subState);
        _state.Start(_stateMachine);

        subState.Received().Start(_stateMachine);
    }

    [Test]
    public void ShouldEnterSubState()
    {
        TestState subState = Substitute.For<TestState>();

        _state.LoadSubState(subState);
        _state.Start(_stateMachine);
        _state.Enter();

        subState.Received().Enter();
    }

    [Test]
    public void ShouldUpdateSubState()
    {
        TestState subState = Substitute.For<TestState>();

        _state.LoadSubState(subState);
        _state.Start(_stateMachine);
        _state.Enter();
        _state.Update();

        subState.Received().Update();
    }

    [Test]
    public void ShouldExitSubState()
    {
        TestState subState = Substitute.For<TestState>();

        _state.LoadSubState(subState);
        _state.Start(_stateMachine);
        _state.Enter();
        _state.Update();
        _state.Exit();

        subState.Received().Exit();
    }

    [Test]
    public void ShouldAddTransition()
    {
        TestState subStateA = new TestState();
        OtherState subStateB = new OtherState();

        _state.LoadSubState(subStateA);
        _state.LoadSubState(subStateB);

        _state.AddTransition(subStateA, subStateB, new Condition[]{});

        Assert.IsNotEmpty(_state.Transitions[subStateA.GetType()]);
    }

    [Test]
    public void ShouldNotAddTransitionToMissingState()
    {
        TestState subStateA = new TestState();
        OtherState subStateB = new OtherState();

        _state.LoadSubState(subStateA);

        Assert.Throws<InvalidTransitionException>(() =>
            _state.AddTransition(subStateA, subStateB, new Condition[]{})
        );
    }

    [Test]
    public void ShouldEnterTransitions()
    {
        TestState subStateA = new TestState();
        OtherState subStateB = new OtherState();
        Transition transition = Substitute.For<Transition>(subStateA, subStateB, new Condition[]{}, Operator.Or);

        _state.LoadSubState(subStateA);
        _state.LoadSubState(subStateB);
        _state.AddTransition(transition);

        _state.Start(_stateMachine);
        _state.Enter();

        transition.Received().Enter();
    }

    [Test]
    public void ShouldUpdateTransitions()
    {
        TestState subStateA = new TestState();
        OtherState subStateB = new OtherState();
        Transition transition = Substitute.For<Transition>(subStateA, subStateB, new Condition[]{}, Operator.Or);

        _state.LoadSubState(subStateA);
        _state.LoadSubState(subStateB);
        _state.AddTransition(transition);

        _state.Start(_stateMachine);
        _state.Enter();
        _state.Update();

        transition.Received().Update();
    }

    [Test]
    public void ShouldExitTransitions()
    {
        TestState subStateA = new TestState();
        OtherState subStateB = new OtherState();
        Transition transition = Substitute.For<Transition>(subStateA, subStateB, new Condition[]{}, Operator.Or);

        _state.LoadSubState(subStateA);
        _state.LoadSubState(subStateB);
        _state.AddTransition(transition);

        _state.Start(_stateMachine);
        _state.Enter();
        _state.Update();
        _state.Exit();

        transition.Received().Exit();
    }

    [Test]
    public void ShouldChangeSubState()
    {
        TestState subStateA = new TestState();
        OtherState subStateB = new OtherState();

        _state.LoadSubState(subStateA);
        _state.LoadSubState(subStateB);
        _state.AddTransition(subStateA, subStateB, new Condition[]{ new EnterCondition() });

        _state.Start(_stateMachine);
        _state.Enter();
        _state.Update();

        Assert.AreEqual(subStateB, _state.CurrentSubState);
    }

    [Test]
    public void ShouldFinishSubState()
    {
        bool triggeredFinishEvent = false;

        State a = new TestState();
        State b = new FinishOnEnterState();

        b.finished += () => triggeredFinishEvent = true;

        _state.LoadSubState(a);
        _state.LoadSubState(b);
        _state.AddTransition(a, b, new Condition[]{ new EnterCondition() });

        _state.Start(_stateMachine);
        _state.Enter();
        _state.Update();

        Assert.IsTrue(triggeredFinishEvent);
    }

    [Test]
    public void ShouldForceStateChange()
    {
        bool changedSubstate = false;

        State a = new TestState();
        State b = new FinishOnEnterState();
        State c = new OtherState();

        b.finished += () => _state.ChangeSubState(c);
        c.entered += () => changedSubstate = true;

        _state.LoadSubState(a);
        _state.LoadSubState(b);
        _state.LoadSubState(c);
        _state.AddTransition(a, b, new Condition[]{ new EnterCondition() });

        _state.Start(_stateMachine);
        _state.Enter();
        _state.Update();

        Assert.IsTrue(changedSubstate);
    }

    [Test]
    public void ShouldAcceptMultipleEqualSubstates()
    { }

    [TearDown]
    public void TearDown()
    { }

    public class EmptyCondition : Condition
    { }

    public class EnterCondition : Condition
    {
        public override void OnEnter() => Trigger();
    }

    public class TestState : State
    { }

    public class OtherState : State
    { }

    public class FinishOnEnterState : State
    {
        protected override void OnEnter() => Finish();
    }
}

