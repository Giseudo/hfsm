using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace HFSM
{
    public abstract class State
    {
        private State _currentSubState;
        private State _defaultSubState;
        private State _parent;
        private StateMachine _stateMachine;

        private Dictionary<Type, State> _subStates = new Dictionary<Type, State>();
        private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();

        public StateMachine StateMachine => _stateMachine;

        public void Start(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            OnStart();

            var subStates = _subStates.Values.ToList();

            subStates.ForEach(state => state.Start(stateMachine));
        }

        public void Enter()
        {
            OnEnter();

            if (_currentSubState == null && _defaultSubState != null)
            {
                _currentSubState = _defaultSubState;
            }

            _currentSubState?.Enter();
        }

        public void Update()
        {
            UpdateTransitions();

            OnUpdate();

            _currentSubState?.Update();
        }

        public void Exit()
        {
            _currentSubState?.Exit();

            OnExit();
        }

        protected virtual void OnStart() { }
        protected virtual void OnEnter() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnExit() { }

        public void LoadSubState(State subState)
        {
            if (_subStates.Count == 0)
            {
                _defaultSubState = subState;
            }

            subState._parent = this;

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
            if (!_subStates.TryGetValue(from.GetType(), out _))
            {
                throw new InvalidTransitionException($"State {GetType()} does not have a substate of type {from.GetType()} to transition from.");
            }

            if (!_subStates.TryGetValue(to.GetType(), out _))
            {
                throw new InvalidTransitionException($"State {GetType()} does not have a substate of type {to.GetType()} to transition from.");
            }

            Transition transition = new Transition(from, to, conditions, operation);

            if (_transitions.TryGetValue(from.GetType(), out List<Transition> transitions))
            {
                transitions.Add(transition);
                return;
            }

            _transitions.Add(from.GetType(), new List<Transition> { transition });
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

            _currentSubState = newState;
            newState.Enter();
            EnterTransitions();
        }
    }
}