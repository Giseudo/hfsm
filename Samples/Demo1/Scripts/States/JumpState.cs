using UnityEngine;
using HFSM;

public class JumpState : State
{
    protected override void OnEnter()
    {
        Debug.Log("Enter JumpState");
    }

    protected override void OnUpdate()
    {
        Debug.Log("Updating JumpState");
    }

    protected override void OnExit()
    {
        Debug.Log("Exit JumpState");
    }
}