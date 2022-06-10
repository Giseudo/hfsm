using UnityEngine;
using HFSM;

namespace Demo1
{
    public class GroundedState : State
    {
        public GroundedState(StateMachine stateMachine) : base(stateMachine)
        {
            State idleState = new IdleState(stateMachine);
            State walkState = new WalkState(stateMachine);
            State runState = new RunState(stateMachine);

            LoadSubState(idleState);
            LoadSubState(walkState);
            LoadSubState(runState);

            AddTransition(idleState, walkState, new Condition[]{ new IsMovingCondition(stateMachine) });
            AddTransition(idleState, runState, new Condition[]{ new IsMovingCondition(stateMachine), new IsRunningCondition(stateMachine) }, Operator.And);

            AddTransition(walkState, idleState, new Condition[]{ new IsMovingCondition(stateMachine, true) });
            AddTransition(walkState, runState, new Condition[]{ new IsMovingCondition(stateMachine), new IsRunningCondition(stateMachine) }, Operator.And);

            AddTransition(runState, idleState, new Condition[]{ new IsMovingCondition(stateMachine, true) });
            AddTransition(runState, walkState, new Condition[]{ new IsMovingCondition(stateMachine), new IsRunningCondition(stateMachine, true) }, Operator.And);
        }

        protected override void OnUpdate()
        {
            Debug.Log("Grounded - Apply gravity");
        }
    }
}