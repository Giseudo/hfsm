using UnityEngine;
using HFSM;

public class JumpState : State
{
    public override string Name => "Jump";

    private CharacterController _controller;
    private Vector3 _jumpVelocity;
    public float jumpHeight = 2f;
    public float speed = 3f;

    protected override void OnStart()
    {
        StateMachine.TryGetComponent<CharacterController>(out _controller);
    }

    protected override void OnEnter()
    {
        if (_controller == null) return;

        _jumpVelocity = Vector3.up * Mathf.Sqrt(jumpHeight * -3.0f * Physics.gravity.y);
    }

    protected override void OnUpdate()
    {
        if (_controller == null) return;

        Vector3 moveVelocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        _controller.Move(moveVelocity * speed * Time.deltaTime);

        _jumpVelocity.y += Physics.gravity.y * Time.deltaTime;
        _controller.Move(_jumpVelocity * Time.deltaTime);
    }
}