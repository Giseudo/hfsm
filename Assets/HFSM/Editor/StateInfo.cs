using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;
using HFSM;

public class StateInfo : VisualElement
{
    static string DisabledClass = "state-info--disabled";

    public string title {
        get { return this.Q<Label>(null, "state-info__title").text; }
        set { this.Q<Label>(null, "state-info__title").text = value; }
    }

    public string state {
        get { return this.Q<StateBadge>(null, "state-info__state").text; }
        set { this.Q<StateBadge>(null, "state-info__state").text = value; }
    }

    public string time {
        get { return this.Q<Label>(null, "state-info__time").text; }
        set { this.Q<Label>(null, "state-info__time").text = value; }
    }

    public StateBadge stateBadge {
        get { return this.Q<StateBadge>("State"); }
    }

    public string transitions {
        get { return this._transitions; }
        set {
            VisualElement container = this.Q<VisualElement>(null, "state-info__transitions");

            container.Clear();

            string[] states = value.Split(", ");

            for (int i = 0; i < states.Length; i++)
            {
                string state = states[i];

                container.Add(new StateBadge(state));
            }

            this._transitions = value;
        }
    }
    private string _transitions;

    public bool disabled {
        get { return _disabled; }
        set {
            _disabled = value;

            if (disabled) AddToClassList(DisabledClass);
            else RemoveFromClassList(DisabledClass);
        }
    }

    private bool _disabled;

    public StateInfo()
    {
        try
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.gigi.HFSM/Editor/StateInfo.uxml");
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.gigi.HFSM/Editor/StateInfo.uss"));
            visualTree.CloneTree(this);
        }
        catch
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/HFSM/Editor/StateInfo.uxml");
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/HFSM/Editor/StateInfo.uss"));
            visualTree.CloneTree(this);
        }
    }

    public StateInfo(string title = "Current State") : this()
    {
        this.title = title;
    }

    public new class UxmlFactory : UxmlFactory<StateInfo, UxmlTraits>
    { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription _title = new UxmlStringAttributeDescription { name = "title", defaultValue = "CURRENT STATE" };
        UxmlBoolAttributeDescription _disabled = new UxmlBoolAttributeDescription { name = "disabled", defaultValue = false };
        UxmlStringAttributeDescription _state = new UxmlStringAttributeDescription { name = "state", defaultValue = "State A" };
        UxmlStringAttributeDescription _time = new UxmlStringAttributeDescription { name = "time", defaultValue = "00:00" };
        UxmlStringAttributeDescription _transitions = new UxmlStringAttributeDescription { name = "transitions", defaultValue = "State B, State C, State D" };
 
        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }
 
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var ate = ve as StateInfo;

            ate.title = _title.GetValueFromBag(bag, cc);
            ate.disabled = _disabled.GetValueFromBag(bag, cc);
            ate.state = _state.GetValueFromBag(bag, cc);
            ate.time = _time.GetValueFromBag(bag, cc);
            ate.transitions = _transitions.GetValueFromBag(bag, cc);
        }
    }
}