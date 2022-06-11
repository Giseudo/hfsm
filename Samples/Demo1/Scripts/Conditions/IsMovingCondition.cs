using UnityEngine;
using HFSM;

public class IsMovingCondition : Condition
{
    private bool _negate;

    public IsMovingCondition(bool negate = false)
    {
        _negate = negate;
    }

    protected override void OnUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(horizontal) > .1f || Mathf.Abs(vertical) > .1f;

        if (isMoving && !_negate || !isMoving && _negate)
            Trigger();
        else
            Reset();
    }
}