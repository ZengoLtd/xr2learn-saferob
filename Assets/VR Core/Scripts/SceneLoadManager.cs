using System.Collections;
using System.Collections.Generic;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public Animator animator;

    Dictionary<string, Scene> loadedScenes = new Dictionary<string, Scene>();
    public static SceneLoadManager Instance;
    public string SceneSelectorScene = "Scenes/SceneSelector/SceneSelector";
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        LoadMenu();
    }
    public void LoadMenu()
    {

        LoadScene(SceneSelectorScene);
        CommunicationManager.Instance?.EndModule(null, true);
    }
    public void LoadMenuScene()
    {
        int operationIndex = SceneManager.sceneCount;
        SceneManager.LoadScene(SceneSelectorScene, LoadSceneMode.Single);

        Scene currentScene = SceneManager.GetSceneAt(operationIndex);
        EventManager.SceneStart();
        StartCoroutine(CheckSceneStartLocation(currentScene));
    }
    /*
        TODO: Handle bad sceneNames
    */
    public void LoadScene(string sceneName)
    {
#if PHOTON_UNITY_NETWORKING
#else
        if(loadedScenes.ContainsKey(sceneName))
        {
            return;
        }
#endif
        if (sceneName != SceneSelectorScene)
        {
            EventManager.BeforeSceneChange();
        }
#if PHOTON_UNITY_NETWORKING
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            ScreenFader.Instance?.StartTransition();
            PhotonNetwork.LoadLevel(sceneName);

            return;
        }
#endif
        Debug.Log("Loading scene: " + sceneName);
        //animator.SetTrigger("Start");
        int operationIndex = SceneManager.sceneCount;

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        ScreenFader.Instance?.StartTransition();
        Scene currentScene = SceneManager.GetSceneAt(operationIndex);
        // Set active scene to the one we just loaded

        loadedScenes.Add(sceneName, currentScene);
        List<string> removableKeys = new List<string>();
        foreach (var scene in loadedScenes)
        {
            if (scene.Key != sceneName)
            {
                try
                {
                    SceneManager.UnloadSceneAsync(scene.Value);
                }
                catch (System.Exception e)
                {
                    Debug.Log("Scene already loaded" + e.Message);
                }

                removableKeys.Add(scene.Key);
            }
        }
        foreach (var key in removableKeys)
        {
            loadedScenes.Remove(key);
        }

        StartCoroutine(CheckSceneStartLocation(currentScene));

    }
    IEnumerator CheckSceneStartLocation(Scene scene)
    {
        Debug.Log("Waiting scene to load");
        while (!scene.isLoaded)
        {
            yield return null;
        }
        Debug.Log("Scene loaded");
        CommunicationManager.Instance?.Log("Modul elindult", "start");
        SceneManager.SetActiveScene(scene);
        yield return new WaitForEndOfFrame();
        Debug.Log("Scene start");
        EventManager.SceneStart();

    }

    void OnDestroy()
    {
        Instance = null;
    }

}
