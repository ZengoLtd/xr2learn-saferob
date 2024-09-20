using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//TODO: We need to mark objects, what has a component (BlockLogicBase), but object don't has connections. This should mean an issue.

public class GizmoHandleEditor
{
    private static GizmoHandleEditor _instance;
    private GizmoHandleEditor() { }
    
    // Public static method to access the single instance of the class
    public static GizmoHandleEditor Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GizmoHandleEditor();
            }
            return _instance;
        }
    }
    
    private static Color _color = Color.cyan;
    private static float _bezierWidth;
    
    private static void OnSceneGUI(SceneView sceneView)
    {
        List<BlockLogicBase> blockLogicBases = new List<BlockLogicBase>(Resources.FindObjectsOfTypeAll<BlockLogicBase>());

        foreach(BlockLogicBase blockLogicBase in blockLogicBases){
            var returndata = new List<BlockLogicBase>();
            foreach (BlockLogicBase other in blockLogicBases)
            {
                if (other.ActivateOn.Find(x => x.block == blockLogicBase) != null)
                {
                    returndata.Add(other);
                }
            }
            
            if (blockLogicBase.ActivateOn.Count == 0 && returndata.Count == 0)
            {
                Handles.CircleHandleCap(0, blockLogicBase.gameObject.transform.position, Quaternion.identity, 0.3f, EventType.Repaint);
                Handles.Label(blockLogicBase.gameObject.transform.position, "Empty ActivateOn list");
                
                continue;
            }
            
            blockLogicBase.ActivateOn.ForEach(data =>
            {
                if (data.block == null)
                {
                    //Debug.Log("Null Reference");
                    Handles.CircleHandleCap(0, blockLogicBase.gameObject.transform.position, Quaternion.identity, 0.3f, EventType.Repaint);
                    Handles.Label(blockLogicBase.gameObject.transform.position, "Null Reference");
                    
                    return;
                }
                
                Vector3 startPoint = blockLogicBase.gameObject.transform.position;
                Vector3 endPoint = data.block.gameObject.transform.position;
                
                Vector3 startTangent = startPoint + Vector3.up * 2.0f;
                Vector3 endTangent = endPoint + Vector3.up * 2.0f;
            
                _bezierWidth = CalcBezierWidthDepOnZoom(startPoint, endPoint);
                
                Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, _color, null, _bezierWidth);
            


                Vector3 worldPosition = blockLogicBase.gameObject.transform.position;
                bool insideSceneView = SceneView.currentDrawingSceneView.camera.WorldToViewportPoint(worldPosition).z > 0;
                if (!insideSceneView)
                {
                    return;
                }
              
                Vector2 guiPosition = HandleUtility.WorldToGUIPoint(worldPosition);              
                Rect buttonRect = new Rect(guiPosition.x - 50, guiPosition.y - 25, 25, 25);
                Handles.BeginGUI();
                GUIStyle redStyle = new GUIStyle(GUI.skin.button);
                redStyle.normal.textColor = Color.white;
                Handles.DrawSolidRectangleWithOutline(buttonRect, Color.red, Color.red);

                if (GUI.Button(buttonRect, "+", redStyle))
                {
                    Debug.Log("Custom GUI Button clicked!");
                    Selection.activeGameObject = blockLogicBase.gameObject;
                    PopUpWindowModuleBlocks.ShowWindow();
                }

                Handles.EndGUI();

            });
        }
    }

    private static void DrawBezierCurvePoints(Vector3 startPoint, Vector3 startTangent, Vector3 endTangent,
        Vector3 endPoint)
    {
        for (float t = 0; t <= 1; t += 0.01f)
        {
            Vector3 bezierPoint = BezierPoint(startPoint, startTangent, endTangent, endPoint, t);
            Handles.DotHandleCap(0, bezierPoint, Quaternion.identity, 0.01f, EventType.Repaint);
        }
    }

    private static float CalcBezierWidthDepOnZoom(Vector3 startPoint, Vector3 endPoint)
    {
        float interpolateValue = 0.5f;
        Vector3 midPoint = Vector3.Lerp(startPoint, endPoint, interpolateValue);
        float distance = Vector3.Distance(SceneView.currentDrawingSceneView.camera.transform.position, midPoint);
        float width = 50.0f / distance;
        
        return width;
    }
    
    private static Vector3 BezierPoint(Vector3 startPoint, Vector3 startTangent, Vector3 endTangent, Vector3 endPoint, float t)
    {
        // Calculate the polynomial coefficients.
        float oneMinusT = 1 - t;
        float oneMinusTSquared = oneMinusT * oneMinusT;
        float oneMinusTCubed = oneMinusTSquared * oneMinusT;
        float tSquared = t * t;
        float tCubed = tSquared * t;

        // Calculate the Bezier point using the Bernstein polynomial.
        Vector3 point = oneMinusTCubed * startPoint; // (1-t)^3 * startPoint
        point += 3 * oneMinusTSquared * t * startTangent; // 3(1-t)^2 * t * startTangent
        point += 3 * oneMinusT * tSquared * endTangent; // 3(1-t) * t^2 * endTangent
        point += tCubed * endPoint; // t^3 * endPoint

        return point;
    }
    
    public void SubscribeToSceneGUI()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    public void UnSubscribeFromSceneGUI()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
}
