using MEM;

using UnityEngine;

public class HintUI : MonoBehaviour
{
    public AudioSource hintPopupSound;

    public string LogEvent = "";
    public int delay = 3;
    public int lifeTime=15;
    public float fadeTime = 1f;

    public bool isActive;
    public bool isAlreadyActivated;

    public bool isSceneSelector;
    public bool isFallingHint;
    public bool isDangerHint;

    TriggerAll trigger;
    Canvas canvas;

    private void Start()
    {
        isAlreadyActivated = false;
        isActive = false;
        trigger = GetComponent<TriggerAll>();
        canvas = GetComponent<Canvas>();
        GetComponent<CanvasGroup>().LeanAlpha(0, 0);
        if (isSceneSelector)
        {
            ShowHint();
        }
    }


    public void ShowHint()
    {
        GameObject[] hints;
        hints = GameObject.FindGameObjectsWithTag("Hints");
        foreach (GameObject hint in hints)
        {
            if (hint.GetComponent<HintUI>() != null && hint.GetComponent<HintUI>().isActive && !hint.GetComponent<HintUI>().isFallingHint && hint != this.gameObject)
            {
                hint.GetComponent<HintUI>().TurnOffHint();
            }
        }
        if (isAlreadyActivated)
        {
            return;
        }
        else
        {
            if(canvas == null)
            {
                Debug.Log("Canvas is null");
                return;
            }
            canvas.enabled = true;
            isActive = true;
            Invoke("TimerHint", delay);
            if(LogEvent != ""){
                CommunicationManager.Instance?.Log(LogEvent);
            }
            hintPopupSound?.Play();
        }
        
    }

    public void TriggerHints() {
        if (trigger != null)
        {
            trigger.TriggerAllEvents();
        }
    }

    void TimerHint()
    {
        GetComponent<CanvasGroup>().LeanAlpha(1, fadeTime);
        Invoke("TurnOffHint", lifeTime);
    }

    public void TurnOffHint()
    {
        GetComponent<CanvasGroup>().LeanAlpha(0, fadeTime+1);
        canvas.enabled = false;
        isActive = false;
        if (isFallingHint || isDangerHint  )
        {
            isAlreadyActivated = false;
        }
        else
        {
            isAlreadyActivated = true;
        }
        TriggerHints();
    }

    public void CheckForRopes()
    {
        ShowHint();
    }
}
