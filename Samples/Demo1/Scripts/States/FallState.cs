using UnityEngine;
using HFSM;

public class FallState : State
{
    protected override void OnUpdate()
    {
        Debug.Log("Falling");
    }
}