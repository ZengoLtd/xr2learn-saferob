
using UnityEditor;
using UnityEngine.UIElements;



[CustomEditor(typeof(ModulEvents), true, isFallback = true)]
public class ModulEventsInspector : BlockLogicBaseInspector
{
     public override VisualElement CreateInspectorGUI(){

        var baseUI =  base.CreateInspectorGUI();
        
        myInspector.Q("bindingsContent").style.display = DisplayStyle.None;
        //myInspector.Q("LogicFoldout").style.display = DisplayStyle.None;
        //myInspector.Q("ListenersFoldout").style.display = DisplayStyle.None;

        return baseUI;
     }
}
