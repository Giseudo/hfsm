using System;

namespace HFSM
{
    [Serializable]
    public class RootState : State
    {
        private StateMachineContext _context;
        public T Context<T>() where T : StateMachineContext => _context as T;

        public void SetContext(StateMachineContext context)
        {
            _context = context;
        }
    }
}