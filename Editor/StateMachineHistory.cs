using System.Collections;
using System.Collections.Generic;
using System;
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

        FirstButton.clicked += () => OnStateSelect(_stateMachine.History.SelectFirst());
        PreviousButton.clicked += () => OnStateSelect(_stateMachine.History.SelectPrevious());
        NextButton.clicked += () => OnStateSelect(_stateMachine.History.SelectNext());
        LastButton.clicked += () => OnStateSelect(_stateMachine.History.SelectLast());

        _stateMachine.History.stateSelected += OnStateSelect;
    }

    public void Destroy()
    {
        // TODO unsubscribe from this hell?
    }

    public void OnStateSelect(LinkedListNode<State> node)
    {
        PreviousInfo.state = node.Previous?.Value?.GetType().ToString();
        CurrentInfo.state = node.Value.GetType().ToString();

        activeIndex = _stateMachine.History.ActiveIndex.ToString();
        totalCount = _stateMachine.History.List.Count.ToString();

        stateSelected.Invoke(node);
    }
}