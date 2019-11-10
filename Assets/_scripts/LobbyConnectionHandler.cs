using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using PlayFab;
public class LobbyConnectionHandler : MonoBehaviourPunCallbacks, ILobbyCallbacks, IConnectionCallbacks, IMatchmakingCallbacks, IInRoomCallbacks
{
    public static LobbyConnectionHandler instance;
    public bool IsMultiplayerMode;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        Init();
    }

    void Init()
    {
        IsMultiplayerMode = false;
        PhotonNetwork.AutomaticallySyncScene = true;
        //PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 40), PhotonNetwork.NetworkClientState.ToString());
    }

    public void StartMatchMaking()
    {
        LobbyUI.instance.EnterMultiplayerMode();
        Hashtable hash = new Hashtable();
        hash.Add("LevelNumber", 1);

        PhotonNetwork.JoinRandomRoom(hash, (byte)0);
    }

    public void ConnectToPhoton(string userID)
    {
        //PhotonNetwork.AuthValues = new AuthenticationValues();
        //PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
        ////PhotonNetwork.AuthValues.AddAuthParameter("authenticated", "true");
        //PhotonNetwork.AuthValues.AddAuthParameter("token", userID);
        //PhotonNetwork.AuthValues.AddAuthParameter("token", PlayFabClientAPI.GetPhotonAuthenticationToken());
        //PhotonNetwork.ConnectUsingSettings(); Debug.Log("Connecting to photon");

    }

    void ILobbyCallbacks.OnJoinedLobby()
    {

    }

    public override void OnLeftRoom()
    {

    }

    void IInRoomCallbacks.OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
    }

    public override void OnCreatedRoom()
    {
    }

    public override void OnJoinedLobby()
    {
    }

    public override void OnLeftLobby()
    {
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
    }



    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        //if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        //{
        //    SceneManager.LoadScene("LoadingRoom");
        //}
    }

    void IInRoomCallbacks.OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            if(PhotonNetwork.IsMasterClient)
                SceneManager.LoadScene("LoadingRoom");
        }
    }

    void IInRoomCallbacks.OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string[] temp = new string[1];
        temp[0] = "LevelNumber";

        Hashtable hash = new Hashtable();
        hash.Add("LevelNumber", 1);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.PublishUserId = true;
        roomOptions.CustomRoomPropertiesForLobby = temp;
        roomOptions.CustomRoomProperties = hash;
        PhotonNetwork.CreateRoom(null, roomOptions);


    }

    public override void OnConnectedToMaster()
    {
        LobbyUI.instance.AuthPanel.SetActive(false);
    }

    void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {

    }

    void IInRoomCallbacks.OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {

    }

    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }

    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
    }


}
