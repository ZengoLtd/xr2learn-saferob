using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class TeleportActivationArea : MonoBehaviour
{
      void OnDrawGizmos(){
        
        Gizmos.color = new Color(0.86f, 0.47f, 0.05f, 0.5f);
        Gizmos.DrawCube(transform.position +GetComponent<BoxCollider>().center, Vector3.Scale(GetComponent<BoxCollider>().size,transform.lossyScale));
        
    }
    void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "Player"){
            transform.parent.GetComponent<TeleportDestinationMarker>().SetAsLastTargetFromArea();
            
        }
    }
}
