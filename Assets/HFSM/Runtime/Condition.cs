namespace HFSM
{
    public abstract class Condition
    {
        private StateMachine _stateMachine;
        private bool _triggered;

        public StateMachine StateMachine => _stateMachine;
        public bool Triggered => _triggered;

        public Condition(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void EnterState() => OnEnter();
        public void UpdateState() => OnUpdate();
        public void ExitState() => OnExit();

        protected virtual void OnEnter() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnExit() { }

        public void Reset() => _triggered = false;
        public void Trigger() => _triggered = true;
    }
}