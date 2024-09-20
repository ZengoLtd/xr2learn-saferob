using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashPlayerVR : MonoBehaviour
{
    public int loadSceneIndex = 1;
    public float distance = 3f;
    public Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3f;
    public float waitAfterVideo = 0.0f;

    VideoPlayer player;

    bool isPlaying;

    public Transform vplayerCanvas;

    private void Awake()
    {
        player = GetComponent<VideoPlayer>();
        player.Play();
    }
    private void Start()
    {
        isPlaying = true;
        StartCoroutine(LoadYourAsyncScene());
    }

    private void OnEnable()
    {
        player.loopPointReached += EndReached;
    }

    private void OnDisable()
    {
        player.loopPointReached -= EndReached;
    }

    private void EndReached(VideoPlayer source)
    {
        player.Stop();
        player.clip = null;
        isPlaying = false;
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadSceneIndex);
        asyncLoad.allowSceneActivation = false;
        // Wait until the asynchronous scene fully loads
        while (isPlaying)
        {
            yield return null;
        }
        yield return new WaitForSeconds(waitAfterVideo);
        asyncLoad.allowSceneActivation = true;
    }

    private void Update()
    {
        Vector3 targetPosition = Camera.main.transform.TransformPoint(new Vector3(0, 0, distance));

        vplayerCanvas.transform.position = Vector3.SmoothDamp(vplayerCanvas.transform.position, targetPosition, ref velocity, smoothTime);
        //var lookAtPos = new Vector3(GetComponent<Camera>().transform.position.x, GetComponent<Camera>().transform.position.y, GetComponent<Camera>().transform.position.z);
        vplayerCanvas.transform.LookAt(Camera.main.transform);
    }
}
