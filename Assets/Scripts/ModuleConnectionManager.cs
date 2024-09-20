using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleConnectionManager : MonoBehaviour
{
    public GameObject Pinpanel;
    public GameObject Pinpanel_already;
    public GameObject Pinpanel_error;
    public GameObject Pinpanel_success;
    public Button StartModuleButton;

    public GameObject NoActiveModulePanel;

    public GameObject ModuleSelectorUI;
    

    bool? isDeviceRegistered = null;

    public void StartModule(){
        StartCoroutine(CommunicationManager.Instance.StartModule(StartModuleSuccessCallback,StartModuleErrorCallback));
        Pinpanel.SetActive (false);
        NoActiveModulePanel.SetActive(false);
    }

    void StartModuleSuccessCallback(){
        //loadmodule
        SceneLoadManager.Instance.LoadScene("Modules/Module01/Scenes/Module01_Scene");
    }

    void StartModuleErrorCallback(){
        NoActiveModulePanel.SetActive(true);
    }


    void Start(){
        //check if device registeredd
        StartCoroutine(CommunicationManager.Instance.CheckDevice(DeviceRegisteredCallback,DeviceNotRegisteredCallback));
        StartModuleButton.interactable = isDeviceRegistered.HasValue && isDeviceRegistered.Value;
    }

    public void PairDevice(){
        string pin =  Pinpanel.GetComponent<Keyboard>().GetInput();
        StartCoroutine(CommunicationManager.Instance.RegisterDevice(pin,DeviceRegisterCallback,DeviceRegisterFailCallback));
    }
   
    void DeviceRegisteredCallback(){
        isDeviceRegistered = true;
         StartModuleButton.interactable = isDeviceRegistered.HasValue && isDeviceRegistered.Value;
    }

    void DeviceNotRegisteredCallback(){
        isDeviceRegistered = false;
    }

    void DeviceRegisterCallback(){
        Pinpanel_success.SetActive(true);
        Pinpanel_already.SetActive(false);
        Pinpanel_error.SetActive(false);
        isDeviceRegistered = true;
        StartModuleButton.interactable = isDeviceRegistered.HasValue && isDeviceRegistered.Value;
    }

    void DeviceRegisterFailCallback(){
        Pinpanel_error.SetActive(true);
        Pinpanel_success.SetActive(false);
        Pinpanel_already.SetActive(false);
    }

    public void TogglePinPanel()
    {
        Pinpanel.SetActive(!Pinpanel.activeSelf);
        ModuleSelectorUI.SetActive(!Pinpanel.activeSelf);
        if(isDeviceRegistered == true){
            Pinpanel_already.SetActive(true);
        }
        else{
            Pinpanel_already.SetActive(false);
        }
    }

   

}
