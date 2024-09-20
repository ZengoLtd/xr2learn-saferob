using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

public class RedOnLook : MonoBehaviour
{

    public Material highlightMaterial;
    public Material defaultMaterial;
    void Trigger(){
           GetComponent<MeshRenderer>().material = highlightMaterial;
            Debug.Log("trigger");
    }
     void OnEnable(){
        GetComponent<LookAtInteractable>().OnTrigger += Trigger;
    }


    void OnDisable(){
         GetComponent<LookAtInteractable>().OnTrigger -= Trigger;
    }
    void Update(){
        GetComponent<MeshRenderer>().material = defaultMaterial;
    }
}
