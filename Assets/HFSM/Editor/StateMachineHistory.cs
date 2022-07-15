using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using HFSM;

public class StateMachineHistory : VisualElement
{
    private StateMachine _stateMachine;
    public string activeIndex {
        get { return this.Q<TextField>("ActiveIndex").value; }
        set { this.Q<TextField>("ActiveIndex").value = value; }
    }
    public string totalCount {
        get { return this.Q<Label>("TotalCount").text; }
        set { this.Q<Label>("TotalCount").text = $"/ {value}"; }
    }

    public Button FirstButton => this.Q<Button>("FirstButton");
    public Button PreviousButton => this.Q<Button>("PreviousButton");
    public Button NextButton => this.Q<Button>("NextButton");
    public Button LastButton => this.Q<Button>("LastButton");
    public StateInfo PreviousInfo => this.Q<StateInfo>("PreviousInfo");
    public StateInfo CurrentInfo => this.Q<StateInfo>("CurrentInfo");

    public Action<LinkedListNode<State>> stateSelected = delegate { };

    public new class UxmlFactory : UxmlFactory<StateMachineHistory, VisualElement.UxmlTraits>
    { }

    public StateMachineHistory()
    {
        try
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.gigi.HFSM/Editor/StateMachineHistory.uxml");
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.gigi.HFSM/Editor/StateMachineHistory.uss"));
            visualTree.CloneTree(this);
        }
        catch
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/HFSM/Editor/StateMachineHistory.uxml");
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/HFSM/Editor/StateMachineHistory.uss"));
            visualTree.CloneTree(this);
        }
    }

    public void Start(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;

        if (stateMachine.History == null) return;

        FirstButton.clicked += _stateMachine.History.SelectFirst;
        PreviousButton.clicked += _stateMachine.History.SelectPrevious;
        NextButton.clicked += _stateMachine.History.SelectNext;
        LastButton.clicked += _stateMachine.History.SelectLast;

        _stateMachine.History.stateSelected += OnStateSelect;
        _stateMachine.assetChanged += OnAssetChange;

        OnStateSelect(_stateMachine.History.Current);
    }

    private void OnAssetChange(StateMachineAsset asset)
    {
        _stateMachine.History.Clear();

        PreviousInfo.style.display = DisplayStyle.None;
        CurrentInfo.style.display = DisplayStyle.None;

        activeIndex = "0";
        totalCount = "0";
    }

    public void Destroy()
    {
        FirstButton.clicked -= _stateMachine.History.SelectFirst;
        PreviousButton.clicked -= _stateMachine.History.SelectPrevious;
        NextButton.clicked -= _stateMachine.History.SelectNext;
        LastButton.clicked -= _stateMachine.History.SelectLast;

        _stateMachine.History.stateSelected -= OnStateSelect;
    }

    public void OnStateSelect(LinkedListNode<State> node)
    {
        // Set previous state info
        bool hasPrevious = node?.Previous != null;

        CurrentInfo.style.display = DisplayStyle.Flex;
        PreviousInfo.style.display = !hasPrevious ? DisplayStyle.None : DisplayStyle.Flex;

        if (hasPrevious)
        {
            PreviousInfo.transitions = GetTransitions(node.Previous.Value);
            PreviousInfo.state = node.Previous.Value.Name;
        }

        // Set current state info
        CurrentInfo.state = node.Value.Name;
        CurrentInfo.transitions = GetTransitions(node.Value);
        CurrentInfo.stateBadge.selected = true;

        // Set actions values
        activeIndex = _stateMachine.History.ActiveIndex.ToString();
        totalCount = (_stateMachine.History.List.Count - 1).ToString();

        stateSelected.Invoke(node);
    }

    private string GetTransitions(State state)
    {
        State previousState = state;
        State currentState = state.Parent;
        string value = "";

        while (currentState != null)
        {
            var transitions = currentState.SubStates
                .Where(subState => subState.Value.GetType() != state.GetType())
                .Where(subState => subState.Value.GetType() != previousState.GetType())
                .Select(subState => subState.Value.Name);

            if (transitions.Count() > 0)
            {
                if (value != "")
                    value += ", ";

                value += String.Join(", ", transitions);
            }

            previousState = currentState;
            currentState = currentState.Parent;
        }

        return value;
    }
}