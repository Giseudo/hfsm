using UnityEngine;
using HFSM;

public class IdleState : State
{
    public IdleState(StateMachine stateMachine) : base(stateMachine) { }

    protected override void OnUpdate()
    {
        Debug.Log("Idleing");
    }
}