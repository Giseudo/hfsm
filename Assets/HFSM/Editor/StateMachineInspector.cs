using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace HFSM
{
    [CustomEditor(typeof(StateMachine))]
    public class StateMachineInspector : Editor
    {
        StateMachineDebugger _debugger;
        StateMachine _stateMachine;

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

            _stateMachine = stateMachine;
            _stateMachine.assetChanged += OnAssetChange;

            _debugger = root.Query<StateMachineDebugger>("StateMachineDebugger");
            _debugger?.Start(stateMachine);

            return root;
        }

        public void OnDestroy()
        {
            _stateMachine.assetChanged -= OnAssetChange;

            _debugger?.Destroy();
        }

        public void OnAssetChange(StateMachineAsset asset)
        {
            _debugger?.Clear();
            _debugger?.Start(_stateMachine);
        }
    }
}