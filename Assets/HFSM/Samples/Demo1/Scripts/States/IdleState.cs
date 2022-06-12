using UnityEngine;
using HFSM;

public class IdleState : State
{
    protected override void OnUpdate()
    {
        Debug.Log("Idleing");
    }
}