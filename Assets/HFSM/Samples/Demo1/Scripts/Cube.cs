using UnityEngine;
using HFSM;

namespace Demo1
{
    public class Cube : MonoBehaviour
    {
        [SerializeField]
        private CubeStateMachine _stateMachine;

        void Awake()
        {
            _stateMachine = new CubeStateMachine(gameObject);
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