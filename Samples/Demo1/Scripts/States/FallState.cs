using UnityEngine;
using HFSM;

public class FallState : State
{
    public override string Name => "Fall";

    private CharacterController _controller;
    private Vector3 _gravity;
    public float speed = 2f;

    protected override void OnStart()
    {
        StateMachine.TryGetComponent<CharacterController>(out _controller);
    }

    protected override void OnEnter()
    {
        _gravity = Vector3.zero;
    }

    protected override void OnUpdate()
    {
        Vector3 moveVelocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        _controller.Move(moveVelocity * speed * Time.deltaTime);

        _gravity += Physics.gravity * Time.deltaTime;
        _controller.Move(_gravity * Time.deltaTime);
    }
}