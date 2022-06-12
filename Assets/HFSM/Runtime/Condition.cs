namespace HFSM
{
    public abstract class Condition
    {
        private StateMachine _stateMachine;
        private bool _triggered;

        public StateMachine StateMachine => _stateMachine;
        public bool Triggered => _triggered;

        public void Start(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            OnStart();
        }

        public void Enter() => OnEnter();

        public void Update() => OnUpdate();

        public void Exit() {
            Reset();
            OnExit();
        }

        public virtual void OnStart() { }
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }

        public void Reset() => _triggered = false;
        public void Trigger() => _triggered = true;
    }
}