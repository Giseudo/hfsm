using UnityEngine;
using HFSM;

public class IsMovingCondition : Condition
{
    private CharacterController _controller;
    private bool _negate;

    public IsMovingCondition(StateMachine stateMachine, bool negate) : base(stateMachine)
    {
        stateMachine.Context.TryGetComponent<CharacterController>(out _controller);

        _negate = negate;
    }

    protected override void OnUpdate()
    {
        float speed = _controller.velocity.magnitude;

        if (speed > 0f || speed == 0f && _negate)
            Trigger();
        else
            Reset();
    }
}