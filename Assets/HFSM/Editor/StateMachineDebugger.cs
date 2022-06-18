using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using HFSM;

public class StateMachineDebugger : VisualElement
{
    public new class UxmlFactory : UxmlFactory<StateMachineDebugger, VisualElement.UxmlTraits>
    {
        public override VisualElement Create(IUxmlAttributes bag, CreationContext cc)
        {
            VisualElement root = base.Create(bag, cc);

            return root;
        }
    }

    public void Start(StateMachine stateMachine)
    {
        if (stateMachine.Asset == null) return;

        if (!stateMachine.Initialized)
            stateMachine.Init();

        foreach (State state in stateMachine.Root.SubStates.Values)
        {
            StateCard card = new StateCard(GetTitle(state));

            AddCards(state, card, this);
        }
    }

    public void AddCards(State state, StateCard card, VisualElement parent)
    {
        parent.Add(card);

        foreach (State child in state.SubStates.Values)
        {
            StateCard childCard = new StateCard(GetTitle(child));

            AddCards(child, childCard, card);
        }
    }

    public string GetTitle(State state)
    {
        string title = state.GetType().ToString();

        string[] values = title.Split(".");

        return values[values.Length - 1];
    }
}