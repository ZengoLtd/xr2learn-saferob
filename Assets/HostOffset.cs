using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostOffset : MonoBehaviour
{
    
    void Awake()
    {   
        #if PHOTON_UNITY_NETWORKING
            if(Photon.Pun.PhotonNetwork.IsMasterClient)
            {
                transform.localPosition = new Vector3(0.3f, 0, 0.1f);
            }
        #endif
    }

    
}
