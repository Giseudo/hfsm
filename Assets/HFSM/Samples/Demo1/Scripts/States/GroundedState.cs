using UnityEngine;
using HFSM;

namespace Demo1
{
    public class GroundedState : State
    {
        private CharacterController _controller;
        private State _idleState = new IdleState();
        private State _walkState = new WalkState();
        private State _runState = new RunState();

        protected override void OnStart()
        {
            StateMachine.Context.TryGetComponent<CharacterController>(out _controller);

            LoadSubState(_idleState);
            LoadSubState(_walkState);
            LoadSubState(_runState);

            // Idle transitions
            AddTransition(
                _idleState,
                _runState,
                new Condition[] {
                    new IsMovingCondition(),
                    new IsRunningCondition()
                },
                Operator.And
            );
            AddTransition(
                _idleState,
                _walkState,
                new Condition[] {
                    new IsMovingCondition()
                }
            );

            // Walk transitions
            AddTransition(
                _walkState,
                _idleState,
                new Condition[] {
                    new IsMovingCondition(true)
                }
            );
            AddTransition(
                _walkState,
                _runState,
                new Condition[] {
                    new IsMovingCondition(),
                    new IsRunningCondition()
                },
                Operator.And
            );

            // Run transitions
            AddTransition(
                _runState,
                _idleState,
                new Condition[]{
                    new IsMovingCondition(true)
                }
            );
            AddTransition(
                _runState,
                _walkState,
                new Condition[]{
                    new IsMovingCondition(),
                    new IsRunningCondition(true)
                },
                Operator.And
            );
        }
    }
}