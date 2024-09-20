using UnityEngine;
using Zengo.Inventory;

public class RopeConnector : BlockLogicBase
{

    [HideInInspector]
    public BlockEvent OnRopeConnected;
    [HideInInspector]
    public BlockEvent OnRopeDisconnected;
    [HideInInspector]
    public BlockEvent OnRopeChanged;
    
    public BlockState ropeConnected;

    public GameObject connectionPoint;
    GameObject rope;
    
    public bool allowTeleport = false;
    public bool allowGrabTeleport = true;
    public bool hideEmeloController = false;

    public GameObject harnessRopePrefab;

    RoofCart roofCart;
    GameObject harnessRope;

    private void Start()
    {
        roofCart = GetComponent<RoofCart>();
        ropeConnected.state = false;
    }

    public void ToggleRope()
    {
        Debug.Log("ToggleRope");
        if (InventoryManager.Instance.HasItem(ItemType.Ropes))
        {
            if ((!(bool)ropeConnected.state) && RopeManagerBlock.Instance.FreeRopesAmount() > 0)
            {
                ConnectRope();
                if (roofCart != null)
                {
                    roofCart.MoveCartTurnOn();
                }

            }
            else
            {
                DisconnectRope();
                if (roofCart != null)
                {
                    roofCart.MoveCartTurnOff();
                }
            }
        }
        OnTriggered.Invoke(null);
    }

    public void ToggleHarnessRope()
    {
        Debug.Log("ToggleHarnessRope");
        if (InventoryManager.Instance.HasItem(ItemType.Harness))
        {
            if (! (bool)ropeConnected.state)
            {
                harnessRope = Instantiate(harnessRopePrefab);

                 ropeConnected.state = true;
               
                if (harnessRopePrefab != null)
                {
                    harnessRope.transform.parent = connectionPoint.transform;
                    harnessRope.transform.position = connectionPoint.transform.position;
                }
                RopeManagerBlock.Instance.AddHarness(harnessRope);
                if (!allowTeleport)
                {
                    //Debug.Log("Blocking teleports!");
                    EventManager.TeleportBlock();
                }
                if (!allowGrabTeleport)
                {
                    //Debug.Log("Blocking grab teleports!");
                    EventManager.GrabTeleportBlock();
                }
                if (!hideEmeloController)
                {
                    EventManager.GrabTeleportHide();
                }
                if (roofCart != null)
                {
                    roofCart.MoveCartTurnOn();
                }
                EventManager.RopeConnected();
            }
            else
            {
               
                ropeConnected.state = false;
                RopeManagerBlock.Instance.RemoveHarness(harnessRope);
                Destroy(harnessRope.gameObject);
                if (!allowTeleport)
                {
                    EventManager.TeleportUnblock();
                    //Debug.Log("Unblocking teleports!");
                }
                if (!allowGrabTeleport)
                {
                    EventManager.GrabTeleportUnblock();
                }
                if (!hideEmeloController)
                {
                    EventManager.GrabTeleportUnHide();
                }
                if (roofCart != null)
                {
                    roofCart.MoveCartTurnOff();
                }
                EventManager.RopeDisconnected();
            }
        }
        OnTriggered.Invoke(null);
    }

    void ConnectRope()
    {
        Debug.Log("Rope connected");
        ropeConnected.state = true;
        rope = RopeManagerBlock.Instance.GetRope();
        if (rope == null)
        {
            return;
        }
        rope.transform.parent = connectionPoint.transform;
        rope.transform.position = connectionPoint.transform.position;
        if (!allowTeleport)
        {
            //Debug.Log("Blocking teleports!");
            EventManager.TeleportBlock();
        }
        if (!allowGrabTeleport)
        {
            //Debug.Log("Blocking grab teleports!");
            EventManager.GrabTeleportBlock();
        }
        if (!hideEmeloController)
        {
            //Debug.Log("Blocking grab teleports!");
            EventManager.GrabTeleportHide();
        }
        EventManager.RopeConnected();
        OnRopeConnected.Invoke(null);
        OnRopeChanged.Invoke(null);
        OnTriggered.Invoke(null);
    }
    void DisconnectRope()
    {
        Debug.Log("Rope disconnected");
        ropeConnected.state = false;
        RopeManagerBlock.Instance.FreeUpRope(rope);
        if (!allowTeleport)
        {
            EventManager.TeleportUnblock();
            //Debug.Log("Unblocking teleports!");
        }
        if (!allowGrabTeleport)
        {
            EventManager.GrabTeleportUnblock();
        }
        if (!hideEmeloController)
        {
            EventManager.GrabTeleportUnHide();
        }
        EventManager.RopeDisconnected();
        OnRopeDisconnected.Invoke(null);
         OnTriggered.Invoke(null);
    }

   

}
