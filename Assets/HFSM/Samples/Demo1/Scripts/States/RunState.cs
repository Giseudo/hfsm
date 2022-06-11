using UnityEngine;
using HFSM;

public class RunState : State
{
    protected override void OnUpdate()
    {
        Debug.Log("Running");
    }
}