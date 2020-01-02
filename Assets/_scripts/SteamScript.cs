using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamScript : MonoBehaviour
{
    // Start is called before the first frame update
    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;
    private CallResult<NumberOfCurrentPlayers_t> m_NumberOfCurrentPlayers;

    public static SteamScript instance;
    void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
            m_NumberOfCurrentPlayers = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
        }
    }

    void Start()
    {
        if (SteamManager.Initialized)
        {
            GetUserName();
            GetSteamUserID();
            GetUserFriends();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SteamAPICall_t handle = SteamUserStats.GetNumberOfCurrentPlayers();
        //    m_NumberOfCurrentPlayers.Set(handle);
        //    Debug.Log("Called GetNumberOfCurrentPlayers()");
        //}
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            Debug.Log("Steam Overlay has been activated");
        }
        else
        {
            Debug.Log("Steam Overlay has been closed");
        }

    }

    private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIOFailure)
    {
        if (pCallback.m_bSuccess != 1 || bIOFailure)
        {
            Debug.Log("There was an error retrieving the NumberOfCurrentPlayers.");
        }
        else
        {
            Debug.Log("The number of players playing your game: " + pCallback.m_cPlayers);
        }
    }

    private void GetSteamUserID()
    {
        if (SteamManager.Initialized)
        {
            CSteamID steamId = SteamUser.GetSteamID();
            //LobbyConnectionHandler.instance.myUserId = steamId.ToString();
            Photon.Pun.PhotonNetwork.AutomaticallySyncScene = true;
            Photon.Pun.PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues();
            Photon.Pun.PhotonNetwork.AuthValues.UserId = steamId.ToString();//SteamUser.GetSteamID().ToString();
            Photon.Pun.PhotonNetwork.ConnectUsingSettings();
            //    LobbyConnectionHandler.instance.Init();
            Debug.Log("User ID: " + steamId);

        }
    }
    private void GetUserFriends()
    {
        if (SteamManager.Initialized)
        {
            int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
            Debug.Log("[STEAM-FRIENDS] Listing " + friendCount + " Friends.");
            for (int i = 0; i < friendCount; ++i)
            {
                CSteamID friendSteamId = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
                string friendName = SteamFriends.GetFriendPersonaName(friendSteamId);
                EPersonaState friendState = SteamFriends.GetFriendPersonaState(friendSteamId);

                Debug.Log(friendName + " is " + friendState);
                //Invite friend to game
                //SteamFriends.
            //    SteamFriends.InviteUserToGame(friendSteamId, "path");
            }
           
        }

    }

    private void GetUserName()
    {
        string name = SteamFriends.GetPersonaName();
        LobbyConnectionHandler.instance.myDisplayName = name;
        Debug.Log(name);
    }
}