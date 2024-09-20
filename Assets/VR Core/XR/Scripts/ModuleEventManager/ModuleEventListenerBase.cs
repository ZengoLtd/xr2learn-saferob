using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MEM{
public class ModuleEventListenerBase : ModuleEventBase
{
    
    void OnEnable()
    {
        
       // if(variables == null){
       //     //Debug.LogError("Variables is null! "+ DevelopmentUtilities.GetGameObjectPath(this.gameObject));
       //     this.enabled = false;
       //     return;
       // }
       // ModuleEventManager.OnEvent += OnEvent;  
    }

    protected virtual void OnEvent(string eventName, object value) {
      //  if(!variables.declarations.IsDefined(eventName)){
      //      return;
      //  }
    }
    void OnDisable(){
        ModuleEventManager.OnEvent -= OnEvent;
    }
}
}