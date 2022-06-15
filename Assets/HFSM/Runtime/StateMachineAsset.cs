using UnityEngine;

namespace HFSM
{
    public abstract class StateMachineAsset : ScriptableObject
    {
        public abstract State Init(StateMachine context);
    }
}