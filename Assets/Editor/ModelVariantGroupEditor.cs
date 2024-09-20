using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;



[CustomEditor(typeof(ModelVariantGroup))] // [CanEditMultipleObjects]
public class ModelVariantGroupEditor : Editor
{
	SerializedProperty activeRendererIndexProp;

   void OnEnable()
    {
        // Setup the SerializedProperties.
        activeRendererIndexProp = serializedObject.FindProperty ("activeRendererIndex");
    }

	public override void OnInspectorGUI()
    {
		serializedObject.Update ();
		ModelVariantGroup t = (ModelVariantGroup)target;
        //GUILayout.Label(t.gameObject.name);
        EditorGUILayout.LabelField(t.gameObject.name, EditorStyles.boldLabel);
        GUILayout.Space(5);
        if (t.enabled == false) return;
		t.UpdateRendererList();
        var names = FormatNames(t.GetRendererNames());
		activeRendererIndexProp.intValue = EditorGUILayout.Popup(activeRendererIndexProp.intValue, names);
		serializedObject.ApplyModifiedProperties ();
		t.ApplyRenderersChange();
		DrawDefaultInspector();
    }

    void OnSceneGUI () {
        serializedObject.Update ();
        ModelVariantGroup t = (ModelVariantGroup)target;
        if (t.enabled == false) return;
        if (UnityEngine.Event.current.button == 1)
        {
            
            if (UnityEngine.Event.current.type == EventType.MouseDown)
            {              
                UnityEngine.Event.current.Use();
                t.UpdateRendererList();
                GenericMenu menu = new GenericMenu();
                var names = t.GetRendererNames();
                for(int i = 0; i < names.Length; i++)
                {
                    var name = FormatName(names[i]);
                    menu.AddItem(new GUIContent(name), activeRendererIndexProp.intValue == i, ChangeRenderer, i);
                }
                menu.ShowAsContext();
            }
        }
    }
 
    string FormatName(string name)
    {
        return string.Join(" ", name.Split('_').ToList()
		.ConvertAll(word => word.Substring(0, 1).ToUpper() + word.Substring(1)));
    }

    string[] FormatNames(string[] names)
    {
        var formattedNames = names;
        for (int i=0; i<formattedNames.Length; i++) formattedNames[i] = FormatName(names[i]);
        return formattedNames;
    }

    void ChangeRenderer (object obj) {
        activeRendererIndexProp.intValue = (int)obj;
		serializedObject.ApplyModifiedProperties ();
        ModelVariantGroup t = (ModelVariantGroup)target;
		t.ApplyRenderersChange();
    }    
}
