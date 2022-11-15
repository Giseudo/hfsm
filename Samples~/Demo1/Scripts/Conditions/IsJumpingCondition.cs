using UnityEngine;
using HFSM;

public class IsJumpingCondition : Condition
{
    public override void OnUpdate()
    {
        if (Input.GetButtonDown("Jump"))
            Trigger();
    }
}