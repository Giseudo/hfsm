using UnityEngine;
using HFSM;

public class FallState : State
{
    private CharacterController _controller;
    private Vector3 _velocity;
    public float speed = 2f;

    protected override void OnStart()
    {
        StateMachine.Context.TryGetComponent<CharacterController>(out _controller);
    }

    protected override void OnEnter()
    {
        _velocity = _controller.velocity;
    }

    protected override void OnUpdate()
    {
        Vector3 xzVelocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        _controller.Move(xzVelocity * speed * Time.deltaTime);

        _velocity.y += Physics.gravity.y * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);
    }
}