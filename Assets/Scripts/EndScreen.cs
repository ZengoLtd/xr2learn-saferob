using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{

    public GameObject TaskHolder;
    public GameObject TaskPrefab;
    public GameObject Time;
    public GameObject Teleport;
    public GameObject TopPanel;
    Canvas canvas;
  
    void OnEnable(){
        canvas = GetComponent<Canvas>();
        if(StressManager.Instance == null){
            TopPanel.SetActive(false);
        }
    }
    

    public void ShowUI(){
        //destroy every child of taskholder
        foreach (Transform child in TaskHolder.transform)
        {
            Destroy(child.gameObject);
        }
      
        //get report
        
        List<ModuleTask> tasks = ModuleGoals.Instance.Report();

        for(int i = 0; i < tasks.Count; i++){
            if (i == 0)
            {
                Time.GetComponent<TMP_Text>().text = tasks[i].taskDescription;
            }
            else if (i == 1)
            {
                Teleport.GetComponent<TMP_Text>().text = tasks[i].taskDescription;
                LayoutRebuilder.ForceRebuildLayoutImmediate(Teleport.transform.parent as RectTransform);
            }
            else
            {
                var instance = Instantiate(TaskPrefab, TaskHolder.transform, false);
                instance.transform.parent = TaskHolder.transform;
                instance.GetComponent<TMP_Text>().text = tasks[i].taskDescription;
                if (tasks[i].success)
                {
                    instance.transform.GetChild(0).gameObject.SetActive(true);
                    instance.transform.GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    instance.transform.GetChild(0).gameObject.SetActive(false);
                    instance.transform.GetChild(1).gameObject.SetActive(true);
                }
                if (!tasks[i].isSuccessStateVisible)
                {
                    instance.transform.GetChild(0).gameObject.SetActive(false);
                    instance.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
              
        }
        canvas.enabled = true;

    }

    public void EndModule(){
        PhotonNetwork.Disconnect();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(currentScene);
        SceneLoadManager.Instance.LoadMenu();
    }
}
