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
        private StateHistory _history = new StateHistory();

        public Action<State, State> stateChanged = delegate { };
        public Action<StateMachineAsset> assetChanged = delegate { };

        public State Root => _root;
        public StateMachineAsset Asset => _asset;
        public StateHistory History => _history;

        public void Awake() => Init();

        public void Start()
        {
            _history.Start(this);
        }

        public void Restart()
        {
            _root?.Exit();

            Init();
            Start();

            _root?.Enter();
        }

        public void Init()
        {
            StateMachineAsset asset = ScriptableObject.Instantiate<StateMachineAsset>(_asset);

            _root = asset?.Init(this);
        }

        public void OnEnable() => _root?.Enter();

        public void OnDisable() => _root?.Exit();

        public void Update() 
        {
            _root?.Update();
            _history?.Update();
        }

        public void SetAsset(StateMachineAsset value)
        {
            if (_previousAsset == value) return;

            _previousAsset = _asset;
            _asset = value;

            Restart();

            assetChanged.Invoke(value);
        }

        public void OnValidate() => SetAsset(_asset);
    }
}