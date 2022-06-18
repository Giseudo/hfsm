using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using HFSM;

public class StateMachineDebugger : VisualElement
{
    public new class UxmlFactory : UxmlFactory<StateMachineDebugger, VisualElement.UxmlTraits>
    {
        public override VisualElement Create(IUxmlAttributes bag, CreationContext cc)
        {
            VisualElement root = base.Create(bag, cc);

            return root;
        }
    }

    public void Start(StateMachine stateMachine)
    {
        StateCard stateA = new StateCard();
        stateA.stateName = "State A";
        Add(stateA);

        /*
        PopupWindow stateAI = new PopupWindow();
        stateAI.text = "State A I";
        stateA.contentContainer.Add(stateAI);

        PopupWindow stateAII = new PopupWindow();
        stateAII.text = "State A II";
        stateA.contentContainer.Add(stateAII);

        PopupWindow stateB = new PopupWindow();
        stateB.text = "State B";
        foldout.Add(stateB);
        */

        // TODO create a state visual element class
        // TODO populate states
    }
}