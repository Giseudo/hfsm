using UnityEngine;
using NUnit.Framework;
using HFSM;
using System.Linq;
using NSubstitute;

public class StateHistoryTest
{
    private StateMachine _stateMachine;
    private StateHistory History => _stateMachine.History;

    [SetUp]
    public void Setup ()
    {
        GameObject context = new GameObject();

        _stateMachine = context.AddComponent<StateMachine>();
        _stateMachine.SetAsset(ScriptableObject.CreateInstance<TestStateMachineAsset>());
        _stateMachine.Awake();
        _stateMachine.Start();
    }

    [Test]
    public void ShouldAddStateToList()
    {
        _stateMachine.Update();

        Assert.AreEqual(2, _stateMachine.History.List.Count);

        _stateMachine.Update();

        Assert.AreEqual(3, _stateMachine.History.List.Count);
    }

    [Test]
    public void ShouldSelectPreviousState()
    {
        _stateMachine.Update();
        _stateMachine.Update();

        State previous = _stateMachine.History.Previous;

        _stateMachine.History.SelectPrevious();

        Assert.AreEqual(previous, _stateMachine.History.Current);
    }

    [Test]
    public void ShouldSelectNextState()
    {
        _stateMachine.Update();
        _stateMachine.History.SelectFirst();

        State next = _stateMachine.History.Next;

        _stateMachine.History.SelectNext();

        Assert.AreEqual(next, _stateMachine.History.Current);
    }

    [Test]
    public void ShouldSelectStateAtIndex()
    {
        _stateMachine.Update();
        _stateMachine.Update();
        _stateMachine.Update();

        State state = _stateMachine.History.List.ElementAt(1);

        _stateMachine.History.SelectIndex(1);

        Assert.AreEqual(state, _stateMachine.History.Current);
        Assert.AreEqual(1, _stateMachine.History.ActiveIndex);

        // TODO check errors too
    }

    [Test]
    public void ShouldSelectLastState()
    { }

    [Test]
    public void ShouldSelectFirstState()
    { }

    [Test]
    public void ShouldAutoSelectLast()
    { }

    [Test]
    public void ShouldNotAutoSelectLast()
    { }

    [Test]
    public void ShouldClear()
    { }

    [Test]
    public void ShouldDestroy()
    { }

    [TearDown]
    public void TearDown()
    { }

    public class TestStateMachineAsset : StateMachineAsset
    {
        public override State Init(StateMachine context)
        {
            State root = new RootState();
            TestStateA stateA = new TestStateA();
            TestStateB stateB = new TestStateB();

            root.LoadSubState(stateA);
            root.LoadSubState(stateB);

            root.AddTransition(stateA, stateB, new Condition[] { new TriggerOnEnterCondition() });
            root.AddTransition(stateB, stateA, new Condition[] { new TriggerOnEnterCondition() });

            return root;
        }
    }

    public class TestStateA : State
    { }

    public class TestStateB : State
    { }

    public class TriggerOnEnterCondition : Condition
    {
        public override void OnEnter() => Trigger();
    }
}