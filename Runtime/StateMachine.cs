using System;
using UnityEngine;

namespace HFSM
{
    [Serializable]
    public class StateMachine : MonoBehaviour
    {
        [SerializeField]
        private StateMachineAsset _asset;

        [SerializeField]
        private StateMachineDebugger _debugger;

        private State _root;

        public State Root => _root;

        public void Awake() {
            _root = _asset?.Init(this);
        }

        public void OnEnable()
        {
            _root.Enter();
        }

        public void OnDisable()
        {
            _root.Exit();
        }

        public void Update() {
            Root.Update();
        }
    }
}