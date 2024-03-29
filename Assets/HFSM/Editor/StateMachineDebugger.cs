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
        _stateMachine.started += PopulateCards;

        _history.style.display = !Application.isPlaying || stateMachine.Asset == null ? DisplayStyle.None : DisplayStyle.Flex;

        if (stateMachine.Asset == null) return;

        if (!Application.isPlaying)
            stateMachine.Init();
        else
            _history.Start(stateMachine);

        PopulateCards();
    }

    public void Update()
    {
        _history?.Update();
    }

    public void Destroy()
    {
        destroyed.Invoke();
        _stateMachine.started -= PopulateCards;

        if (!Application.isPlaying) return;

        _history.Destroy();
    }

    private void PopulateCards()
    {
        if (_stateMachine.Root == null) return;

        foreach (State state in _stateMachine.Root.SubStates.Values)
            AddCards(state, new StateCard(state.Name), this);
    }

    public void AddCards(State state, StateCard card, VisualElement parent)
    { 
        if (Application.isPlaying)
        {
            // Set initial active cards on play mode
            card.disabled = true;

            State next = _stateMachine.Root.CurrentSubState;

            while(next != null)
            {
                if (next == state)
                {
                    card.disabled = false;
                    card.selected = next.IsLeaf;
                }

                next = next.CurrentSubState;
            }

            Action<LinkedListNode<State>> onStateChange = (node) => {
                State next = node.Value;

                card.disabled = true;
                card.selected = false;

                while (next != null)
                {
                    if (next == state)
                    {
                        card.disabled = false;
                        card.selected = next.IsLeaf;
                    }

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
            AddCards(child, new StateCard(child.Name), card);
    }

    public void Reset()
    {
        Clear();

        _stateMachine.History.Clear();

        if (_stateMachine.Asset == null) return;

        PopulateCards();
    }
}