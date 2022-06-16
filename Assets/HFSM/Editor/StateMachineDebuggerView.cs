using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class StateMachineDebuggerView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<StateMachineDebuggerView, VisualElement.UxmlTraits>
    {
        public override VisualElement Create(IUxmlAttributes bag, CreationContext cc)
        {
            VisualElement root = base.Create(bag, cc);

            Foldout foldout = new Foldout();
            foldout.text = "Debugger";
            foldout.value = false;

            PopupWindow stateA = new PopupWindow();
            stateA.text = "State A";
            foldout.Add(stateA);

            PopupWindow stateAI = new PopupWindow();
            stateAI.text = "State A I";
            stateA.contentContainer.Add(stateAI);

            PopupWindow stateAII = new PopupWindow();
            stateAII.text = "State A II";
            stateA.contentContainer.Add(stateAII);

            PopupWindow stateB = new PopupWindow();
            stateB.text = "State B";
            foldout.Add(stateB);

            root.Add(foldout);

            return root;
        }
    }
}