using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Steamworks;
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


    public Text InvitedFriendName;
    public GameObject InvitationPanel, FriendsListButton;

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
    
    public void InviteRecieved(string id, string roomId)
    {
        CSteamID Id = new CSteamID();

        InvitedFriendName.text = id;    //SteamFriends.GetFriendPersonaName(id);
        InvitationPanel.SetActive(true);
    }


    public void SendInviteHunzlah()
    {
        PhotonChatClient.instance.SendInvitation("76561199002318893", "By Pringo");
    }

    public void SendInvitePringo()
    {
        PhotonChatClient.instance.SendInvitation("76561198139240499", "By Hunzlah");
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
            LobbyConnectionHandler.instance.isPrivateMatch = true;
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
        
        roomOptions.IsVisible = false;
        
        PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.UserId, roomOptions);
    }


    public void EnterMultiplayerMode()
    {
        LobbyConnectionHandler.instance.IsMultiplayerMode = true;
        GameManager.Instance.isLocalSPMode = GameManager.Instance.IsLocalPvPMode = false;
    }

    public void LeaveMultiplayerMode()
    {
        LobbyConnectionHandler.instance.IsMultiplayerMode = false;
    }

    

}
