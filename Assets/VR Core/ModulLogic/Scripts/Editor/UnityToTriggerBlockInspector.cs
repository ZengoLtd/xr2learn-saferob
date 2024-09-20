using UnityEditor;
using UnityEngine.UIElements;


[CustomEditor(typeof(UnityToTriggerBlock), true, isFallback = true)]
public class UnityToTriggerBlockInspector : BlockLogicBaseInspector
{
     public override VisualElement CreateInspectorGUI(){

        var baseUI =  base.CreateInspectorGUI();
        
        //myInspector.Q("BindingsFoldout").style.display = DisplayStyle.None;
        //myInspector.Q("LogicFoldout").style.display = DisplayStyle.None;
        myInspector.Q("listenersContent").style.display = DisplayStyle.None;
 
        return baseUI;
     }
}
