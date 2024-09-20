using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    public bool hasCustomSplash = false;
    public bool hasCustomLayerMask = false;
    public static PersistentManager Instance { get; private set; }
    private GameObject player;
    private GameObject playerController;
    private GameObject waist;
    public LayerMask uiLayerMask;

    public GameObject GetPlayer(){
        return player;
    }

    public GameObject GetPlayerController()
    {
        return playerController;
    }

    public GameObject GetWaist()
    {
        return waist;
    }
    void Awake(){
         
        foreach (GameObject go in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            Debug.Log(go.name);
            DontDestroyOnLoad(go);

        }   
        StartCoroutine(XRInteractionManagerFix());
    }
    IEnumerator XRInteractionManagerFix(){
        yield return new WaitForEndOfFrame();
        DontDestroyOnLoad(GameObject.Find("XRInteractionManager"));
    }
    
    void OnEnable(){
        if(Instance == null){
            Instance = this;
            player = GameObject.Find("HeadCollision");
            waist = GameObject.Find("Waist");
            playerController = GameObject.Find("PlayerController");
            return;
        }
        if(Instance != this){
            Destroy(gameObject);
        }

       
       
    }
    
    void OnDisable(){
        if(Instance == this){
            Instance = null;
        }
        
    }
    

    
}
