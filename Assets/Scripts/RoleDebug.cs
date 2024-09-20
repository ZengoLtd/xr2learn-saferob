using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleDebug : MonoBehaviour
{
    public Material itMaterial;
    public Material electricianMaterial;

   
    void Update()
    {
       
        if(RoleSelector.Instance.playerRole == Role.IT){
            GetComponent<Renderer>().material = itMaterial;
        }
        else if(RoleSelector.Instance.playerRole == Role.Electrician){
            GetComponent<Renderer>().material = electricianMaterial;
        }
    }
}
