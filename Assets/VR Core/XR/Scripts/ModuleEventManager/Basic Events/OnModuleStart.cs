using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace MEM{

//removeable class
public class OnModuleStart : ModuleEventTriggerBase 
{
    void OnEnable()
    {
        EventManager.OnSceneStart += OnSceneStart;
    }
    void OnSceneStart()
    {
        TriggerAll();
    }
    void OnDisable()
    {
        EventManager.OnSceneStart -= OnSceneStart;
    }
}
}
