using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class CutscenePlayer : MonoBehaviour
{
    public InputActionReference skipCutscene;

    public VideoPlayer videoPlayer;

    private void Start()
    {
        skipCutscene.action.Enable();
        skipCutscene.action.performed += TurnOffVideo;
        videoPlayer.loopPointReached += Endvideo;
    }

    private void Endvideo(VideoPlayer source)
    {
        StopPlayingVideo();
    }

    public void TurnOffVideo(InputAction.CallbackContext context)
    {
        StopPlayingVideo();
    }

    public void StopPlayingVideo()
    {
        gameObject.SetActive(false);
    }
}
