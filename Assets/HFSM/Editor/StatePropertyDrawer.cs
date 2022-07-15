using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using HFSM;

[CustomPropertyDrawer(typeof(State))]
public class StatePropertyDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create a new VisualElement to be the root the property UI
        var container = new VisualElement();

        // Create drawer UI using C#
        var popup = new UnityEngine.UIElements.PopupWindow();
        popup.text = "State";
        popup.Add(new PropertyField(property.FindPropertyRelative("_children"), "Sub States"));
        container.Add(popup);

        // Return the finished UI
        return container;
    }
}