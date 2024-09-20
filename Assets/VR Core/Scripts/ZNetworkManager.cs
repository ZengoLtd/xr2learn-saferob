#if PUN_2_OR_NEWER
using Photon.Pun;
using Photon.Realtime;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BNG;
using System;
using UnityEngine.Events;


public class ZNetworkManager : MonoBehaviourPunCallbacks
{

    public GameObject ReconnectingHint;
    public GameObject OtherPlayerDisconnectedHint;
    /// <summary>
    /// Maximum number of players per room. If the room is full, a new radom one will be created.
    /// </summary>
    [Tooltip("Maximum number of players per room. If the room is full, a new random one will be created. 0 = No Max.")]
    [SerializeField]
    private byte maxPlayersPerRoom = 0;

    [Tooltip("If true, the JoinRoomName will try to be Joined On Start. If false, need to call JoinRoom yourself.")]
    public bool JoinRoomOnStart = true;

    [Tooltip("If true, do not destroy this object when moving to another scene")]
    public bool dontDestroyOnLoad = true;

    public string JoinRoomName = "RandomRoom";

    [Tooltip("Game Version can be used to separate rooms.")]
    public string GameVersion = "1";

    [Tooltip("Name of the Player object to spawn. Must be in a /Resources folder.")]
    public string RemotePlayerObjectName = "RemotePlayer";

    [Tooltip("Optional GUI Text element to output debug information.")]
    public Text DebugText;
    public static string LobbyName = "Saferob";
    private TypedLobby customLobby = new TypedLobby(LobbyName, LobbyType.Default);

    ScreenFader sf;
    public static ZNetworkManager Instance;

    public UnityAction OnOtherPlayerConnected;

    List<RoomInfo> roomList = new List<RoomInfo>();

