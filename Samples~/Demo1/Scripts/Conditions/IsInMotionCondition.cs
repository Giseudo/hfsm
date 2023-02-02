using UnityEngine;
using HFSM;

public class IsInMotionCondition : Condition
{
    private bool _negate;
    private CharacterController _controller;

    public IsInMotionCondition(bool negate = false)
    {
        _negate = negate;
    }

    public override void OnStart()
    {
        StateMachine.TryGetComponent<CharacterController>(out _controller);
    }

    public override void OnUpdate()
    {
        bool isMoving = Mathf.Abs(_controller.velocity.magnitude) > .1f;

        if (isMoving && !_negate || !isMoving && _negate)
            Trigger();
        else
            Reset();
    }
}