using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Steamworks;

public class SteamScript : MonoBehaviour
{
    
 /*   public GameObject FriendPrefab, FriendsListParent, FriendsListPanel, InvitationsParent, InvitationsPrefab;


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
        instance = this;
        if (SteamManager.Initialized)
        {
            GetUserName();
            GetSteamUserID();
            //GetUserFriends();
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
            PhotonChatClient.instance.Connect("834485bf-64a5-4d0e-8e7f-f6898b8707ab", "0", steamId.ToString());
            //    LobbyConnectionHandler.instance.Init();
            Debug.Log("User ID: " + steamId);

        }
    }

    void RefreshFriendsList()
    {
        int count = FriendsListParent.transform.childCount;
        for(int i=0; i < count; i++)
        {
            Destroy(FriendsListParent.transform.GetChild(i).gameObject);
        }
    }

    public void GetUserFriends()
    {
        if (SteamManager.Initialized)
        {
            RefreshFriendsList();
            int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
            Debug.Log("[STEAM-FRIENDS] Listing " + friendCount + " Friends.");
            for (int i = 0; i < friendCount; ++i)
            {
                CSteamID friendSteamId = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
                string friendName = SteamFriends.GetFriendPersonaName(friendSteamId);
                EPersonaState friendState = SteamFriends.GetFriendPersonaState(friendSteamId);

                GameObject temp = Instantiate(FriendPrefab, FriendsListParent.transform);
                temp.GetComponent<FriendProps>().NameText.text = friendName;
                temp.GetComponent<FriendProps>().InviteButton.onClick.AddListener(() => InviteFriend(friendSteamId));
                temp.GetComponent<FriendProps>().Avatar.sprite = Sprite.Create(GetSmallAvatar(friendSteamId), new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
                
                if(friendState == EPersonaState.k_EPersonaStateOnline)
                {
                    temp.GetComponent<FriendProps>().OnlineStatus.color = Color.green;
                    temp.GetComponent<FriendProps>().OfflinePanel.SetActive(false);
                }
                else
                {
                    temp.GetComponent<FriendProps>().OnlineStatus.color = Color.gray;
                    temp.GetComponent<FriendProps>().OfflinePanel.SetActive(true);
                }

                Debug.Log(friendName + " is " + friendSteamId.ToString());
                //Invite friend to game
                //SteamFriends.
               // SteamFriends.InviteUserToGame(friendSteamId, "path");
            }
            FriendsListPanel.SetActive(true);
        }

    }

    public void InviteRecieved(string id, string roomId)
    {
        if (Photon.Pun.PhotonNetwork.CurrentRoom == null && MainMenuUIManager.Instance.currentMenu == MainMenuUIManager.Menu.Splash)
        {
            GameObject temp = Instantiate(InvitationsPrefab, InvitationsParent.transform);
            temp.GetComponent<InvitationProps>().RoomId = roomId;
            temp.GetComponent<InvitationProps>().InvitationBy = id;
            temp.GetComponent<InvitationProps>().AcceptInviteButton.onClick.AddListener(() => JoinInvitedRoom(roomId));
            temp.transform.SetAsFirstSibling();
        }
    }

    void JoinInvitedRoom(string roomId)
    {
        LobbyConnectionHandler.instance.JoinInvitedRoom(roomId);
    }

    public void InviteFriend(CSteamID id)
    {
        if (Photon.Pun.PhotonNetwork.CurrentRoom != null)
        {
            string roomId = Photon.Pun.PhotonNetwork.CurrentRoom.Name;
            PhotonChatClient.instance.SendInvitation(id.ToString(), roomId);
        }
    }

    public Texture2D GetSmallAvatar(CSteamID user)
    {
        int FriendAvatar = SteamFriends.GetSmallFriendAvatar(user);
        uint ImageWidth;
        uint ImageHeight;
        bool success = SteamUtils.GetImageSize(FriendAvatar, out ImageWidth, out ImageHeight);

        if (success && ImageWidth > 0 && ImageHeight > 0)
        {
            byte[] Image = new byte[ImageWidth * ImageHeight * 4];
            Texture2D returnTexture = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, true);
            success = SteamUtils.GetImageRGBA(FriendAvatar, Image, (int)(ImageWidth * ImageHeight * 4));
            if (success)
            {
                returnTexture.LoadRawTextureData(Image);
                returnTexture.Apply();
            }
            return returnTexture;
        }
        else
        {
            Debug.LogError("Couldn't get avatar.");
            return new Texture2D(0, 0);
        }
    }


    private void GetUserName()
    {
        string name = SteamFriends.GetPersonaName();
        LobbyConnectionHandler.instance.myDisplayName = name;
        Debug.Log(name);
    }*/
}