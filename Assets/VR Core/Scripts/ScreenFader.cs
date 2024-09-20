using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{

    public Animator transition;

    public float transitionTime;

    public static ScreenFader Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this){
            Debug.LogError("Duplicated Instance");
            Destroy(this);
        }else
        {
            Instance = this;
        }
    }

    public void StartTransition()
    {
        //StartCoroutine(Transition());
        transition.Play("CrossFade_start");
    }


    IEnumerator Transition()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
    }
}
