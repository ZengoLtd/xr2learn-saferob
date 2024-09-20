using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class AlertBoxMessages : MonoBehaviour
{
    public LocalizedString messageSuccess;
    public LocalizedString messageError;


    public void ShowSuccessMessage()
    {
        SetAlertBoxes.instance.ShowSuccessAlert(messageSuccess);
    }

    public void ShowErrorMessage()
    {
        SetAlertBoxes.instance.ShowErrorAlert(messageError);
    }
}
