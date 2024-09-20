using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using RootMotion.FinalIK;


public class MyRemotePlayer : MonoBehaviour
{
    public VRIK ik;
    public string PlayerID;
    public GameObject HelmetTransform;
    public GameObject GlassesTransform;
    
    [PunRPC]
    public void SetNetworkID(string userid ){
        PlayerID = userid;
        transform.name = "RemotePlayer_" + userid;    
    }
    
    [PunRPC]
    public void GetNetworkIDsFromOthers(){
        PhotonView photonView = PhotonView.Get(this);
        photonView?.RPC("SetNetworkID", RpcTarget.Others, PhotonNetwork.LocalPlayer.UserId);
    }

    [PunRPC]
    public void OwnerTeleported(){
        StartCoroutine(LegFixerRoutine());
    }

    IEnumerator LegFixerRoutine(){
        //"kicsit" hack de jelenleg nincs jobb ötletem
        for(int i = 0;i<10;i++){
            for(int y = 0;y<30;y++){
                ik.solver.Update();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void Start()
    {
        PhotonView photonView = PhotonView.Get(this);
        transform.parent = ZNetworkManager.Instance.transform;
        if(photonView.IsMine){
            PlayerID = PhotonNetwork.LocalPlayer.UserId;
            transform.name = "LocalPlayer_" + PlayerID;
            if(PhotonNetwork.IsConnected && PhotonNetwork.InRoom){
                photonView?.RPC("SetNetworkID", RpcTarget.Others, PlayerID);
            }
        }else{
            photonView?.RPC("GetNetworkIDsFromOthers", RpcTarget.Others);
        }
    }
    void OnEnable(){
        EventManager.OnPlayerTeleportEnd += OnTeleporEnd;
    }
    void OnDisable(){
        EventManager.OnPlayerTeleportEnd -= OnTeleporEnd;
    }
    void OnTeleporEnd(){
       
        PhotonView photonView = PhotonView.Get(this);
        if(photonView.IsMine){
            photonView?.RPC("OwnerTeleported", RpcTarget.Others);

        }
    }
    

    void Awake(){
        NetworkDataHolder.Instance.OnDataUpdate += OnDataUpdate;
        
    }
    void OnDataUpdate(string userid, string name, object value){
        if(userid == PlayerID){
            switch(name){
                case "Helmet":
                    HelmetTransform.SetActive((bool)value);
                    break;
                case "Glasses":
                    GlassesTransform.SetActive((bool)value);
                    break;
            }
            
        }

    }
    




}
