using UnityEngine;
using HFSM;

namespace Demo1
{
    public class Cube : MonoBehaviour
    {
        [SerializeField]
        private HumanStateMachine _stateMachine;

        void Awake()
        {
            _stateMachine = new HumanStateMachine(gameObject);
        }

        void OnEnable()
        {
            _stateMachine.Start();
        }

        void OnDisable()
        {
            _stateMachine.Stop();
        }

        void Update()
        {
            _stateMachine.Update();

            if (Input.GetKeyDown(KeyCode.J))
            {
                _stateMachine.Root.SendTrigger(Triggers.JUMP);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _stateMachine.Root.SendTrigger(Triggers.RUN);
            }
        }
    }
}