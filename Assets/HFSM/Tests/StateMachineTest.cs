using NUnit.Framework;
using UnityEngine;
using NSubstitute;
using HFSM;

public class StateMachineTest
{
    private StateMachine _stateMachine;
    private TestHFSM _asset;

    [SetUp]
    public void Setup ()
    {
        GameObject context = new GameObject();

        _stateMachine = context.AddComponent<StateMachine>();
        _stateMachine.SetAsset(ScriptableObject.CreateInstance<TestHFSM>());
    }

    [Test]
    public void ShouldGetContextVariable()
    {
        TestStateMachineContext context = _stateMachine.GetContext<TestStateMachineContext>();

        Assert.AreEqual(context.variable, "Default");
    }

    [Test]
    public void ShouldSetContextVariable()
    {
        TestStateMachineContext context = _stateMachine.GetContext<TestStateMachineContext>();

        context.variable = "New value";

        Assert.AreEqual(context.variable, "New value");
    }

    [TearDown]
    public void TearDown()
    { }

    public class TestState : State
    { }

    public class TestStateMachineContext : StateMachineContext
    {
        public string variable = "Default";
    }

    public class TestHFSM : StateMachineAsset
    {
        private TestState _testState = new TestState();

        public override State Init(StateMachine origin)
        {
            State root = new State();

            origin.SetContext(new TestStateMachineContext());

            root.LoadSubState(_testState);

            LoadTransitions(root);

            return root;
        }

        private void LoadTransitions(State root)
        { }
    }
}

