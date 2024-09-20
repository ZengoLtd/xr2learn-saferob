
using BNG;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnGrab : BlockLogicBase
{
    [HideInInspector]
    public BlockEvent OnGrabbed;
    public BlockEvent OnGrabbedFromNetwork;
    public BlockEvent OnGrabbedLocal;
   
    public InputActionReference[] ToggleActions;

    public GameObject handle;
    protected bool? isleft = null;
    protected bool isHover = false;
    public Texture2D normalTex;
    public Texture2D selectedTex;
    protected Vector3 startSize = Vector3.zero;
    protected HandleMode handleMode;
    private bool onlyonce;

    [PunRPC]
    public void NetworkGrabbed(){
        Debug.Log("NetworkGrabbed");
        OnGrabbed?.Invoke(null);
        OnGrabbedFromNetwork?.Invoke(null);
    }


    public void Grabbed()
    {
        OnGrabbed?.Invoke(null);
        OnGrabbedLocal?.Invoke(null);
        if(networked){
            PhotonView photonView = PhotonView.Get(this);
            if(photonView != null){
                photonView?.RPC("NetworkGrabbed", RpcTarget.Others);
            }
        }
    }
    
    



    public AudioSource actionSound;
    
   

    private void Start()
    {
        if (actionSound == null)
        {
            actionSound = GameObject.FindGameObjectWithTag("Mute")?.GetComponent<AudioSource>();
        }
        if(ToggleActions.Length == 0)
        {
            Debug.LogError("No ToggleActions set in " + DevelopmentUtilities.GetGameObjectPath(this.gameObject));
        }
        if(ZNetworkManager.Instance == null){
            networked = false;
        }
    }
   
    public void OnValidToggle()
    {
       
        if (HotspotHintSystem.Instance != null)
        {
            HotspotHintSystem.Instance.ClearHint();
        }
        Grabbed();
       
       
       
        handleMode?.CheckOptimalEquipmentUse();
        
        //has bad item
        actionSound?.Play();
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Grabber>())
        {
            var side = other.gameObject.GetComponent<Grabber>().HandSide;
            if (side == ControllerHand.Left)
            {
                isleft = true;
            }
            else
            {
                isleft = false;
            }
            isHover = true;
            onlyonce = true;
        }
    }

    protected new void OnEnable()
    {
        base.OnEnable();
        if (transform.childCount == 0)
        {
            Debug.LogError("No handle found in " + DevelopmentUtilities.GetGameObjectPath(this.gameObject));
            this.enabled = false;
            return;
        }
        handle = transform.GetChild(0).gameObject;
        foreach (var action in ToggleActions)
        {
            if (action)
            {
                action.action.Enable();
                action.action.performed += OnToggle;
            }
        }
        if (startSize == Vector3.zero)
        {
            startSize = transform.localScale;
        }
        handleMode = transform?.parent?.GetComponent<HandleMode>();
        if (handleMode == null)
        {
            handleMode = GetComponent<HandleMode>();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<BNG.Grabber>())
        {
            isHover = false;
            isleft = false;
        }
    }
    protected new void OnDisable()
    {
        base.OnDisable();
        foreach (var action in ToggleActions)
        {
            if (action)
            {
                //action.action.Disable();
                action.action.performed -= OnToggle;
            }
        }
        isHover = false;
        isleft = null;
    }
    protected virtual void HoverEffect()
    {
        if (HotspotHintSystem.Instance != null)
        {
            if (isHover)
            {
                HotspotHintSystem.Instance.currentGrab = this.gameObject;
                transform.localScale = Vector3.Lerp(transform.localScale, startSize * 1.5f, Time.deltaTime * 10);
                if (gameObject.GetComponent<HotspotHint>() != null)
                {
                    if (onlyonce)
                    {
                        gameObject.GetComponent<HotspotHint>().ShowHotspot();
                        onlyonce = false;
                    }

                }
            }
            else if (HotspotHintSystem.Instance.currentGrab != null)
            {
                if (HotspotHintSystem.Instance.currentGrab == gameObject && !isHover || !HotspotHintSystem.Instance.currentGrab.activeSelf)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * 10);
                    if (gameObject.GetComponent<HotspotHint>() != null)
                    {
                        gameObject.GetComponent<HotspotHint>().HideHotspot();
                    }
                }
            }
            else
            {
                HotspotHintSystem.Instance.currentGrab = null;
                transform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * 10);
            }
        }
        if (normalTex != null && selectedTex != null)
        {
            if (isHover)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, startSize * 1.5f, Time.deltaTime * 10);
                handle.GetComponent<Renderer>().material.mainTexture = selectedTex;
            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * 10);
                handle.GetComponent<Renderer>().material.mainTexture = normalTex;
            }
        }

    }
    void Update()
    {
        HoverEffect();

    }

    public void OnToggle(InputAction.CallbackContext context)
    {
        //Debug.Log("ishover" + isHover +  "context" + context.action.ToString().ToLower() + "isleft" + isleft);
        if (!isHover || isleft == null)
        {
            return;
        }

        if (context.action.ToString().ToLower().Contains("left") && isleft == true)
        {
            OnValidToggle();
        }

        if (context.action.ToString().ToLower().Contains("right") && isleft == false)
        {
            OnValidToggle(); 
        }
    }

}
