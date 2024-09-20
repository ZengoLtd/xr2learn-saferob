
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class NetworkedTriggerBlock : BlockLogicBase
{


    [PunRPC]
    public void NetworkLogic(object data){
        Debug.Log("NetworkedTriggerBlock Logic");
        OnTriggered?.Invoke(null);
    }


    public override void Logic(object data)
    {
        Debug.Log("NetworkedTriggerBlock Logic");
        PhotonView photonView = PhotonView.Get(this);
        if(photonView == null){
            Debug.LogError("No PhotonView found");
            return;
        }
        photonView?.RPC("NetworkLogic", RpcTarget.Others, data);
        OnTriggered.Invoke(null);
        

    }
    void OnDestroy(){
      
    }
}
