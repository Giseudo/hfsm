using UnityEngine;
using HFSM;

public class WalkState : State
{
    private CharacterController _controller;
    public float speed = 3f;

    protected override void OnStart()
    {
        StateMachine.Context.TryGetComponent<CharacterController>(out _controller);
    }

    protected override void OnUpdate()
    {
        if (_controller == null) return;

        Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        _controller.Move(velocity * speed * Time.deltaTime);
    }
}