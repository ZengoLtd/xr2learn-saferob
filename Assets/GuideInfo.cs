using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideInfo : MonoBehaviour
{
   
    void Start()
    {
        if(NetworkDataHolder.Instance == null){
            return;
        }
        if(NetworkDataHolder.Instance?.CheckValue("Guided") == null || (bool) NetworkDataHolder.Instance?.CheckValue("Guided") == false){
            gameObject.SetActive(false);
        }
    }

}
