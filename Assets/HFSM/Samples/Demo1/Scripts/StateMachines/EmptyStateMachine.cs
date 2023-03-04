using System;
using UnityEngine;
using HFSM;

namespace Demo1
{
    [CreateAssetMenu(menuName = "State Machines/Empty State Machine")]
    public class EmptyStateMachine : StateMachineAsset
    {
        public override State Init(StateMachine context)
        {
            State root = new State();
            State idle = new IdleState();

            root.LoadSubState(idle);

            return root;
        }
    }
}