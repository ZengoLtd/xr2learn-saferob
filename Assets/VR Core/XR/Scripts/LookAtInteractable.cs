using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LookAtInteractable : MonoBehaviour
{


    public float triggerDistance = 3.0f;
  

    public UnityAction OnTrigger;

    void OnEnable(){
        EventManager.RegisterLookAtInteractable(this);
    }

    void OnDisable(){
        EventManager.UnregisterLookAtInteractable(this);
    }
    public void Trigger(Vector3 lookAt)
    {
        if(Vector3.Distance(transform.position,lookAt)<triggerDistance){
            OnTrigger.Invoke();
        }
        
    }
    
}
