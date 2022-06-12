using UnityEngine;
using HFSM;

public class WalkState : State
{
    protected override void OnUpdate()
    {
        Debug.Log("Moving");
    }
}