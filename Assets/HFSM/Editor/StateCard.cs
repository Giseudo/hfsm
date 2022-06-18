using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;
using HFSM;

public class StateCard : VisualElement
{
    public new class UxmlFactory : UxmlFactory<StateCard, VisualElement.UxmlTraits>
    { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription _title = new UxmlStringAttributeDescription { name = "title", defaultValue = "State Name" };
 
        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }
 
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var ate = ve as StateCard;

            ate.title = _title.GetValueFromBag(bag, cc);

            // FIXME this is not being executed..
            UnityEngine.Debug.Log("OOI");
        }
    }

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

    public override VisualElement contentContainer => this.Query<VisualElement>("Content");

    public string title {
        get { return this.Q<Label>(null, "state-card__title").text; }
        set { this.Q<Label>(null, "state-card__title").text = value; }
    }
}