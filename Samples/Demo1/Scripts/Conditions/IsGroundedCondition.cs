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
        StateMachine.Context.TryGetComponent<CharacterController>(out _controller);
    }

    public override void OnUpdate()
    {
        bool valid = _controller.isGrounded;

        if (valid && !_negate || !valid && _negate)
            Trigger();
        else
            Reset();
    }
}