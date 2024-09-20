using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SenarioSettingsData
{
    public Role Role = Role.IT;
    public int Senario = 1;
    public bool isGuided = false;

    public bool isExam = false;
    public bool networked = false;
}

public class MainMenuUI : MonoBehaviour
{
    public float fadeTime = 1f;

    public TMP_Text generatedRoomPin;

    public TMP_InputField pinpanelRoomPin;

    // windows
    [Header("UI panels")]
    public GameObject mainMenu;
    public GameObject networkingMode;   // 1
    public GameObject multiPlayer;      // 1.b
    public GameObject host;             // 1.b.1
    public GameObject join;             // 1.b.2
    public GameObject multiPlayerSettingsPanel;         // 2
    public GameObject singlePlayer;

    [Header("Scenario grab gameobjects")]
    public GameObject Scenario1Grab;
    public GameObject Scenario2Grab;

    [Header("Settings toggles")]
    public List<Toggle> settingsToggles = new List<Toggle>();
    public Toggle roleIt;
    public Toggle roleMech;
    public Toggle scenario1;
    public Toggle scenario2;
    public Toggle guided;
    public Toggle noGuided;
    public Toggle exam;
    public Toggle practice;

    CanvasGroup canvasGroup;
    Canvas canvas;
    SenarioSettingsData settings = new SenarioSettingsData();

    string setNetworkMode = "single";   // default value
    string setMultiMode = "host";       // default value

    public GameObject PopUpSuccess;
    public GameObject PopUpError;

    public void StartUI()
    {
        networkingMode.SetActive(true);
        multiPlayer.SetActive(false);
        host.SetActive(false);
        join.SetActive(false);
        multiPlayerSettingsPanel.SetActive(false);
        singlePlayer.SetActive(false);
        PopUpSuccess.SetActive(false);
        PopUpError.SetActive(false);
    }

