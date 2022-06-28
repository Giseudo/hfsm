using System;
using System.Collections.Generic;

namespace HFSM
{
    public class StateHistory
    {
        private LinkedList<State> _list = new LinkedList<State>();
        private StateMachine _stateMachine;
        private LinkedListNode<State> _currentState;
        private int _activeIndex = 0;
        private bool _autoSelectLast = true;

        // TODO tests
        // TODO populate
        // TODO clear
        // TODO update current time
        private Dictionary<int, string> _timers = new Dictionary<int, string>();

        public Action<LinkedListNode<State>> stateSelected = delegate { };

        public LinkedList<State> List => _list;
        public int ActiveIndex => _activeIndex;
        public State First => _list.First.Value;
        public State Last => _list.Last.Value;
        public State Next => _currentState.Next?.Value;
        public State Previous => _currentState.Previous?.Value;
        public State Current => _currentState.Value;

        public void Start(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            _stateMachine.stateChanged += OnStateChange;
        }

        public void Destroy()
        {
            _stateMachine.stateChanged -= OnStateChange;
        }

        public void Clear()
        {
            _list = new LinkedList<State>();
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

        public LinkedListNode<State> SelectPrevious()
        {
            if (_currentState.Previous == null) return _currentState;

            _activeIndex--;

            Select(_currentState.Previous);

            return _currentState.Previous;
        }

        public LinkedListNode<State> SelectNext()
        {
            if (_currentState.Next == null) return _currentState;

            _activeIndex++;

            Select(_currentState.Next);

            return _currentState.Next;
        }

        public LinkedListNode<State> SelectLast()
        {
            _activeIndex = _list.Count - 1;

            Select(_list.Last);

            return _list.Last;
        }

        public LinkedListNode<State> SelectFirst()
        {
            _activeIndex = 0;

            Select(_list.First);

            return _list.First;
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

        private void OnStateChange(State from, State to)
        {
            LinkedListNode<State> node = _list.AddLast(to);

            if (_autoSelectLast)
            {
                _activeIndex = _list.Count - 1;
                Select(node);
            }
        }
    }
}