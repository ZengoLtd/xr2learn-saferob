using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puppet : MonoBehaviour
{
    public static Puppet Instance;
    void OnEnable()
    {
        if(Instance != null && Instance != this)
        {
            Debug.LogError("Duplicated puppet instance");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

  
}
