using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class StressData
{
    public float StressLevel;
    public string StressLabel;
}
public class StressSaveData{
    public float StressLevel;
    public string TimeStamp;
}

public class StressManager : MonoBehaviour
{
    public string providerURL = "";
    public int refreshInterval = 5;
    public static StressManager Instance;
    bool connected =false;
    bool saveData = false;
    StressData lastData;
    public void Seturl(string url)
    {
        providerURL = url;
    }
    private bool isRunning = true;
    public List<StressSaveData> stressData = new List<StressSaveData>();
    void OnSceneLoad(){
        Debug.Log("OnSceneLoad");
        if(!connected){
            gameObject.SetActive(false);
            return; 
        }
        saveData = true;
        StartCoroutine(StressSaveRoutine());
    }
    IEnumerator StressSaveRoutine(){
        yield return null;
        float time = 0;
        while(saveData){
            string timeStamp = $"{(int)time / 60:00}:{time % 60:00}";
            stressData.Add(new StressSaveData { StressLevel = lastData.StressLevel, TimeStamp = timeStamp });
            yield return new WaitForSeconds(15);
            time += 15;
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
         if (PlayerPrefs.HasKey("url"))
        {
            providerURL = PlayerPrefs.GetString("url");
        }
        StartCoroutine(RequestDataLoop()); 
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "SceneSelector" || scene.name == "Persistent"){
            return;
        }
        Debug.Log(":::::::::::::::"+scene.name);
        OnSceneLoad();
    }
    void OnEnable()
    {
        EventManager.OnBeforeSceneChange += OnSceneLoad;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        EventManager.OnBeforeSceneChange -= OnSceneLoad;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Instance = null;    
    }

    IEnumerator GetRequest()
    {
        yield return null;
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://"+providerURL+"/getstress"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log("Some error on web request");
                    Debug.Log(webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    connected = true;
                    Debug.Log("Received: " + webRequest.downloadHandler.text);
                    string json = webRequest.downloadHandler.text;
                    StressData data = JsonUtility.FromJson<StressData>(json);
                    lastData = data;
                    Debug.Log("Stress Level: " + data.StressLevel);
                    Debug.Log("Stress Label: " + data.StressLabel);
                    EventManager.StressDataReceived(data);
                    break;
            }
        }
    }

    IEnumerator RequestDataLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(refreshInterval);
            if (isRunning)
            {
                StartCoroutine(GetRequest());
            }
        }

    }
}
