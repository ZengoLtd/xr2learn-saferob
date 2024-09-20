using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class SetAlertBoxes : MonoBehaviour
{
    public static SetAlertBoxes instance;

    public AudioSource popUpSound;

    public Canvas alertBox;

    public Image icon;

    public Sprite successIcon;
    public Sprite errorIcon;

    public TMP_Text descriptionText;
    public TMP_Text titleText;

    public int showOnScreenTime = 15;
    public float fadeTime = 0.5f;
    public float delay = 1f;

    public bool isActive;

    private const string ERROR_KEY = "error_title";
    private const string SUCCESS_KEY = "success_title";
    private const string TABLE_NAME = "SafeRob";

    CanvasGroup canvasGroup;
    Coroutine activeCoroutine;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        canvasGroup = alertBox.GetComponent<CanvasGroup>();
        isActive = false;
        canvasGroup.alpha = 0;
    }

    public void ShowSuccessAlert(LocalizedString description)
    {
        ShowAlert(successIcon, GetLocalizedString(SUCCESS_KEY).GetLocalizedString(), description.GetLocalizedString());
    }

    public void ShowErrorAlert(LocalizedString description)
    {
        ShowAlert(errorIcon, GetLocalizedString(ERROR_KEY).GetLocalizedString(), description.GetLocalizedString());
    }

    private LocalizedString GetLocalizedString(string key)
    {
        return new LocalizedString(TABLE_NAME, key);
    }

    private void ShowAlert(Sprite iconSprite, string title, string description)
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }

        icon.sprite = iconSprite;
        titleText.text = title;
        descriptionText.text = description;

        if (!isActive)
        {
            isActive = true;
            alertBox.enabled = true;
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
        alertBox.enabled = false;
        isActive = false;

        if (HotspotHintSystem.Instance != null)
        {
            HotspotHintSystem.Instance.AllowHint();
        }
    }
}
