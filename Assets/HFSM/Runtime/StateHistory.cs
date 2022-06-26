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

        public LinkedList<State> List => _list;
        public int ActiveIndex => _activeIndex;
        public Action selectedStateChanged = delegate { };
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

        public State SelectPrevious()
        {
            State previous = this.Previous;

            if (previous == null) return Current;

            _activeIndex--;

            Select(_currentState.Previous);

            return previous;
        }

        public State SelectNext()
        {
            State next = this.Next;

            if (next == null) return Current;

            _activeIndex++;

            Select(_currentState.Next);

            return next;
        }

        public State SelectLast()
        {
            State last = this.Last;

            _activeIndex = _list.Count - 1;

            Select(_list.Last);

            return last;
        }

        public State SelectFirst()
        {
            State first = this.First;

            _activeIndex = 0;

            Select(_list.First);

            return first;
        }

        public void AutoSelectLast(bool value)
        {
            _autoSelectLast = value;

            if (!value) return;

            // TODO set activeIndex to last and trigger event emission
        }

        private void Select(LinkedListNode<State> state)
        {
            _currentState = state;
        }

        private void OnStateChange(State from, State to)
        {
            LinkedListNode<State> node = _list.AddLast(to);

            if (_autoSelectLast)
                Select(node);
        }
    }
}