    int GenerateRoomID()
    {
        if(!PhotonNetwork.IsConnectedAndReady){
            throw new Exception("Not connected to Photon Server");
        }
        while (true)
        {
            int randomID = GenerateRandomRoomID();
            if (!roomList.Exists(item => item.Name == randomID.ToString()))
            {
                return randomID;
            }
        }

    }
    int GenerateRandomRoomID()
    {
        return UnityEngine.Random.Range(1000, 9999);
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        // Required if you want to call PhotonNetwork.LoadLevel() 
        PhotonNetwork.AutomaticallySyncScene = true;
        
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);
        }

        if (Camera.main != null)
        {
            sf = Camera.main.GetComponentInChildren<ScreenFader>(true);
        }
        StartCoroutine(ReconnectLoop());
    }

    
    public void CreateRoom(Action<string> onRoomCreated = null, Action<string> onRoomCreateFailed = null)
    {
        
        try{
            int roomid = GenerateRoomID();
            bool roomcreated =  PhotonNetwork.CreateRoom(roomid.ToString(), new RoomOptions { MaxPlayers = maxPlayersPerRoom }, TypedLobby.Default);
            if(!roomcreated){
                throw new Exception("Room creation failed");
            }
            if(onRoomCreated != null){
                onRoomCreated(roomid.ToString());
            }           
        }catch(Exception e){
            Debug.Log(e.Message);
            if(onRoomCreateFailed != null){
                onRoomCreateFailed(e.Message);
            }
        }
        
    }
    public void ConnectToMaster(){
        if(PhotonNetwork.IsConnected){
           return;
        }
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = GameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    IEnumerator ReconnectLoop(){
        while(true){
            if(!PhotonNetwork.IsConnected){
                ConnectToMaster();
            }
            yield return new WaitForSeconds(5);
        }
    }
    void Start()
    {
        // Connect to Random Room if Connected to Photon Server
        if (PhotonNetwork.IsConnected)
        {
            if (JoinRoomOnStart)
            {
                LogText("Joining Room : " + JoinRoomName);
                PhotonNetwork.JoinRoom(JoinRoomName);
            }
        }
        // Otherwise establish a new connection. We can then connect via OnConnectedToMaster
        else
        {
           ConnectToMaster();
        }


    }
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            Debug.Log(info.Name);


        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
        UpdateCachedRoomList(roomList);
    }
    public void LoadScene(string moduleName){
        SceneLoadManager.Instance.LoadScene(moduleName);
        StartCoroutine(SceneLoad());
        if(PhotonNetwork.IsMasterClient){
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("LoadSceneRPC", RpcTarget.Others);
        }
    }
    [PunRPC]
    public void LoadSceneRPC(){
        StartCoroutine(SceneLoad());
    }

    IEnumerator SceneLoad(){
       
        yield return new WaitForEndOfFrame();
        while(PhotonNetwork.LevelLoadingProgress < 1){
            yield return null;
        }
        CommunicationManager.Instance?.Log("Modul elindult","start");
        EventManager.SceneStart();
    }
    void Update()
    {   
        
        // Show Loading Progress
        if (PhotonNetwork.LevelLoadingProgress > 0 && PhotonNetwork.LevelLoadingProgress < 1)
        {
            Debug.Log(PhotonNetwork.LevelLoadingProgress);
        }else if(PhotonNetwork.LevelLoadingProgress >= 1){
   
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        LogText("OnJoinRoomFailed Failed, Error : " + message);
        //JoinRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed Failed, Error : " + message);
    }

    public override void OnConnectedToMaster()
    {

        LogText("Connected to Master Server. \n");

        if (JoinRoomOnStart)
        {
            LogText("Joining Room : <color=aqua>" + JoinRoomName + "</color>");
            PhotonNetwork.JoinRoom(JoinRoomName);
        }
        PhotonNetwork.JoinLobby(customLobby);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        float playerCount = PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null ? PhotonNetwork.CurrentRoom.PlayerCount : 0;

        LogText("Connected players : " + playerCount);
        OtherPlayerDisconnectedHint.SetActive(false);
        OnOtherPlayerConnected?.Invoke();
    }

    public override void OnJoinedRoom()
    {

        LogText("Joined Room. Creating Remote Player Representation.");

        // Network Instantiate the object used to represent our player. This will have a View on it and represent the player         
        GameObject player = PhotonNetwork.Instantiate(RemotePlayerObjectName, new Vector3(0f, -100f, 0f), Quaternion.identity, 0);
        BNG.NetworkPlayer np = player.GetComponent<BNG.NetworkPlayer>();
        if (np)
        {
            np.transform.name = "MyRemotePlayer";
            np.AssignPlayerObjects();
        }
        player.transform.parent = transform;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if(cause == DisconnectCause.DisconnectByClientLogic || cause == DisconnectCause.DisconnectByServerLogic){
            return;
        }
        LogText("Disconnected from PUN due to cause : " + cause);
        ShowDisconnect();
        if (!PhotonNetwork.ReconnectAndRejoin())
        {
            LogText("Reconnect and Joined.");
            ShowReconnect();
            JoinRoom();
        }

        base.OnDisconnected(cause);
    }


    public void ShowDisconnect(){
        ReconnectingHint.SetActive(true);
        ReconnectingHint.GetComponent<HintUI>().ShowHint();
    }

    public void ShowReconnect(){
        ReconnectingHint.SetActive(false);
    }


    public string lastRoomID = "";
    public void JoinRoom(string roomid = "",Action<string> onRoomJoined = null, Action<string> onRoomJoinFailed = null)
    {   
        Debug.Log("Joining Room"+roomid);
        if(roomid == "" && lastRoomID != ""){
            roomid = lastRoomID;
        }
        if(roomid == ""){
            LogText("Room ID is empty");
            onRoomJoinFailed?.Invoke("Room ID is empty");
            return;
        }
        if(roomid.Length != 4){
            LogText("Room ID must be 4 digits");
            onRoomJoinFailed?.Invoke("Room ID must be 4 digits");
            return;
        }
        PhotonNetwork.JoinRoom(roomid);
        lastRoomID = roomid;
        onRoomJoined?.Invoke(roomid);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // should get the reason of disconnect


       // OtherPlayerDisconnectedHint.SetActive(true);
       // OtherPlayerDisconnectedHint.GetComponent<HintUI>().ShowHint();
    }
    void LogText(string message)
    {

        // Output to worldspace to help with debugging.
        if (DebugText)
        {
            DebugText.text += "\n" + message;
        }

        Debug.Log(message);
    }
}
