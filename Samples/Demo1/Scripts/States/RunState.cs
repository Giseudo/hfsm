using UnityEngine;
using HFSM;

public class RunState : State
{
    public RunState(StateMachine stateMachine) : base(stateMachine) { }

    protected override void OnUpdate()
    {
        Debug.Log("Running");
    }
}