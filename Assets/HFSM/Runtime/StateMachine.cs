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
        private StateMachineContext _context;

        public Action<State, State> stateChanged = delegate { };
        public Action<StateMachineAsset> assetChanged = delegate { };
        public T GetContext<T>() where T : StateMachineContext => _context as T;

        public State Root => _root;
        public StateMachineAsset Asset => _asset;
        public StateHistory History => _history;

        public void Start() => Init();

        public void Restart()
        {
            _root?.Exit();

            Init();
        }

        public void Init()
        {
            StateMachineAsset asset = ScriptableObject.Instantiate<StateMachineAsset>(_asset);

            _root = asset?.Init(this);
            _history?.Start(this);

            _root?.Start(this);
            _root?.Enter();
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

        public void SetContext(StateMachineContext context)
        {
            _context = context;
        }

        public void OnValidate()
        {
            if (Application.isPlaying) return;

            SetAsset(_asset);
        }
    }
}