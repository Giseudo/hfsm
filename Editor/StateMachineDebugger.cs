using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using HFSM;

public class StateMachineDebugger : VisualElement
{
    private StateMachine _stateMachine;

    public new class UxmlFactory : UxmlFactory<StateMachineDebugger, VisualElement.UxmlTraits>
    { }

    public void Start(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;

        if (stateMachine.Asset == null) return;

        // TODO add support to empty asset & asset change

        if (!Application.isPlaying)
            stateMachine.Init();

        // add state cards
        foreach (State state in stateMachine.Root.SubStates.Values)
            AddCards(state, new StateCard(GetTitle(state)), this);

        // every state change, update the history (maybe do this on monobehaviour side? the editor would be limited to just the selected one)

    }

    public void AddCards(State state, StateCard card, VisualElement parent)
    {
        parent.Add(card);
        
        if (Application.isPlaying)
        {
            _stateMachine.stateChanged += (from, to) => {
                State next = to;

                card.disabled = true;

                while (next != null)
                {
                    if (next == state) card.disabled = false;

                    next = next.Parent;
                }
            };

            card.disabled = true;
        }

        foreach (State child in state.SubStates.Values)
            AddCards(child, new StateCard(GetTitle(child)), card);
    }

    public string GetTitle(State state)
    {
        string title = state.GetType().ToString();

        string[] values = title.Split(".");

        return values[values.Length - 1];
    }
}