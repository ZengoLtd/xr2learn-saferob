using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuToggleManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Toggle thisToggle;
    public TMP_Text buttonText;

    public Color defaultColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color selectedColor = Color.green;
    public Toggle[] togglesToDisable;
    public bool isChanging = false; 


    void Start()
    {
        thisToggle = GetComponent<Toggle>();
        thisToggle.onValueChanged.AddListener(delegate { OnToggleChanged(); });
        OnToggleChanged(); // Call this on start to apply the default state
    }

    public void OnToggleChanged()
    {
        if (isChanging) return;
        if (thisToggle.isOn)
        {
            buttonText.color = selectedColor;
            DisableOtherToggles(true);
        }
        else
        {
            buttonText.color = defaultColor;
            DisableOtherToggles(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!thisToggle.isOn)
        {
            buttonText.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!thisToggle.isOn)
        {
            buttonText.color = defaultColor;
        }
    }

    void DisableOtherToggles(bool disable)
    {
        foreach (var toggle in togglesToDisable)
        {
            toggle.isOn = false;
            toggle.interactable = !disable;
        }
    }
}
