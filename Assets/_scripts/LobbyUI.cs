using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;
//using Steamworks;
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
    public CharacterSelectUI offlineCharacterSelect;
    public ShowLevelTitle tempHolderLA;
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
    
    public void CancelMatchmaking()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void InviteRecieved(string id, string roomId)
    {
       // CSteamID Id = new CSteamID();

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

    IEnumerator checkInternetConnection(Action<bool> action)
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }
    public void CheckConnection()
    {
        // Start matchmaking counter
        


        StartCoroutine(checkInternetConnection((isConnected) => {
            // handle connection status here
            //Debug.Log("Connected to internet: " + isConnected);
            StartMatchmaking(isConnected);
        }));
    }

    public void StartMatchmaking(bool isConnectedToInternet)
    {
        if (!isConnectedToInternet)
        {
            MainMenuUIManager.Instance.touchMenuUI.localMatchingCancelled = false;
            LobbyConnectionHandler.instance.IsMultiplayerMode = false;
            MainMenuUIManager.Instance.touchMenuUI.EnableCancelMatchmakingButton();
            StartCoroutine(StartLocalStage_NoInernetConnection());
        }
        else
        {
            LobbyConnectionHandler.instance.IsMultiplayerMode = true;
            MainMenuUIManager.Instance.SwitchToCharacterSelect_WithoutStick();
            LobbyConnectionHandler.instance.StartMatchMaking();
        }
    }

    IEnumerator StartLocalStage_NoInernetConnection()
    {
        yield return new WaitForSeconds(3f);
        if (!MainMenuUIManager.Instance.touchMenuUI.localMatchingCancelled)
        {
            MainMenuUIManager.Instance.OfflineButtonListener(0);
            MainMenuUIManager.Instance.SwitchToCharacterSelect_WithoutStick();
            offlineCharacterSelect.SetCharacter();
            tempHolderLA.EnforceThisStageSettings();
            MainMenuUIManager.Instance.LoadScene_LoadingRoon();
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
