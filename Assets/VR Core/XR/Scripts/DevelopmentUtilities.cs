using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DevelopmentUtilities 
{
    public static string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }
}
