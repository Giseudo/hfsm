using UnityEngine;
using HFSM;

public class IsGroundedCondition : Condition
{
    private CharacterController _controller;
    private bool _negate;

    public IsGroundedCondition(bool negate = false)
    {
        _negate = negate;
    }

    public override void OnStart()
    {
        StateMachine.TryGetComponent<CharacterController>(out _controller);
    }

    public override void OnUpdate()
    {
        Vector3 origin = StateMachine.transform.position + Vector3.up * .05f;
        bool valid = Physics.Raycast(origin, Vector3.down, .1f, 1 >> LayerMask.NameToLayer("Default"));

        if (valid && !_negate || !valid && _negate)
            Trigger();
        else
            Reset();
    }
}