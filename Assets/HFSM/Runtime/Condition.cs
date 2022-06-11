namespace HFSM
{
    public abstract class Condition
    {
        private StateMachine _stateMachine;
        private bool _triggered;

        public StateMachine StateMachine => _stateMachine;
        public bool Triggered => _triggered;

        public Condition()
        { }

        public virtual void Start(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter() => OnEnter();
        public void Update() => OnUpdate();
        public void Exit() {
            OnExit();
            Reset();
        }

        protected virtual void OnEnter() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnExit() { }

        public void Reset() => _triggered = false;
        public void Trigger() => _triggered = true;
    }
}