using UnityEngine;
using HFSM;

public class JumpState : State
{
    private CharacterController _controller;
    private Vector3 _velocity;
    public float speed = 3f;

    protected override void OnStart()
    {
        StateMachine.Context.TryGetComponent<CharacterController>(out _controller);
    }

    protected override void OnEnter()
    {
        if (_controller == null) return;

        _velocity = Vector3.zero;
        _velocity.y += Mathf.Sqrt(2f * -3.0f * Physics.gravity.y);
    }

    protected override void OnUpdate()
    {
        if (_controller == null) return;

        Vector3 xzVelocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        _controller.Move(xzVelocity * speed * Time.deltaTime);

        _velocity.y += Physics.gravity.y * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}