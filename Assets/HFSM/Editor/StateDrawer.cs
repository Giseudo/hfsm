using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace HFSM
{
    [CustomPropertyDrawer(typeof(State))]
    public class StatePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            container.Add(new Label("oie"));

            return container;
        }
    }
}