using System;
using System.Collections.Generic;

namespace HFSM
{
    public class State
    {
        private State _currentSubState;
        private State _defaultSubState;
        private State _parent;
        private StateMachine _stateMachine;

        private Dictionary<Type, State> _subStates = new Dictionary<Type, State>();
        private Dictionary<string, State> _transitions = new Dictionary<string, State>();

        public StateMachine StateMachine => _stateMachine;

        public State(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void EnterState()
        {
            OnEnter();

            if (_currentSubState == null && _defaultSubState != null)
            {
                _currentSubState = _defaultSubState;
            }

            _currentSubState?.EnterState();
        }

        public void UpdateState()
        {
            OnUpdate();

            _currentSubState?.UpdateState();
        }

        public void ExitState()
        {
            _currentSubState?.ExitState();

            OnExit();
        }

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

        // State from, State to, Array<Condition>, Operator operator (and | or)
        public void AddTransition(State from, State to, string trigger)
        {
            if (!_subStates.TryGetValue(from.GetType(), out _))
            {
                throw new InvalidTransitionException($"State {GetType()} does not have a substate of type {from.GetType()} to transition from.");
            }

            if (!_subStates.TryGetValue(to.GetType(), out _))
            {
                throw new InvalidTransitionException($"State {GetType()} does not have a substate of type {to.GetType()} to transition from.");
            }

            try
            {
                from._transitions.Add(trigger, to);
            }
            catch (ArgumentException)
            {
                throw new DuplicateTransitionException($"State {from} already has a transition defined for trigger {trigger}");
            }

        }

        public void SendTrigger(string trigger)
        {
            var root = this;

            while (root?._parent != null)
            {
                root = root._parent;
            }

            while (root != null)
            {
                if (root._transitions.TryGetValue(trigger, out State toState))
                {
                    root._parent?.ChangeSubState(toState);

                    return;
                }

                root = root._currentSubState;
            }

            throw new NeglectedTriggerException($"Trigger {trigger} was not consumed by any transition!");
        }

        private void ChangeSubState(State state)
        {
            _currentSubState?.ExitState();

            var newState = _subStates[state.GetType()];

            _currentSubState = newState;
            newState.EnterState();
        }
    }
}