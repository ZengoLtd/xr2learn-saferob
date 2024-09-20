using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleGoals : MonoBehaviour
{
    public static ModuleGoals Instance;

    public bool? optimalUse = null;

    protected virtual void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple ModuleGoals instances found");
            Destroy(this);
        }
    }
    public virtual List<ModuleTask> Report(){
        return null;
    }
}
