using UnityEngine;
using System;
using System.Collections.Generic;

namespace HFSM
{
    public class StateHistory
    {
        private StateMachine _stateMachine;
        private LinkedList<State> _list = new LinkedList<State>();
        private StateHistoryTimers _timers = new StateHistoryTimers();
        private LinkedListNode<State> _currentState;
        private int _activeIndex = 0;
        private bool _autoSelectLast = true;

        public Action<LinkedListNode<State>> stateSelected = delegate { };

        public LinkedList<State> List => _list;
        public int ActiveIndex => _activeIndex;
        public StateHistoryTimers Timers => _timers;
        public LinkedListNode<State> First => _list.First;
        public LinkedListNode<State> Last => _list.Last;
        public LinkedListNode<State> Next => _currentState.Next;
        public LinkedListNode<State> Previous => _currentState.Previous;
        public LinkedListNode<State> Current => _currentState;

        public void Start(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            if (_stateMachine.Root == null) return;

            _stateMachine.Root.stateChanged += OnStateChange;
        }

        public void Destroy()
        {
            if (_stateMachine.Root == null) return;

            _stateMachine.Root.stateChanged -= OnStateChange;
        }

        public void Clear()
        {
            _list = new LinkedList<State>();
            _timers = new StateHistoryTimers();
            _activeIndex = 0;
            _currentState = null;
        }

        public State SelectIndex(int index)
        {
            LinkedListNode<State> node = _list.First;

            for (int i = 0; i < index; i++)
            {
                if (node == null)
                    throw new IndexOutOfRangeException();

                node = node.Next;
            }

            if (node == null)
                throw new HistoryStateNotFoundException($"State at index {index} not found");

            _activeIndex = index;

            Select(node);

            return node.Value;
        }

        public void SelectPrevious()
        {
            if (_currentState?.Previous == null) return;

            _activeIndex--;

            Select(_currentState.Previous);
        }

        public void SelectNext()
        {
            if (_currentState?.Next == null) return;

            _activeIndex++;

            Select(_currentState.Next);
        }

        public void SelectLast()
        {
            _activeIndex = _list.Count - 1;

            Select(_list.Last);
        }

        public void SelectFirst()
        {
            _activeIndex = 0;

            Select(_list.First);
        }

        public void AutoSelectLast(bool value)
        {
            _autoSelectLast = value;

            if (!value) return;

            SelectLast();
        }

        public void Select(LinkedListNode<State> state)
        {
            _currentState = state;

            stateSelected.Invoke(state);
        }

        public void Update() => _timers?.Update();

        private void OnStateChange(State from, State to)
        {
            if (to == null) return;

            _list.AddLast(to);
            _timers.ActiveIndex = _list.Count - 1;

            if (!_autoSelectLast) return;

            SelectLast();
        }
    }
}