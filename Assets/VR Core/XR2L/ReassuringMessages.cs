using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using System.Collections;


enum Location
{
    STORAGE,
    GROUND,
    HEIGHT
}
public class ReassuringMessages : MonoBehaviour
{
    public static ReassuringMessages instance;
    public List<LocalizedString> reassuringMessages = new List<LocalizedString>();

    public List<LocalizedString> storageReassuringMessages = new List<LocalizedString>();
    public List<LocalizedString> groundReassuringMessages = new List<LocalizedString>();
    public List<LocalizedString> heightReassuringMessages = new List<LocalizedString>();
    public AudioSource popUpSound;
    public Canvas messageBox;
    public TMP_Text text;
    public float interval = 10f;
    public int showOnScreenTime = 15;
    public float fadeTime = 0.5f;
    public float delay = 1f;
    public bool isActive;
    CanvasGroup canvasGroup;
    Coroutine activeCoroutine;
    Coroutine displayCoroutine;
    float stresslevel = 0f;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        EventManager.OnStressDataReceived += ReactToStressData;
    }
    Location currentLocation = Location.GROUND;
    public void InStorage()
    {
        currentLocation = Location.STORAGE;
    }
    public void OnGround()
    {
        currentLocation = Location.GROUND;
    }

    public void OnHeight()
    {
       currentLocation = Location.HEIGHT;
    }   
    void ReactToStressData(StressData data)
    {
        stresslevel = data.StressLevel;
    }

    private void Start()
    {
        canvasGroup = messageBox.GetComponent<CanvasGroup>();
        isActive = false;
        canvasGroup.alpha = 0;
        StartDisplayingMessages();
    }

    private void StartDisplayingMessages()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = StartCoroutine(DisplayMessagesRoutine());
    }

    private IEnumerator DisplayMessagesRoutine()
    {
        while (true)
        {
            if(stresslevel < 0.1f)
            {
                yield return new WaitForSeconds(2.0f);
                continue;
            }
            yield return new WaitForSeconds(interval);
            DisplayRandomMessage();
        }
    }

    private void DisplayRandomMessage()
    {
        
        if(currentLocation == Location.STORAGE){
            int randomIndex = Random.Range(0, storageReassuringMessages.Count);
            text.text = storageReassuringMessages[randomIndex].GetLocalizedString();
        }else if(currentLocation == Location.GROUND){
            int randomIndex = Random.Range(0, groundReassuringMessages.Count);
            text.text = groundReassuringMessages[randomIndex].GetLocalizedString();
        }else if(currentLocation == Location.HEIGHT){
            int randomIndex = Random.Range(0, heightReassuringMessages.Count);
            text.text = heightReassuringMessages[randomIndex].GetLocalizedString();
        }
        
        ShowAlert();
        
    }

    private void ShowAlert()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }
        if (!isActive)
        {
            isActive = true;
            messageBox.enabled = true;
            canvasGroup.alpha = 0;
            canvasGroup.LeanAlpha(1, fadeTime);
            if (HotspotHintSystem.Instance != null)
            {
                HotspotHintSystem.Instance.DenyHint();
            }
            if (popUpSound != null)
            {
                popUpSound.Play();
            }
        }
        activeCoroutine = StartCoroutine(TimerHint());
    }

    private IEnumerator TimerHint()
    {
        yield return new WaitForSeconds(showOnScreenTime);
        TurnOffHint();
    }

    public void TurnOffHint()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
        StartCoroutine(FadeOutAndDisable());
    }

    private IEnumerator FadeOutAndDisable()
    {
        canvasGroup.LeanAlpha(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        messageBox.enabled = false;
        isActive = false;
        if (HotspotHintSystem.Instance != null)
        {
            HotspotHintSystem.Instance.AllowHint();
        }
    }

    private void OnDisable()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        EventManager.OnStressDataReceived -= ReactToStressData;
    }
}