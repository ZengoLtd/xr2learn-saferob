using UnityEngine;
using UnityEngine.Events;

public static class EventManager 
{
    #region Actions

    public static event UnityAction OnTestAction;
    public static event UnityAction<Vector3,Quaternion,bool,GameObject> OnPlayerTeleport;
    public static event UnityAction OnPlayerTeleportBeginning;
    public static event UnityAction OnPlayerTeleportEnd;
    
    public static event UnityAction OnTeleporterAimStart;
    public static event UnityAction OnTeleporterAimEnd;
    public static event UnityAction<LookAtInteractable> OnRegisterLookAtInteractable;
    public static event UnityAction<LookAtInteractable> OnUnregisterLookAtInteractable;
   

    public static event UnityAction OnRopeConnected;
    public static event UnityAction OnRopeDisconnected;

    public static event UnityAction OnTeleportBlocked;
    public static event UnityAction OnTeleportUnblocked;

    public static event UnityAction OnGrabTeleportBlocked;
    public static event UnityAction OnGrabTeleportUnblocked;
    public static event UnityAction OnGrabTeleportHide;
    public static event UnityAction OnGrabTeleportUnHide;
    
    public static event UnityAction OnTeleporterAreaEntered;
    public static event UnityAction OnUINextButtonPressed;
    
    public static event UnityAction OnSceneStart;

    public static event UnityAction OnInventroyEquip;       // inventory
    public static event UnityAction OnInventoryUnequip;     // inventory

    public static event UnityAction OnPlayerFallingTooFast;   
    public static event UnityAction<float> OnPlayerFalling;
    public static event UnityAction OnPlayerFallEnded;

    public static event UnityAction OnBadItemUsed;
    public static event UnityAction<StressData> OnStressDataReceived;

    public static event UnityAction OnBadTeleport;
    public static event UnityAction OnBeforeSceneChange;

    #endregion



    #region Events

    public static void BeforeSceneChange() => OnBeforeSceneChange?.Invoke();
    public static void BadTeleport() => OnBadTeleport?.Invoke();
    public static void StressDataReceived(StressData data) => OnStressDataReceived?.Invoke(data);

    public static void RopeConnected() => OnRopeConnected?.Invoke();
    public static void RopeDisconnected() => OnRopeDisconnected?.Invoke();
    public static void TestAction() => OnTestAction?.Invoke(); 

    public static void PlayerTeleportBeginning() => OnPlayerTeleportBeginning?.Invoke();
    public static void PlayerTeleportEnd() => OnPlayerTeleportEnd?.Invoke();
    public static void PlayerTeleport(Vector3 position, Quaternion rotation, bool keepRotation,GameObject destinationobject = null) {
  

        OnPlayerTeleportBeginning?.Invoke();
        OnPlayerTeleport?.Invoke(position,rotation,keepRotation,destinationobject);
        
    } 
    public static void PlayerFalling(float movement) => OnPlayerFalling?.Invoke(movement);
    public static void TeleportBlock() => OnTeleportBlocked?.Invoke();
    public static void TeleportUnblock() => OnTeleportUnblocked?.Invoke();

    public static void GrabTeleportBlock() => OnGrabTeleportBlocked?.Invoke();
    public static void GrabTeleportUnblock() => OnGrabTeleportUnblocked?.Invoke();

    public static void GrabTeleportHide() => OnGrabTeleportHide?.Invoke();
    public static void GrabTeleportUnHide() => OnGrabTeleportUnHide?.Invoke();

    public static void SceneStart() => OnSceneStart?.Invoke();
    public static void TeleporterAimStart() =>OnTeleporterAimStart?.Invoke();
    
    public static void TeleporterAimEnd() => OnTeleporterAimEnd?.Invoke();

    public static void RegisterLookAtInteractable(LookAtInteractable interactable) => OnRegisterLookAtInteractable?.Invoke(interactable);
    public static void UnregisterLookAtInteractable(LookAtInteractable interactable) => OnUnregisterLookAtInteractable?.Invoke(interactable);
  
    public static void UINextButtonPressed() => OnUINextButtonPressed?.Invoke();


    public static void Equip() => OnInventroyEquip?.Invoke();
    public static void Unequip() => OnInventoryUnequip?.Invoke();


    public static void PlayerFallingTooFastEvent() => OnPlayerFallingTooFast?.Invoke();

    public static void TeleporterAreaEntered() => OnTeleporterAreaEntered?.Invoke();

    public static void PlayerFallEnded () => OnPlayerFallEnded?.Invoke();

    public static void BadItemUsed () => OnBadItemUsed?.Invoke();


    #endregion
}
