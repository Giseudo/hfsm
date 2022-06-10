using UnityEngine;
using HFSM;

public class FallState : State
{
    public FallState(StateMachine stateMachine) : base(stateMachine) { }

    protected override void OnUpdate()
    {
        Debug.Log("Falling");
    }
}