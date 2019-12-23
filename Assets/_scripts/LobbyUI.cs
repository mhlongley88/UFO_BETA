using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class LobbyUI : MonoBehaviour
{
    public static LobbyUI instance;

    public GameObject CharacterSelectMul;

    public GameObject AuthPanel;
    public bool isPublicMatch, isPrivateMatch;
    //Sign In
    public Text userEmailTextSI;
    public Text userPasswordTextSI;
    
    //Sign Up
    public Text userEmailTextSU;
    public Text userPasswordTextSU;
    public Text usernameTextSU;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if(PhotonNetwork.CurrentRoom != null)
            PhotonNetwork.LeaveRoom();
        isPublicMatch = isPrivateMatch = false;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MatchMaking(bool _isPrivateMatch)
    {
        isPrivateMatch = _isPrivateMatch ? true : false;
        isPublicMatch = !_isPrivateMatch ? true : false;
        if (!_isPrivateMatch)
        {
            LobbyConnectionHandler.instance.StartMatchMaking();
        }
        else
        {
            PrivateMatch();
        }
    }

    public void PrivateMatch()
    {
        string[] temp = new string[1];
        temp[0] = "LevelNumber";

        Hashtable hash = new Hashtable();
        hash.Add("LevelNumber", 1);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.PublishUserId = true;
        roomOptions.CustomRoomPropertiesForLobby = temp;
        roomOptions.CustomRoomProperties = hash;
        
        roomOptions.IsOpen = false;
        
        PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.UserId, roomOptions);
    }


    public void EnterMultiplayerMode()
    {
        LobbyConnectionHandler.instance.IsMultiplayerMode = true;
    }

    public void LeaveMultiplayerMode()
    {
        LobbyConnectionHandler.instance.IsMultiplayerMode = false;
    }

    

}
