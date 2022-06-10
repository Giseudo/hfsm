using UnityEngine;
using HFSM;

public class WalkState : State
{
    public WalkState(StateMachine stateMachine) : base(stateMachine) { }

    protected override void OnUpdate()
    {
        Debug.Log("Moving");
    }
}