using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace HFSM
{
    [Serializable]
    public class State
    {
        public string name = "oi";
        private State _currentSubState;
        private State _defaultSubState;
        [SerializeField]
        private State _parent;
        private StateMachine _stateMachine;

        private Dictionary<Type, State> _subStates = new Dictionary<Type, State>();
        private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();

        public State Parent => _parent;
        public State CurrentSubState => _currentSubState;
        public StateMachine StateMachine => _stateMachine;
        public Dictionary<Type, State> SubStates => _subStates;
        public Dictionary<Type, List<Transition>> Transitions => _transitions;
        public Action<State, State> stateChanged = delegate { };

        public void Start(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            OnStart();

            _subStates.Values
                .ToList()
                .ForEach(state => state.Start(stateMachine));

            StartTransitions();
        }

        public void Enter()
        {
            OnEnter();

            if (_currentSubState == null && _defaultSubState != null)
                _currentSubState = _defaultSubState;

            _currentSubState?.Enter();

            EnterTransitions();
        }

        public void Update()
        {
            OnUpdate();

            _currentSubState?.Update();

            UpdateTransitions();
        }

        public void Exit()
        {
            _currentSubState?.Exit();

            ExitTransitions();

            OnExit();
        }

        public void LoadSubState(State subState)
        {
            if (_subStates.Count == 0)
                _defaultSubState = subState;

            subState._parent = this;
            subState.stateChanged += (from, to) => stateChanged.Invoke(from, to);

            try
            {
                _subStates.Add(subState.GetType(), subState);
            }
            catch (ArgumentException)
            {
                throw new DuplicateSubStateException($"State {GetType()} already contains a substate of type {subState.GetType()}");
            }

        }

        public void AddTransition(State from, State to, Condition[] conditions, Operator operation = Operator.Or)
        {
            Transition transition = new Transition(from, to, conditions, operation);

            AddTransition(transition);
        }

        public void AddTransition(Transition transition)
        {
            State from = transition.From;
            State to = transition.To;

            if (!_subStates.TryGetValue(from.GetType(), out _))
                throw new InvalidTransitionException($"State {GetType()} does not have a substate of type {from.GetType()} to transition from.");

            if (!_subStates.TryGetValue(to.GetType(), out _))
                throw new InvalidTransitionException($"State {GetType()} does not have a substate of type {to.GetType()} to transition from.");

            if (_transitions.TryGetValue(from.GetType(), out List<Transition> transitions))
            {
                transitions.Add(transition);
                return;
            }

            _transitions.Add(from.GetType(), new List<Transition> { transition });
        }

        public void StartTransitions()
        {
            List<State> subStates = _subStates.Values.ToList();

            subStates.ForEach(state => {
                if (!_transitions.TryGetValue(state.GetType(), out List<Transition> transitions))
                    return;

                transitions.ForEach(transition => transition.Start(_stateMachine));
            });
        }

        public void EnterTransitions()
        {
            if (_currentSubState == null)
                return;

            if (!_transitions.TryGetValue(_currentSubState.GetType(), out List<Transition> transitions))
                return;

            for (int i = 0; i < transitions.Count; i++)
            {
                Transition transition = transitions[i];

                transition.Enter();
            }
        }

        public void ExitTransitions()
        {
            if (_currentSubState == null)
                return;

            if (!_transitions.TryGetValue(_currentSubState.GetType(), out List<Transition> transitions))
                return;

            for (int i = 0; i < transitions.Count; i++)
            {
                Transition transition = transitions[i];

                transition.Exit();
            }
        }

        public void UpdateTransitions()
        {
            if (_currentSubState == null)
                return;

            if (!_transitions.TryGetValue(_currentSubState.GetType(), out List<Transition> transitions))
                return;

            for (int i = 0; i < transitions.Count; i++)
            {
                Transition transition = transitions[i];

                transition.Update();

                if (transition.Triggered)
                {
                    ChangeSubState(transition.To);
                    return;
                }
            }
        }

        private void ChangeSubState(State state)
        {
            _currentSubState?.Exit();
            ExitTransitions();

            var newState = _subStates[state.GetType()];

            stateChanged.Invoke(_currentSubState, newState);

            _currentSubState = newState;
            newState.Enter();
            EnterTransitions();
        }

        protected virtual void OnStart() { }
        protected virtual void OnEnter() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnExit() { }
    }
}