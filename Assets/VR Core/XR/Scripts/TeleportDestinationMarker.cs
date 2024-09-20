using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BNG;
using UnityEngine.SceneManagement;




[SelectionBase]
public class TeleportDestinationMarker : BNG.TeleportDestination
{
    public UnityEvent OnBeforePlayerTeleported;

    public List<BNG.TeleportDestination> Destinations = new List<BNG.TeleportDestination>();
    public bool hideUIConst = true;
    GameObject UI;
    GameObject Marker;
    bool lastTarget = false;
    bool hideui = true;

    bool isEnabled = true;



    int blockcounter = 0;
    void checkBlock()
    {
        if (blockcounter > 0)
        {
            //blocked
            DisableTeleporter();

        }
        else
        {
            //not blocked
            EnableTeleporter();
        }
    }

    void Block()
    {
        blockcounter++;
        checkBlock();
    }

    void Unblock()
    {
        blockcounter--;
        checkBlock();
    }



    public void DisableTeleporter()
    {

        if (GetComponent<Collider>())
        {
            GetComponent<Collider>().enabled = false;
        }
        UI?.SetActive(false);
        Marker?.SetActive(false);
        isEnabled = false;
    }

    public void EnableTeleporter()
    {

        if (GetComponent<Collider>())
        {
            GetComponent<Collider>().enabled = true;
        }
        UI?.SetActive(true);
        Marker?.SetActive(true);
        isEnabled = true;
    }


    public void HideMarker()
    {
        Marker?.SetActive(false);
        UI?.SetActive(true);
        hideui = false;
    }

    public virtual void SetAsLastTarget()
    {
        EnableTeleporter();
        lastTarget = true;
        CommunicationManager.Instance?.Log("Felhasználó teleportált  (" + gameObject.name + ").");
        HideUI();
    }

    public void SetAsLastTargetFromArea()
    {
        if (lastTarget)
        {
            return;
        }
        if (blockcounter > 0)
        {
            return;
        }
        EventManager.TeleporterAreaEntered();
        EnableTeleporter();
        foreach (Transform child in transform.parent)
        {
            child?.GetComponent<TeleportDestinationMarker>()?.ResetLastTarget();
        }
        lastTarget = true;
        CommunicationManager.Instance?.Log("Felhasználó odasétált  (" + gameObject.name + ").");
        HideUI();
        ShowDestinations();
        this.OnPlayerTeleported?.Invoke();
    }

    void Update()
    {
        if (!isEnabled)
        {
            return;
        }
        if (lastTarget)
        {
            if (!hideui)
            {
                UI?.SetActive(false);
                Marker?.SetActive(true);
            }
            else
            {
                Marker?.SetActive(true);
                UI?.SetActive(false);
            }
            return;
        }
        checkBlock();
        Marker?.SetActive(true);
        if (hideUIConst)
        {
            UI?.SetActive(false);
        }
#if PHOTON_UNITY_NETWORKING
        GameObject closestRemoteplayer = null;
        if(ZNetworkManager.Instance != null){
            foreach (MyRemotePlayer child in ZNetworkManager.Instance?.gameObject?.GetComponentsInChildren<MyRemotePlayer>())
        {
          
            float distance = Vector3.Distance(new Vector3(child.ik.transform.position.x, 0, child.ik.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));
            if (distance < 1.5f)
            {
                closestRemoteplayer = child.ik.gameObject;
            }
        }
        if (closestRemoteplayer != null)
        {
            if(transform.position == closestRemoteplayer.transform.position){
                transform.position += new Vector3(Random.Range(-0.001f, 0.001f),0,Random.Range(-0.001f, 0.001f));
            }
            //move DestinationTransform away from closestRemoteplayer
            Vector3 direction = (transform.position - closestRemoteplayer.transform.position).normalized;
            DestinationTransform.position = transform.position + direction * 0.3f;
            
        } 
        }
        
#endif

    }
    public virtual void ResetLastTarget()
    {
        lastTarget = false;
    }
    void OnEnable()
    {

        EventManager.OnTeleportBlocked += Block;
        EventManager.OnTeleportUnblocked += Unblock;
        OnPlayerTeleported.AddListener(ShowDestinations);
        OnPlayerTeleported.AddListener(SetAsLastTarget);
        OnBeforePlayerTeleported.AddListener(ShowDestinations);
        if (DestinationTransform == null)
        {
            DestinationTransform = transform.Find("Target");
        }
        EventManager.OnTeleporterAimStart += ShowUI;
        EventManager.OnTeleporterAimEnd += HideUI;
        EventManager.OnPlayerTeleportBeginning += ResetLastTarget;
        UI = transform.Find("UI")?.gameObject;
        Marker = transform.Find("Marker")?.gameObject;

        //if(UI == null){
        //    Debug.LogError("UI not found in teleporter" + DevelopmentUtilities.GetGameObjectPath(this.gameObject));
        //    this.enabled = false;
        //}
        //if(Marker == null){
        //    Debug.LogError("Marker not found in teleporter" + DevelopmentUtilities.GetGameObjectPath(this.gameObject));
        //    this.enabled = false;
        //}
        checkBlock();


        if (hideUIConst)
        {
            HideUI();
        }



    }
    public virtual void ShowDestinations()
    {

        Transform teleportparent;

        if (TeleportContainer.Instance != null && TeleportContainer.Instance?.transform != null)
        {
            teleportparent = TeleportContainer.Instance.transform;
        }
        else
        {
            teleportparent = transform.parent;
            if (transform.parent.GetComponent<TeleportBlock>() == null)
            {
                teleportparent = transform.parent.parent;
            }
        }

        var teleporterMarkers = teleportparent.GetComponentsInChildren<TeleportDestinationMarker>();

        foreach (TeleportDestinationMarker marker in teleporterMarkers)
        {
            marker.HideUI();
            marker.DisableTeleporter();
        }

        foreach (BNG.TeleportDestination destination in Destinations)
        {
            destination.GetComponent<TeleportDestinationMarker>().EnableTeleporter();
        }

    }
    void OnDisable()
    {
        ResetLastTarget();
        EventManager.OnTeleporterAimStart -= ShowUI;
        EventManager.OnTeleporterAimEnd -= HideUI;
        EventManager.OnPlayerTeleportBeginning -= ResetLastTarget;
        EventManager.OnTeleportBlocked -= Block;
        EventManager.OnTeleportUnblocked -= Unblock;
        OnBeforePlayerTeleported.RemoveListener(ShowDestinations);
        OnPlayerTeleported.RemoveListener(SetAsLastTarget);
        OnPlayerTeleported.RemoveListener(ShowDestinations);

    }

    void ShowUI()
    {
        if (lastTarget)
        {
            return;
        }


    }

    public void HideUI()
    {
        UI?.SetActive(false);
        hideui = true;
    }
}
