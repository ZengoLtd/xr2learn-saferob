
using BNGExtension;
using UnityEngine;

public class TeleportBlock : BlockLogicBase
{

    public BlockEvent OnTeleported;

    void Teleported()
    {
        Debug.Log("TeleportBlock [" + transform.name + "] OnTeleported called");
        OnTeleported.Invoke(null);
    }
    void Awake()
    {
        transform.GetChild(0).GetComponent<BNG.TeleportDestination>().OnPlayerTeleported.AddListener(Teleported);
    }

    public override void Logic(object data)
    {
        Debug.Log("TeleportBlock [" + transform.name + "] Logic called");
        TeleportDestinationMarker TeleportMarker = transform.GetChild(0).GetComponent<TeleportDestinationMarker>();

        Debug.Log("TeleportMarker [" + TeleportMarker.transform.name + "]");
        //teleport player
        if (TeleportMarker == null)
        {
            Debug.LogError("No teleport marker set. " + DevelopmentUtilities.GetGameObjectPath(gameObject));
            return;
        }
        ScreenFader.Instance?.StartTransition();
        
          
        PlayerTeleport playerTeleport = FindObjectOfType<PlayerTeleport>();
        playerTeleport.DestinationObject = TeleportMarker;

        EventManager.PlayerTeleport(TeleportMarker.DestinationTransform.transform.position, TeleportMarker.DestinationTransform.transform.rotation, !TeleportMarker.ForcePlayerRotation, TeleportMarker.gameObject);
        base.Logic(data);
    }
}
