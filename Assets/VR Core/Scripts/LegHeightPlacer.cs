using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegHeightPlacer : MonoBehaviour
{
    
    public Transform LegIK;
    //raycast down, get the hit point world location

    float legHeight()
    {
        LayerMask mask = gameObject.layer;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit,1000,mask))
        {
            return hit.point.y;
        }
        return 0f; 
    }
    

    // Update is called once per frame
    void Update()
    {
        LegIK.position = new Vector3( LegIK.position.x, legHeight(),  LegIK.position.z);
    }
}
