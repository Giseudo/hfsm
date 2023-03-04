using System;
using UnityEngine;
using HFSM;

namespace Demo1
{
    [CreateAssetMenu(menuName = "State Machines/Cube State Machine")]
    public class CubeStateMachine : StateMachineAsset
    {
        public override State Init(StateMachine context)
        {
            State root = new State();
            State grounded = new GroundedState();
            State jump = new JumpState();
            State fall = new FallState();

            root.LoadSubState(grounded);
            root.LoadSubState(jump);
            root.LoadSubState(fall);

            root.AddTransition(
                grounded,
                jump,
                new Condition[] {
                    new IsJumpingCondition()
                }
            );
            root.AddTransition(
                grounded,
                fall,
                new Condition[] {
                    new IsGroundedCondition(true)
                }
            );

            root.AddTransition(
                jump,
                fall,
                new Condition[] {
                    new WaitCondition(.75f)
                }
            );

            root.AddTransition(
                fall,
                grounded,
                new Condition[] {
                    new IsGroundedCondition()
                }
            );

            root.Start(context);

            return root;
        }
    }
}