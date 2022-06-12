using UnityEngine;
using HFSM;

namespace Demo1
{
    public class CubeStateMachine : StateMachine
    {
        private State _groundedState = new GroundedState();
        private State _jumpState = new JumpState();
        private State _fallState = new FallState();

        public CubeStateMachine(GameObject context) : base (context)
        { }

        public override void Start()
        {
            Root.LoadSubState(_groundedState);
            Root.LoadSubState(_jumpState);
            Root.LoadSubState(_fallState);

            Root.AddTransition(
                _groundedState,
                _jumpState,
                new Condition[] {
                    new IsJumpingCondition()
                }
            );
            Root.AddTransition(
                _groundedState,
                _fallState,
                new Condition[] {
                    new IsGroundedCondition(true)
                }
            );

            Root.AddTransition(
                _jumpState,
                _fallState,
                new Condition[] {
                    new WaitCondition(1f)
                }
            );

            Root.AddTransition(
                _fallState,
                _groundedState,
                new Condition[] {
                    new IsGroundedCondition()
                }
            );

            Root.Start(this);
            Root.Enter();
        }
    }
}