
using UnityEditor;
using UnityEngine.UIElements;



[CustomEditor(typeof(OnGrab), true, isFallback = true)]
public class OnGrabInspector : BlockLogicBaseInspector
{
     public override VisualElement CreateInspectorGUI(){

        var baseUI =  base.CreateInspectorGUI();
        
        myInspector.Q("bindingsContent").style.display = DisplayStyle.None;
        //myInspector.Q("LogicFoldout").style.display = DisplayStyle.None;

        return baseUI;
     }
}
