using System;
using UnityEngine;
using NUnit.Framework;
using HFSM;
using System.Linq;
using System.Collections.Generic;
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

        LinkedListNode<State> previous = _stateMachine.History.Previous;

        _stateMachine.History.SelectPrevious();

        Assert.AreEqual(previous, _stateMachine.History.Current);
    }

    [Test]
    public void ShouldSelectNextState()
    {
        _stateMachine.Update();
        _stateMachine.History.SelectFirst();

        LinkedListNode<State> next = _stateMachine.History.Next;

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

        Assert.AreEqual(state, _stateMachine.History.Current.Value);
        Assert.AreEqual(1, _stateMachine.History.ActiveIndex);

        Assert.Throws<IndexOutOfRangeException>(() => _stateMachine.History.SelectIndex(10));
    }

    [Test]
    public void ShouldSelectLastState()
    {
        _stateMachine.Update();
        _stateMachine.History.SelectFirst();

        LinkedListNode<State> last = _stateMachine.History.Last;

        _stateMachine.History.SelectLast();

        Assert.AreEqual(last, _stateMachine.History.Current);
    }

    [Test]
    public void ShouldSelectFirstState()
    {
        _stateMachine.Update();

        LinkedListNode<State> first = _stateMachine.History.First;

        _stateMachine.History.SelectFirst();

        Assert.AreEqual(first, _stateMachine.History.Current);
    }

    [Test]
    public void ShouldAutoSelectLast()
    {
        _stateMachine.Update();
        _stateMachine.Update();
        _stateMachine.Update();
        _stateMachine.Update();

        LinkedListNode<State> last = _stateMachine.History.Last;

        Assert.AreEqual(last, _stateMachine.History.Current);
        Assert.AreEqual(_stateMachine.History.ActiveIndex, 4);
    }

    [Test]
    public void ShouldNotAutoSelectLast()
    {
        _stateMachine.Update();

        LinkedListNode<State> current = _stateMachine.History.Current;

        Assert.AreEqual(_stateMachine.History.ActiveIndex, 1);

        _stateMachine.History.AutoSelectLast(false);

        _stateMachine.Update();
        _stateMachine.Update();
        _stateMachine.Update();

        Assert.AreEqual(current, _stateMachine.History.Current);
        Assert.AreEqual(_stateMachine.History.ActiveIndex, 1);
    }

    [Test]
    public void ShouldClear()
    {
        _stateMachine.Update();
        _stateMachine.Update();
        _stateMachine.Update();

        Assert.Greater(_stateMachine.History.List.Count, 1);
        Assert.Greater(_stateMachine.History.ActiveIndex, 0);

        _stateMachine.History.Clear();

        Assert.Less(_stateMachine.History.List.Count, 1);
        Assert.AreEqual(_stateMachine.History.ActiveIndex, 0);
    }

    [Test]
    public void ShouldDestroy()
    {
        bool triggeredStateChange = false;

        Action<LinkedListNode<State>> OnStateSelect = (node) => triggeredStateChange = true;

        _stateMachine.History.stateSelected += OnStateSelect;

        _stateMachine.History.Destroy();

        _stateMachine.Update();
        _stateMachine.Update();

        Assert.False(triggeredStateChange);
    }

    [TearDown]
    public void TearDown()
    { }

    public class TestStateMachineAsset : StateMachineAsset
    {
        public override State Init(StateMachine context)
        {
            State root = new State();
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