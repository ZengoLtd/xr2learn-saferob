using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GizmoHandleMenuItem : MonoBehaviour
{
    [MenuItem("Zengo/Gizmo Handle", false, 10)]
    static void CreateGizmoHandle()
    {
        SceneView.duringSceneGui += OnGUI;
        
        GizmoHandleEditor.Instance.SubscribeToSceneGUI();
    }
    
    private static Rect windowRect = new Rect(20, 20, 120, 50);

    private static void OnGUI(SceneView sceneView)
    {
        // Define the maximum position of the window
        float maxX = sceneView.position.width - windowRect.width;
        float maxY = sceneView.position.height - windowRect.height;

        // Clamp the position of the window
        windowRect.x = Mathf.Clamp(windowRect.x, 0, maxX);
        windowRect.y = Mathf.Clamp(windowRect.y, 0, maxY);

        // Create the window
        windowRect = GUILayout.Window(0, windowRect, WindowFunction, "Gizmo Handle Editor");
    }

    private static void WindowFunction(int windowID)
    {
        if (GUILayout.Button("Unsubscribe from GizmoHandleEditor"))
        {
            GizmoHandleEditor.Instance.UnSubscribeFromSceneGUI();
            SceneView.duringSceneGui -= OnGUI;
        }

        // Make the window draggable
        GUI.DragWindow();
    }
}
