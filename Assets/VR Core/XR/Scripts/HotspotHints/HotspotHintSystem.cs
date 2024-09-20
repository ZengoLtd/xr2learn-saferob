using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class HotspotHintSystem : MonoBehaviour
{
    public static HotspotHintSystem Instance;
    public TMP_Text text;
    public string test;
    public float stayTime;

    public GameObject currentGrab;
    Canvas canvas;
    CanvasGroup canvasGroup;

    bool isDenied = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponent<Canvas>();
        Instance = this;
    }

    public void DenyHint()
    {
        isDenied = true;
        ClearHint();
    }

    public void AllowHint()
    {
        isDenied = false;
    }

    public void ShowHotspotHint(LocalizedString displayText = null)
    {
        if(!isDenied && displayText != null)
        {
            canvas.enabled = true;
            text.text = displayText.GetLocalizedString();
            test = text.text;
        }
    }

    void FadeAlpha()
    {
        GetComponent<CanvasGroup>().LeanAlpha(0, 0.2f).delay = stayTime;
    }

    public void ClearHint()
    {
        canvas.enabled = false;
        text.text = null;
    }
}
