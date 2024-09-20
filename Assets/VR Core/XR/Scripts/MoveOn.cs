/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MEM{


//triggers unity event if the listened event is fired
public class MoveOn : ModuleEventListenerBase 
{
    public Vector3 targetPosition;
    protected override void OnEvent(string eventName, object value) {
        
        if(!variables.declarations.IsDefined(eventName)){
            return;
        }
          
        if((variables.declarations.Get(eventName).Equals(value))){
            transform.position = targetPosition;
        }
       
    }
}
}*/