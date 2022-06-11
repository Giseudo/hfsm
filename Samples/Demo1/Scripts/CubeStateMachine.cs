using UnityEngine;
using HFSM;

namespace Demo1
{
    public class CubeStateMachine : StateMachine
    {
        public CubeStateMachine(GameObject context) : base (context)
        {
            State groundedState = new GroundedState();
            // State jumpState = new JumpState(this);
            // State fallState = new FallState(this);

            Root.LoadSubState(groundedState);
            // Root.LoadSubState(jumpState);
            // Root.LoadSubState(fallState);

/*
            Root.AddTransition(groundedState, jumpState, Triggers.JUMP);
            Root.AddTransition(groundedState, fallState, Triggers.FALL);

            Root.AddTransition(jumpState, fallState, Triggers.FALL);

            Root.AddTransition(fallState, groundedState, Triggers.LAND);
            */
        }

        public override void Start()
        {
            Root.StartState(this);
            Root.EnterState();
        }

        public override void Update()
        {
            Root.UpdateState();
        }

        public override void Stop()
        {
            Root.ExitState();
        }
    }
}