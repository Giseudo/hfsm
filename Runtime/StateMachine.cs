using System;
using UnityEngine;

namespace HFSM
{
    [Serializable]
    public class StateMachine : MonoBehaviour
    {
        [SerializeField]
        private StateMachineAsset _asset;

        private State _root;
        private bool _initialized;

        public State Root => _root;
        public StateMachineAsset Asset => _asset;
        public bool Initialized => _initialized;
        public Action<State, State> stateChanged = delegate { };

        public void Awake() {
            Init();
        }

        public void Init()
        {
            _initialized = true;

            _root = _asset?.Init(this);
            _root.stateChanged += (from, to) => stateChanged.Invoke(from, to);
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
            _root.Update();
        }
    }
}