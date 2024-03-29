namespace HFSM
{
    public class Transition
    {
        private State _from;
        private State _to;
        private Condition[] _conditions;
        private Operator _operation;
        private bool _triggered;

        public State From => _from;
        public State To => _to;
        public Condition[] Conditions => _conditions;
        public Operator Operation => _operation;
        public bool Triggered => _triggered;

        public Transition(State from, State to, Condition[] conditions, Operator operation = Operator.Or)
        {
            _from = from;
            _to = to;
            _conditions = conditions;
            _operation = operation;
        }

        private void Trigger() => _triggered = true;
        private void Reset() => _triggered = false;

        public void Start(StateMachine stateMachine)
        {
            for (int i = 0; i < _conditions.Length; i++)
            {
                Condition condition = _conditions[i];

                condition.Start(stateMachine);
            }
        }

        public void Enter()
        {
            for (int i = 0; i < _conditions.Length; i++)
            {
                Condition condition = _conditions[i];

                condition.Enter();
            }
        }

        public void Update()
        {
            for (int i = 0; i < _conditions.Length; i++)
            {
                Condition condition = _conditions[i];

                condition.Update();
            }

            Validate();
       }

        public void Validate()
        {
            int validCount = 0;

            for (int i = 0; i < _conditions.Length; i++)
            {
                Condition condition = _conditions[i];

                if (condition.Triggered)
                    validCount++;
            }

            if (_operation == Operator.And && validCount == _conditions.Length)
                Trigger();

            if (_operation == Operator.Or && validCount > 0)
                Trigger();
        }

        public void Exit()
        {
            for (int i = 0; i < _conditions.Length; i++)
            {
                Condition condition = _conditions[i];

                condition.Exit();
            }

            Reset();
        }
    }
}