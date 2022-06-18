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
            VisualElement root = new VisualElement();

            try
            {
                VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.gigi.HFSM/Editor/StateMachineInspector.uxml");
                visualTree.CloneTree(root);
            }
            catch
            {
                VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/HFSM/Editor/StateMachineInspector.uxml");
                visualTree.CloneTree(root);
            }

            StateMachineDebugger debugger = root.Query<StateMachineDebugger>("StateMachineDebugger");

            debugger.Start(stateMachine);

            return root;
        }
    }
}