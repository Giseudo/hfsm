using System.Collections.Generic;
using UnityEngine;

namespace HFSM
{
    public class StateHistoryTimers
    {
        private float _currentStartTime = 0f;

        private int _activeIndex = 0;

        public int ActiveIndex
        {
            get { return _activeIndex; }
            set {
                _currentStartTime = Time.time;
                _activeIndex = value;
            }
        }

        private Dictionary<int, float> _list = new Dictionary<int, float>();

        public Dictionary<int, float> List => _list;

        public float CurrentTime {
            get {
                if (!_list.TryGetValue(ActiveIndex, out float value)) return 0f;

                return value;
            }
        }

        public void Update()
        {
            float time = Time.time - _currentStartTime;

            if (_list.TryAdd(ActiveIndex, time)) return;

            _list[ActiveIndex] = time;
        }
    }
}