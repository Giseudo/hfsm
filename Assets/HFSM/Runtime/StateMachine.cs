using System;
using UnityEngine;

namespace HFSM
{
    [Serializable]
    public class StateMachine : MonoBehaviour
    {
        [SerializeField]
        private StateMachineAsset _asset;
        private StateMachineAsset _previousAsset;
        private State _root;

        public Action<State, State> stateChanged = delegate { };
        public Action<StateMachineAsset> assetChanged = delegate { };

        public State Root => _root;
        public StateMachineAsset Asset => _asset;

        public void Awake() => Init();

        public void Start()
        {
            _root?.Enter();

            if (_root != null)
                _root.stateChanged += OnStateChange;
        }

        public void Restart()
        {
            if (_root != null)
                _root.stateChanged -= OnStateChange;

            _root?.Exit();

            Init();
            Start();
        }

        public void Init() => _root = _asset?.Init(this);

        public void OnEnable() => _root?.Enter();

        public void OnDisable() => _root?.Exit();

        public void Update() => _root?.Update();

        public void SetAsset(StateMachineAsset value)
        {
            if (_previousAsset == value) return;

            _previousAsset = _asset;
            _asset = value;

            Restart();

            assetChanged.Invoke(value);
        }

        public void OnStateChange(State from, State to) => stateChanged.Invoke(from, to);

        public void OnValidate() => SetAsset(_asset);
    }
}