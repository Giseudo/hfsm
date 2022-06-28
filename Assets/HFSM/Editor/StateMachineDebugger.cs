using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using HFSM;

public class StateMachineDebugger : VisualElement
{
    private StateMachine _stateMachine;
    private StateMachineHistory _history;
    public Action destroyed = delegate { };

    public override VisualElement contentContainer => this.Query<VisualElement>("Content");

    public new class UxmlFactory : UxmlFactory<StateMachineDebugger, VisualElement.UxmlTraits>
    { }

    public StateMachineDebugger()
    {
        try
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.gigi.HFSM/Editor/StateMachineDebugger.uxml");
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.gigi.HFSM/Editor/StateMachineDebugger.uss"));
            visualTree.CloneTree(this);
        }
        catch
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/HFSM/Editor/StateMachineDebugger.uxml");
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/HFSM/Editor/StateMachineDebugger.uss"));
            visualTree.CloneTree(this);
        }

        _history = this.Q<StateMachineHistory>("History");
    }

    public void Start(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _history.Start(stateMachine);

        if (stateMachine.Asset == null) return;

        // TODO add support to empty asset & asset change

        if (!Application.isPlaying)
            stateMachine.Init();

        foreach (State state in stateMachine.Root.SubStates.Values)
            AddCards(state, new StateCard(FormatTitle(state)), this);
    }

    public void Destroy() => destroyed.Invoke();

    public void AddCards(State state, StateCard card, VisualElement parent)
    { 
        if (Application.isPlaying)
        {
            // Set initial active cards on play mode
            card.disabled = true;

            State next = _stateMachine.Root.CurrentSubState;

            while(next != null)
            {
                if (next == state) card.disabled = false;

                next = next.CurrentSubState;
            }

            Action<LinkedListNode<State>> onStateChange = (node) => {
                State next = node.Value;

                card.disabled = true;

                while (next != null)
                {
                    if (next == state) card.disabled = false;

                    next = next.Parent;
                }
            };

            _history.stateSelected += onStateChange;
            destroyed += () => _history.stateSelected -= onStateChange;
        }

        // Add state to parent
        parent.Add(card);

        // Add children states
        foreach (State child in state.SubStates.Values)
            AddCards(child, new StateCard(FormatTitle(child)), card);
    }

    public string FormatTitle(State state)
    {
        string title = state.GetType().ToString();

        string[] values = title.Split(".");

        return values[values.Length - 1];
    }
}