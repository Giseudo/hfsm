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
        UxmlStringAttributeDescription m_String = new UxmlStringAttributeDescription { name = "string-attr", defaultValue = "default_value" };
        UxmlStringAttributeDescription m_StateName = new UxmlStringAttributeDescription { name = "state-name", defaultValue = "NewState" };
 
        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }
 
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var ate = ve as StateCard;
            // ate.stateName = m_StateName.GetValueFromBag(bag, cc);

            ate.stateName = m_String.GetValueFromBag(bag, cc);
            ate.Add(new TextField("String") { value = ate.stateName });
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

    public override VisualElement contentContainer => this.Query<VisualElement>("Content");

    public string stateName { get; set; }
}