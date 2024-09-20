using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class Keyboard : MonoBehaviour
{
    public Button sendButton;
    public TMP_InputField inputField;
    public GameObject PopUp;
    public GameObject PopUpError;
    public int charLimit = 6;
    public bool isJoiningRoom = false;

    StressConnectPanel stressConnectPanel;

    private void OnEnable()
    {
        inputField.text = "";
        PopUp.SetActive(false);
        PopUpError.SetActive(false);
        EventManager.OnStressDataReceived += ThrowSuccessPopUp;
    }

    private void OnDisable()
    {
        EventManager.OnStressDataReceived -= ThrowSuccessPopUp;
    }

    private void Awake()
    {
        stressConnectPanel = GetComponent<StressConnectPanel>();
    }

    private void ThrowSuccessPopUp(StressData arg0)
    {
        if (PopUp != null && PopUp.transform.parent.gameObject.activeSelf && !isJoiningRoom)
        {
            PopUp.SetActive(true);
        }
    }

    public string GetInput(){
        return inputField.text;
    }

    public void BoardKey(string key)
    {
        if (inputField.text.Length < charLimit) 
        {
            inputField.text += key;
            LayoutRebuilder.ForceRebuildLayoutImmediate(inputField.transform.parent as RectTransform);
        }
        else
        {
            return;
        }
    }

    public void BackSpace()
    {
        if (inputField.text.Length == 0)
        {
            return;
        }
        inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
    }
    
}
