using UnityEngine;

namespace HFSM
{
    public abstract class StateMachine
    {
        private RootState _rootState = new RootState();
        private GameObject _context;

        public GameObject Context => _context;
        public GameObject Ctx => _context;
        public RootState Root => _rootState;

        public StateMachine(GameObject context) {
            _context = context;
        }

        public virtual void Start() { }
        public virtual void Stop() { }
        public virtual void Update() { }
    }
}