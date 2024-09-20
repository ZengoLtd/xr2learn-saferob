using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkDataHolder : MonoBehaviour
{

    public static NetworkDataHolder Instance;

    void Awake(){
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(this);
        }
        StartCoroutine(Test());
        OnDataUpdate += TestCallback;
    }



    //callback for data updates
    public delegate void DataUpdateCallback(string userid, string name, object value);
    public DataUpdateCallback OnDataUpdate;

    public Dictionary<string, Dictionary<string ,object>> NetworkData = new Dictionary<string, Dictionary<string ,object>>();

    public void AddOrUpdateData(string name, object value){
        AddOrUpdateData(PhotonNetwork.LocalPlayer.UserId, name, value);
    }
    public object CheckValue(string name){
        if(NetworkData.ContainsKey(PhotonNetwork.LocalPlayer.UserId)){
            if(NetworkData[PhotonNetwork.LocalPlayer.UserId].ContainsKey(name)){
                 return NetworkData[PhotonNetwork.LocalPlayer.UserId][name];
            }
        }
        return null;
    }
    public void AddOrUpdateData(string userid, string name, object value,bool fromNetwork = false){
        if(userid == null){
            return;
        }
        if(NetworkData.ContainsKey(userid)){
            if(NetworkData[userid].ContainsKey(name)){
                NetworkData[userid][name] = value;
            }else{
                NetworkData[userid].Add(name, value);
            }
        }else{   
            NetworkData.Add(userid, new Dictionary<string, object>());
            NetworkData[userid].Add(name, value);
        }
        //message is not from network, send it to network
        if(!fromNetwork){
            if(PhotonNetwork.IsConnected && PhotonNetwork.InRoom){
                PhotonView photonView = PhotonView.Get(this);
                if(photonView != null){
                    photonView?.RPC("NetworkAddOrUpdateData", RpcTarget.Others, userid, name, value);
                }
            }
        }
        OnDataUpdate?.Invoke(userid, name, value);
    }

    [PunRPC]
    public void NetworkAddOrUpdateData(string userid, string name, object value){

       // Debug.Log("Network Data Update: " + userid + " " + name + " " + value);
        AddOrUpdateData(userid, name, value, true);
    }
    void TestCallback(string userid, string name, object value){
       // Debug.Log("Data Update: " + userid + " " + name + " " + value);
    }

    IEnumerator Test(){
        if(!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom ){
            yield return new WaitForSeconds(1);
        }
        int testvalue = 0;
        while(true){
            yield return new WaitForSeconds(2);
            string photonUserId = PhotonNetwork.LocalPlayer.UserId;
            AddOrUpdateData(photonUserId, "test", testvalue);
            testvalue++;
        }
       
    }


}
