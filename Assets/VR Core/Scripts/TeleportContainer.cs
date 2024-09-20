using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportContainer : MonoBehaviour
{
    public static TeleportContainer Instance;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

    }


}
