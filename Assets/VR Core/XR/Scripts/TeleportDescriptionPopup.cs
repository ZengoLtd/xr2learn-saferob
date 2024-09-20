using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

#if UNITY_EDITOR
public class TeleportDescriptionPopup : EditorWindow
{
    public BNG.TeleportDestination targetTeleport;
    public List<BNG.TeleportDestination> Destinations = new List<BNG.TeleportDestination>();
    Vector2 scrollPosition;
    [MenuItem("Zengo/TeleportEditor")]
    public static void ShowWindow()
    {
        TeleportDescriptionPopup window = (TeleportDescriptionPopup)EditorWindow.GetWindow(typeof(TeleportDescriptionPopup));
        window.titleContent.text = "Teleport Target Editor";
        window.GetTeleportDestinations();
    }

    void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(GUILayout.Width(200));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Available Teleports");
        if (GUILayout.Button("Refresh", GUILayout.Width(80)))
        {
            GetTeleportDestinations();
        }

        GUILayout.EndHorizontal();
        foreach (var teleport in Destinations)
        {
            GUI.enabled = (teleport != targetTeleport);
            if (GUILayout.Button(teleport.name))
            {
                targetTeleport = teleport;
            }
            GUI.enabled = true;
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        TargetPanel();
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
    }

    public void GetTeleportDestinations()
    {

        Destinations.Clear();
        //get every object in scene
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            //check if object has a teleport destination component
            BNG.TeleportDestination destination = go.GetComponent<BNG.TeleportDestination>();
            if (destination != null)
            {
                //add to list
                Destinations.Add(destination);
            }
        }
        if (Destinations.Count > 0)
        {
            targetTeleport = Destinations[0];
        }


    }

    void TargetPanel()
    {
        Color defaultColor = GUI.color;
        var destinations = targetTeleport?.gameObject?.GetComponent<TeleportDestinationMarker>()?.Destinations;
        var baddestinations = targetTeleport?.gameObject?.GetComponent<TeleportDestinationMarkerWithBadLocations>()?.BadDestinations;
        
        GUILayout.Label("Enabled Destinations from " + targetTeleport?.name);
        if (GUILayout.Button("Remove all", GUILayout.Width(80)))
        {
            destinations.Clear();
            baddestinations.Clear();
        }
        foreach (var teleport in Destinations)
        {
            if (teleport == targetTeleport)
            {
                continue;
            }

            GUILayout.BeginHorizontal();

            if (destinations.Contains(teleport))
            {

                if (GUILayout.Button("Remove", GUILayout.Width(80)))
                {
                    destinations.Remove(teleport);
                    if(baddestinations.Contains(teleport)){
                        baddestinations.Remove(teleport);
                    }
                }
                GUILayout.Label(teleport.name);
            }
            else
            {

            }


            GUILayout.EndHorizontal();

        }

        GUILayout.Label("Available Destinations");
        if (GUILayout.Button("Add all", GUILayout.Width(80)))
        {
            foreach (var teleport in Destinations)
            {
                destinations.Add(teleport);
            }


        }
        foreach (var teleport in Destinations)
        {
            if (teleport == targetTeleport)
            {
                continue;
            }
            GUILayout.BeginHorizontal();


            if (!destinations.Contains(teleport))
            {

                if (GUILayout.Button("Add", GUILayout.Width(80)))
                {

                    destinations.Add(teleport);
                }
                GUILayout.Label(teleport.name);
            }


            GUILayout.EndHorizontal();

        }
        if (targetTeleport != null)
        {
            EditorUtility.SetDirty(targetTeleport?.gameObject?.GetComponent<TeleportDestinationMarker>());

        }
        //// bad teleports
        if(baddestinations == null){
            return;
        }
        GUILayout.Label("Add possible destinations first!");
        GUILayout.Label("Enabled Dangerous Destinations from " + targetTeleport?.name);
        if (GUILayout.Button("Remove all", GUILayout.Width(80)))
        {
            baddestinations.Clear();
        }
        foreach (var teleport in Destinations)
        {
            if (teleport == targetTeleport)
            {
                continue;
            }

            GUILayout.BeginHorizontal();

            if (baddestinations.Contains(teleport))
            {
                GUI.color = Color.red; // Set the GUI color to red
                if (GUILayout.Button("Remove", GUILayout.Width(80)))
                {
                    baddestinations.Remove(teleport);
                }
                GUILayout.Label(teleport.name);
            }
         
            GUILayout.EndHorizontal();
            GUI.color =  defaultColor;
        }

        GUILayout.Label("Available Dangerous Destinations");
        if (GUILayout.Button("Add all", GUILayout.Width(80)))
        {
            foreach (var teleport in destinations)
            {
                baddestinations.Add(teleport);
            }


        }
        foreach (var teleport in destinations)
        {
            if (teleport == targetTeleport)
            {
                continue;
            }
            GUILayout.BeginHorizontal();


            if (!baddestinations.Contains(teleport))
            {
                GUI.color = Color.red; // Set the GUI color to red
                if (GUILayout.Button("Add", GUILayout.Width(80)))
                {
                    baddestinations.Add(teleport);
                }
                
                GUILayout.Label(teleport.name);
            }


            GUILayout.EndHorizontal();

        }
        if (targetTeleport != null)
        {
            EditorUtility.SetDirty(targetTeleport?.gameObject?.GetComponent<TeleportDestinationMarkerWithBadLocations>());

        }
       
    }
}
#endif