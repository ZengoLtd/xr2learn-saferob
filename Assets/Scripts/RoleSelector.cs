using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum Role
{
   
    IT,
     Electrician
}

public class RoleSelector : MonoBehaviour
{
    public static RoleSelector Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public Role playerRole = Role.Electrician;

    [PunRPC]
    public void SetClientRole(int role){
        playerRole = (Role)role;
    }
    public void SetMasterRole(Role role){
        playerRole = role;
        SetOtherRole();
    }

    public void SetOtherRole(){
        if(!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom ){
            return;
        }
        if(!PhotonNetwork.IsMasterClient){
            return;
        }
        PhotonView photonView = PhotonView.Get(this);
        if(photonView != null){
            Role clientRole = Role.IT;
            if(playerRole == Role.Electrician){
                clientRole = Role.IT;
            }else if(playerRole == Role.IT){
                clientRole = Role.Electrician;
            }
            photonView?.RPC("SetClientRole", RpcTarget.Others, (int)clientRole);
        }
            
    }
      
}