    private void Start()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        canvas = GetComponentInChildren<Canvas>();
        canvasGroup.alpha = 0;
        ShowUI();
        StartUI();
    }

    public void OnEnable()
    {
        ZNetworkManager.Instance.OnOtherPlayerConnected += OnOtherJoinedRoom;
    }

    public void OnDisable()
    {
        ZNetworkManager.Instance.OnOtherPlayerConnected -= OnOtherJoinedRoom;
    }

    public void InitValues()
    {

    }

    public void OnOtherJoinedRoom()
    {
        RoleSelector.Instance.SetMasterRole(this.settings.Role);
        SendPanelUpdate();
    }

    [PunRPC]
    public void RefreshSenarioPanel(int role, int senario, bool isGuided, bool isExam)
    {
        this.settings.Role = (Role)role;
        this.settings.Senario = senario;
        this.settings.isGuided = isGuided;
        this.settings.isExam = isExam;

        UpdatePanel();
    }

    void SendPanelUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView photonView = PhotonView.Get(this);
            if (photonView == null)
            {
                Debug.LogError("No PhotonView found");

                return;
            }
            photonView?.RPC("RefreshSenarioPanel", RpcTarget.Others, (int)settings.Role, settings.Senario, settings.isGuided, settings.isExam);
            return;
        }
    }

    void UpdatePanel()
    {
        if (settings.Role == Role.IT)
        {
            roleIt.interactable = true;
            roleIt.isOn = true;
            roleIt.interactable = false;
        }
        else
        {
            roleMech.interactable = true;
            roleMech.isOn = true;
            roleMech.interactable = false;
        }


        if (settings.Senario == 1)
        {
            scenario1.interactable = true;
            scenario1.isOn = true;
            scenario1.interactable = false;
        }
        else
        {
            scenario2.interactable = true;
            scenario2.isOn = true;
            scenario2.interactable = false;
        }

        if (settings.isGuided)
        {
            guided.interactable = true;
            guided.isOn = true;
            guided.interactable = false;
        }
        else
        {
            noGuided.interactable = true;
            noGuided.isOn = true;
            noGuided.interactable = false;
        }
    }

    public void ShowUI()
    {
        canvas.enabled = true;
        canvasGroup.LeanAlpha(1, fadeTime);
    }

    // Methods for toggle selection change
    public void SetNetworkingMode(string mode)
    {
        setNetworkMode = mode;
    }

    public void SetMultiMode(string mode)
    {
        setMultiMode = mode;
    }

    public void SelectedMultiPlayer()
    {
        settings.networked = true;
    }

    public void SelectedSinglePlayer()
    {
        settings.networked = false;
    }

    public void SelectSenario(int senario)
    {
        settings.Senario = senario;
        SendPanelUpdate();
    }

    public void SetRole(Role role)
    {
        settings.Role = role;
        RoleSelector.Instance.SetMasterRole(this.settings.Role);
        SendPanelUpdate();
    }

    public void SetGuideance(bool isGuided)
    {
        settings.isGuided = isGuided;
        NetworkDataHolder.Instance?.AddOrUpdateData("Guided", (bool)settings.isGuided);
        SendPanelUpdate();
    }

    public void SetExamType(bool isExam)
    {
        settings.isExam = isExam;
        SendPanelUpdate();
    }

    public void SetRoleElectrician()
    {
        settings.Role = Role.Electrician;
        if (PhotonNetwork.IsMasterClient)
        {
            RoleSelector.Instance.SetMasterRole(this.settings.Role);
        }

        SendPanelUpdate();
    }

    public void SetRoleIT()
    {
        settings.Role = Role.IT;
        if (PhotonNetwork.IsMasterClient)
        {
            RoleSelector.Instance.SetMasterRole(this.settings.Role);
        }
        SendPanelUpdate();
    }

    // Methods for continue button clicks
    public void CloseNetworkingModeWindow()
    {
        switch (setNetworkMode)
        {
            case "single":
                networkingMode.SetActive(false);
                singlePlayer.SetActive(true);

                break;
            case "multi":
                networkingMode.SetActive(false);
                multiPlayer.SetActive(true);

                break;
        }
    }

    public void CloseMultiplayerModeWindow()
    {
        switch (setMultiMode)
        {
            case "host":
                CreateRoom();
                multiPlayer.SetActive(false);
                host.SetActive(true);
                break;
            case "join":
                mainMenu.SetActive(false);
                multiPlayer.SetActive(false);
                join.SetActive(true);
                break;
        }
    }

    public void CloseHostWindow()
    {
        host.SetActive(false);
        multiPlayer.SetActive(false);
        multiPlayerSettingsPanel.SetActive(true);

        settings.Role = RoleSelector.Instance.playerRole;
        if (settings.Role == Role.IT)
        {
            roleMech.isOn = true;
        }
        else
        {
            roleIt.isOn = true;
        }
        RoleSelector.Instance.SetMasterRole(this.settings.Role);
    }

    public void CloseJoinWindow()
    {
        join.SetActive(false);
        mainMenu.SetActive(true);
        //multiPlayer.SetActive(true);
        multiPlayerSettingsPanel.SetActive(true);
    }

    public void CloseSettingsWindow()
    {
        UpdateNetworkingSettings();
    }

    // Networking methods
    public void CreateRoom()
    {
        Action<string> onRoomCreated = (string roomid) =>
        {
            Debug.Log("Room created");
            generatedRoomPin.text = roomid;
            settings.networked = true;
        };
        Action<string> onRoomCreateFailed = (string message) =>
        {
            Debug.Log("Room creation failed: " + message);
        };
        ZNetworkManager.Instance.CreateRoom(onRoomCreated, onRoomCreateFailed);
    }

    public void JoinRoom()
    {
        Action<string> onRoomJoined = (string roomid) =>
        {
            settings.networked = true;
            CloseJoinWindow();
            DisableSettingsToggles();
            PopUpSuccess.SetActive(true);
            PopUpError.SetActive(false);
        };
        Action<string> onRoomJoinFailed = (string message) =>
        {
            Debug.Log("Join failed: " + message);
            PopUpSuccess.SetActive(false);
            PopUpError.SetActive(true);
        };
        ZNetworkManager.Instance.JoinRoom(pinpanelRoomPin.text, onRoomJoined, onRoomJoinFailed);
    }

    void UpdateNetworkingSettings()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView photonView = PhotonView.Get(this);
            if (photonView == null)
            {
                Debug.LogError("No PhotonView found");
                return;
            }
            photonView?.RPC("RefreshSenarioPanel", RpcTarget.Others, (int)settings.Role, settings.Senario, settings.isGuided, settings.isExam);

        }
        // itt kell a gombok megjelent�tett �llapot�t �ll�tani
        if (settings.Senario == 1)
        {
            Scenario1Grab.SetActive(true);
            Scenario2Grab.SetActive(false);
        }
        else
        {
            Scenario1Grab.SetActive(false);
            Scenario2Grab.SetActive(true);
        }
    }

    void DisableSettingsToggles()
    {
        foreach (Toggle toggle in settingsToggles)
        {
            toggle.interactable = false;
        }
    }
}