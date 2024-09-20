using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using MEM;
using UnityEngine.SceneManagement;
//using Unity.VisualScripting;
using DS.Windows;
public class ModuleLogic : EditorWindow
{
    [MenuItem("Zengo/ModuleLogic")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ModuleLogic));
    }

    void OnGUI()
    {
     
        RemoveModuleLogicFromActiveScene();
        VisualiseLogic();
    }
    void VisualiseLogic()
    {
        if (GUILayout.Button("Visualise Logic", GUILayout.Width(130), GUILayout.Height(20)))
        {
            DSEditorWindow.Open();
        }
    }
    void RemoveModuleLogicFromActiveScene()
    {
        if (GUILayout.Button("Cleanup Module", GUILayout.Width(130), GUILayout.Height(20)))
        {
            if(EditorUtility.DisplayDialog("Remove Module Logic", "Are you sure you want to remove all Module Logic from the ["+SceneManager.GetActiveScene().name+"] active scene?", "Yes", "No")){
                //find all gameobjects in the scene
                
                List<GameObject> gameObjects = new List<GameObject>();
                gameObjects.AddRange(UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects());
                while(gameObjects.Count > 0){
                    GameObject go = gameObjects[0];
                    gameObjects.RemoveAt(0);
                    foreach(Transform child in go.transform){
                        gameObjects.Add(child.gameObject);
                    }
                    if(go.GetComponent<ModuleEventListenerBase>()!=null){
                        DestroyImmediate(go.GetComponent<ModuleEventListenerBase>());
                    }
                    if(go.GetComponent<ModuleEventTriggerBase>()!=null){
                        DestroyImmediate(go.GetComponent<ModuleEventTriggerBase>());
                    }
                   //if(go.GetComponent<Variables>()!=null){
                   //    DestroyImmediate(go.GetComponent<Variables>());
                   // }
                    if(go.transform.name == "ListeningVariables" || go.transform.name == "TriggerVariables"){
                        DestroyImmediate(go.gameObject);
                    }
                }
            }
        }
    }


}
