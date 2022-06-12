using UnityEngine;
using HFSM;

public class WaitCondition : Condition
{
    private float _enterTime;
    private float _seconds;

    public WaitCondition (float seconds)
    {
        _seconds = seconds;
    }

    public override void OnEnter()
    {
        _enterTime = Time.time;
    }

    public override void OnUpdate()
    {
        if (_enterTime + _seconds < Time.time)
            Trigger();
    }
}