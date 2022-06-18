using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace HFSM
{
    [CustomEditor(typeof(StateMachine))]
    public class StateMachineInspector : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            StateMachine stateMachine = (StateMachine)target;
            // Create a new VisualElement to be the root of our inspector UI
            VisualElement myInspector = new VisualElement();

            // Load and clone a visual tree from UXML
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/HFSM/Editor/StateMachineInspector.uxml");

            visualTree.CloneTree(myInspector);

            StateMachineDebugger debugger = myInspector.Query<StateMachineDebugger>("StateMachineDebugger");

            debugger.Start(stateMachine);

            // Return the finished inspector UI
            return myInspector;
        }

        /*
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our inspector UI
            VisualElement myInspector = new VisualElement();

            // Add a simple label
            myInspector.Add(new Label("This is a custom inspector"));

            // Return the finished inspector UI
            return myInspector;
        }
        */
    }
}