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
        }
    }
}