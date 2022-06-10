using UnityEngine;
using HFSM;

public class IsRunningCondition : Condition
{
    private bool _negate;

    public IsRunningCondition(StateMachine stateMachine, bool negate = false) : base(stateMachine)
    {
        _negate = negate;
    }

    protected override void OnUpdate()
    {
        bool isRunning = Input.GetButton("Run");

        if (isRunning && !_negate || !isRunning && _negate)
            Trigger();
        else
            Reset();
    }
}