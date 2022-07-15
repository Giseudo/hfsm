using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;
using HFSM;

public class StateBadge : VisualElement
{
    public string text {
        get { return this.Q<Label>(null, "state-badge__label").text; }
        set { this.Q<Label>(null, "state-badge__label").text = value; }
    }
    public bool selected {
        get { return this._selected; }
        set
        {
            if (value)
                AddToClassList("state-badge--selected");
            else
                RemoveFromClassList("state-badge--selected");
        }
    }

    private bool _selected;

    public StateBadge()
    {
        try
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.gigi.HFSM/Editor/StateBadge.uxml");
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.gigi.HFSM/Editor/StateBadge.uss"));
            visualTree.CloneTree(this);
        }
        catch
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/HFSM/Editor/StateBadge.uxml");
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/HFSM/Editor/StateBadge.uss"));
            visualTree.CloneTree(this);
        }
    }

    public StateBadge(string text = "Current State") : this()
    {
        this.text = text;
    }

    public new class UxmlFactory : UxmlFactory<StateBadge, UxmlTraits>
    { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription _text = new UxmlStringAttributeDescription { name = "text", defaultValue = "State Name" };
        UxmlBoolAttributeDescription _selected = new UxmlBoolAttributeDescription { name = "selected", defaultValue = false };
 
        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }
 
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var ate = ve as StateBadge;

            ate.text = _text.GetValueFromBag(bag, cc);
            ate.selected = _selected.GetValueFromBag(bag, cc);
        }
    }
}