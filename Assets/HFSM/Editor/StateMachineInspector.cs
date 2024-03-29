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
        Foldout _debuggerFoldout;
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
            EditorApplication.update += Update;

            _debugger = root.Query<StateMachineDebugger>("StateMachineDebugger");
            _debugger?.Start(stateMachine);
            _debuggerFoldout = root.Query<Foldout>("DebuggerFoldout");

            return root;
        }

        public void Update()
        {
            if (!Application.isPlaying) return;

            if (!_debuggerFoldout.value) return;

            _debugger?.Update();
        }

        public void OnDestroy()
        {
            _stateMachine.assetChanged -= OnAssetChange;
            EditorApplication.update -= Update;

            _debugger?.Destroy();
        }

        public void OnAssetChange(StateMachineAsset asset)
        {
            _debugger?.Reset();
        }
    }
}