namespace HFSM
{
    // State Machine Context
    // Works as components cache
    // Maybe a way to share variables between states?
    public class StateMachineContext
    {
        public void SetVariable<T>() {}
        public void GetVariable<T>() {}
        public void GetComponent<T>() {}
        public void TryGetComponent<T>() {}
    }
}