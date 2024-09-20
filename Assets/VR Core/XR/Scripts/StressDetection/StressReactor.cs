using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressReactor : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float EnableFromStressLevel = 0.0f;
    [Range(0.0f, 1.0f)]
    public float EnableToStressLevel = 1.0f;
    
  
    public void OnEnable(){
        if(StressManager.Instance == null){
            Debug.LogError("StressManager not found in scene");
            gameObject.SetActive(false);
            return;
        }
        EventManager.OnStressDataReceived += ReactToStressData;
    }
    public void OnDisable(){
        EventManager.OnStressDataReceived -= ReactToStressData;
    }
    protected virtual void Toggle(bool state){
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(state);
        }
    }
    public void ReactToStressData(StressData data){

        if(data.StressLevel >= EnableFromStressLevel && data.StressLevel <= EnableToStressLevel){
            Toggle(true);
            return;
        }
       
        Toggle(false);  
        
    }
}
