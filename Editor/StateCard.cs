using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;
using HFSM;

public class StateCard : VisualElement
{
    static string DisabledClass = "state-card--disabled";
    static string SelectedClass = "state-card--selected";

    public override VisualElement contentContainer => this.Query<VisualElement>("Content");

    public string title {
        get { return this.Q<Label>(null, "state-card__title").text; }
        set { this.Q<Label>(null, "state-card__title").text = value; }
    }

    public bool disabled {
        get { return _disabled; }
        set {
            _disabled = value;

            if (disabled) AddToClassList(DisabledClass);
            else RemoveFromClassList(DisabledClass);
        }
    }

    public bool selected {
        get { return this._selected; }
        set
        {
            if (value) AddToClassList(SelectedClass);
            else RemoveFromClassList(SelectedClass);
        }
    }

    private bool _disabled;
    private bool _selected;

    public StateCard()
    {
        try
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.gigi.HFSM/Editor/StateCard.uxml");
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.gigi.HFSM/Editor/StateCard.uss"));
            visualTree.CloneTree(this);
        }
        catch
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/HFSM/Editor/StateCard.uxml");
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/HFSM/Editor/StateCard.uss"));
            visualTree.CloneTree(this);
        }
    }

    public StateCard(string title = "State Name") : this()
    {
        this.title = title;
    }

    public new class UxmlFactory : UxmlFactory<StateCard, UxmlTraits>
    { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription _title = new UxmlStringAttributeDescription { name = "title", defaultValue = "State Name" };
        UxmlBoolAttributeDescription _disabled = new UxmlBoolAttributeDescription { name = "disabled", defaultValue = false };
        UxmlBoolAttributeDescription _selected = new UxmlBoolAttributeDescription { name = "selected", defaultValue = false };
 
        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }
 
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var ate = ve as StateCard;

            ate.title = _title.GetValueFromBag(bag, cc);
            ate.selected = _selected.GetValueFromBag(bag, cc);
            ate.disabled = _disabled.GetValueFromBag(bag, cc);
        }
    }
}