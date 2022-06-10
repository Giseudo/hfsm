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

            AddTransition(idleState, walkState, Triggers.WALK);
            AddTransition(idleState, runState, Triggers.RUN);

            AddTransition(walkState, idleState, Triggers.STOP);
            AddTransition(walkState, runState, Triggers.RUN);

            AddTransition(runState, idleState, Triggers.STOP);
            AddTransition(runState, walkState, Triggers.WALK);
        }

        protected override void OnUpdate()
        {
            Debug.Log("Grounded - Apply gravity");
        }
    }
}