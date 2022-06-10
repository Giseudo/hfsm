using UnityEngine;

namespace HFSM
{
    public abstract class StateMachine
    {
        private State _rootState;
        private GameObject _context;

        public GameObject Context => _context;
        public GameObject Ctx => _context;
        public State Root => _rootState;

        public StateMachine(GameObject context) {
            _context = context;
            _rootState = new State(this);
        }

        public abstract void Start();
        public abstract void Stop();
        public abstract void Update();
    }
}