using UnityEngine;
using HFSM;

namespace Demo1
{
    public class GroundedState : State
    {
        State _idleState = new IdleState();
        State _walkState = new WalkState();
        State _runState = new RunState();

        public GroundedState()
        {
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

        protected override void OnUpdate()
        {
            Debug.Log("Grounded - Apply gravity");
        }
    }
}