using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;
using Steamworks;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using PlayFab;
using Rewired.Utils.Platforms.Windows;
using Rewired;
public class LobbyConnectionHandler : MonoBehaviourPunCallbacks, ILobbyCallbacks, IConnectionCallbacks, IMatchmakingCallbacks, IInRoomCallbacks
{
    public static LobbyConnectionHandler instance;
    public string myUserId, myDisplayName;
    public bool IsMultiplayerMode, isPrivateMatch;
    public PhotonView pv;
    public Dictionary<Player, int> playerSelectionDict = new Dictionary<Player, int>();

    public bool showGUI = false;
    // Start is called before the first frame update
    void Awake()
    {
        //if (instance != null && instance != this)
        //{
        //    Destroy(this);
        //}
        //else
        //{
        //    instance = this;
        //    DontDestroyOnLoad(this);
        //}

        if (instance != null)
        {
            //GameObject.Destroy(instance);
            Destroy(this.gameObject);
            // Destroy(this.GetComponent<PhotonView>());
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        IsMultiplayerMode = false;
        isPrivateMatch = false;

        //Init();
    }

    private void Start()
    {
        MainMenuUIManager.Instance.OnlineButton.interactable = false;
    }

    public void LoadSceneMaster(string sceneName)
    {
        pv.RPC("RPC_ChangeScene", RpcTarget.AllBuffered, sceneName);
    }

    [PunRPC]
    void RPC_ChangeScene(string sceneName)
    {
        GameManager.Instance.gameOver = false;
        GameManager.Instance.canAdvance = false;
        if(GameManager.Instance.paused) //= false;
        {
            GameManager.Instance.TogglePause();
        }
        if (pv.IsMine)
        {
            if (sceneName == "MainMenu" || sceneName == "LoadingRoom")
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
                SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
            }
        }
            
    }


    public void Init()
    {
        IsMultiplayerMode = false;
        pv = this.GetComponent<PhotonView>();
        PhotonNetwork.AutomaticallySyncScene = true;
       // PhotonNetwork.AuthValues.UserId = SteamUser.GetSteamID().ToString();
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            showGUI = !showGUI;
        }
    }

    private void OnGUI()
    {
        if(showGUI)
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
        if (PauseMenu.instance != null && PauseMenu.instance.menuCanvasObj != null)
            PauseMenu.instance.menuCanvasObj.SetActive(false);
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


    public Player myPlayerMul;
    public override void OnJoinedRoom()
    {
        //Debug.Log("Joined Room" + PhotonNetwork.CurrentRoom.Name);
        //if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        //{
        //    SceneManager.LoadScene("LoadingRoom");
        //}
        // SteamGameInvite.instance.SyncRoomIdAccrossSteam(PhotonNetwork.CurrentRoom.Name);
       
            MainMenuUIManager.Instance.SwitchToCharacterSelectMul();

        //if(isPrivateMatch)
            LobbyUI.instance.FriendsListButton.SetActive(true);
        RefreshCharacterSelectMul();

        //MainMenuUIManager.Instance.PlayerEnterMul(myPlayerMul);


        // foreach (Player p in playersMul)
        // {
        //GameManager.Instance.AddPlayerToGame(p);
        //MainMenuUIManager.Instance.PlayerEnterMul(p);

        //  }

        //GameManager.Instance.AddPlayerToGame(myplayerNumber[0]);

    }


    public void JoinInvitedRoom(string roomId)
    {
        if(PhotonNetwork.CurrentRoom == null)
        {
            PhotonNetwork.JoinRoom(roomId);
        }
    }

    public GameObject myPlayerInGame;
    void RefreshCharacterSelectMul()
    {
        //MainMenuUIManager.Instance.SwitchToCharacterSelectMul();
        GameManager.Instance.RemoveAllPlayersFromGame();
        GameManager.Instance.PlayerObjsMul.Clear();
        if (myPlayerInGame != null)
        {
            PhotonNetwork.Destroy(myPlayerInGame);
        }
        //List<Player> playersMul = GameManager.Instance.GetActivePlayersMul(false);
        List<Player> myplayerNumber = GameManager.Instance.GetActivePlayersMul(true);
        myPlayerMul = myplayerNumber[0];
        GameObject temp;
        switch (myPlayerMul)
        {
            case Player.One:
                myPlayerInGame = PhotonNetwork.Instantiate(MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[0].name,
                    MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[0].transform.position,
                    MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[0].transform.rotation);
                //Vector3 tempPos = temp.transform.position;
                //temp.transform.SetParent(MainMenuUIManager.Instance.characterSelect.transform);
                ////Debug.Log();
                //temp.transform.localPosition = MainMenuUIManager.Instance.characterSelectMenus[0].gameObject.transform.localPosition;//new Vector3(-19.78f, 0f, 0.82f);
                //temp.transform.localRotation = MainMenuUIManager.Instance.characterSelectMenus[0].transform.localRotation;
                //temp.transform.localScale = MainMenuUIManager.Instance.characterSelectMenus[0].transform.localScale;
                myPlayerInGame.GetComponent<CharacterSelectUI>().PlayerEnterGame();
                break;
            case Player.Two:
                myPlayerInGame = PhotonNetwork.Instantiate(MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[1].name,
                    MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[1].transform.position,
                    MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[1].transform.rotation);
                //Vector3 tempPos1 = temp.transform.position;
                //temp.transform.SetParent(MainMenuUIManager.Instance.characterSelect.transform);
                //temp.transform.position = tempPos1;
                //temp.transform.rotation = MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[1].transform.rotation;
                //temp.transform.localScale = MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[1].transform.localScale;
                myPlayerInGame.GetComponent<CharacterSelectUI>().PlayerEnterGame();
                break;
            case Player.Three:
                myPlayerInGame = PhotonNetwork.Instantiate(MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[2].name,
                    MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[2].transform.position,
                    MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[2].transform.rotation);
                //Vector3 tempPos2 = temp.transform.position;
                //temp.transform.SetParent(MainMenuUIManager.Instance.characterSelect.transform);
                //temp.transform.position = tempPos2;
                //temp.transform.rotation = MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[3].transform.rotation;
                //temp.transform.localScale = MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[3].transform.localScale;
                myPlayerInGame.GetComponent<CharacterSelectUI>().PlayerEnterGame();
                break;
            case Player.Four:
                myPlayerInGame = PhotonNetwork.Instantiate(MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[3].name,
                    MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[3].transform.position,
                    MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[3].transform.rotation);
                //Vector3 tempPos3 = temp.transform.position;
                //temp.transform.SetParent(MainMenuUIManager.Instance.characterSelect.transform);
                //temp.transform.position = tempPos3;
                //temp.transform.rotation = MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[3].transform.rotation;
                //temp.transform.localScale = MainMenuUIManager.Instance.characterSelectMenusMulPrefabs[3].transform.localScale;
                myPlayerInGame.GetComponent<CharacterSelectUI>().PlayerEnterGame();
                break;
            case Player.None:
                break;
        }
    }


    void IInRoomCallbacks.OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom");
        //if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        //{
        //    if(PhotonNetwork.IsMasterClient)
        //        SceneManager.LoadScene("LoadingRoom");
        //}
        //RefreshCharacterSelectMul();
        //List<Player> myplayerNumber = GameManager.Instance.GetActivePlayersMul(true);
        myPlayerInGame.GetComponent<CharacterSelectUI>().PlayerEnterGame();
    }

    void IInRoomCallbacks.OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {

        if( SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                
                if (MainMenuUIManager.Instance.currentMenu == MainMenuUIManager.Menu.LevelSelect)
                {
                    //back to characterselect

                    MainMenuUIManager.Instance.SwitchBackToCharacterSelectMul();
                    
                }
                

            }
            else
            {
                
            }
            RefreshCharacterSelectMul();
            myPlayerInGame.GetComponent<CharacterSelectUI>().isSynced = false;
            myPlayerInGame.GetComponent<CharacterSelectUI>().PlayerEnterGame();

        }
        else
        {
            //direct to splash screen
            SceneManager.LoadScene("MainMenu");

        }

        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
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
        //if (LobbyUI.instance.isPrivateMatch)
        //{
        //    roomOptions.IsOpen = false;
        //}
        PhotonNetwork.CreateRoom(null, roomOptions);


    }


    public override void OnConnectedToMaster()
    {
        // MainMenuUIManager.Instance.MainPanel.SetActive(false);

        // Maybe the player was reconnected after a sudden disconnection from the internet

        ControllerConnectionManager.instance.AssignAllJoySticksToPlayers();

        bool shouldBounceBackToMenu = false;

        if (IsMultiplayerMode) // it entered the multiplayer mode on character selection
            shouldBounceBackToMenu = true;
            
        
        IsMultiplayerMode = false;

        isPrivateMatch = false;
        LobbyUI.instance.FriendsListButton.SetActive(false);
        if (LobbyUI.instance != null)
        {
            Debug.Log(PhotonNetwork.LocalPlayer.UserId);
            LobbyUI.instance.isPrivateMatch = LobbyUI.instance.isPublicMatch = false;
            MainMenuUIManager.Instance.OnlineButton.interactable = true;
            if (!this.GetComponent<PhotonView>())
            {
                pv = this.gameObject.AddComponent<PhotonView>();//MainMenuUIManager.Instance.PV_GameObj.GetComponent<PhotonView>();
                pv.ViewID = 1;
            }
            //else
            //{
            //    destro
            //}
            LobbyUI.instance.AuthPanel.SetActive(false);

            //
            if (MainMenuUIManager.Instance.currentMenu != MainMenuUIManager.Menu.LevelSelect && shouldBounceBackToMenu)
            {
                //
                MainMenuUIManager.Instance.MainPanel.SetActive(true);
                MainMenuUIManager.Instance.characterSelect.SetActive(false);
                MainMenuUIManager.Instance.currentMenu = MainMenuUIManager.Menu.Splash;
                MainMenuUIManager.Instance.SetCameraView(MainMenuUIManager.Instance.vCam1SplashMenu);
            }
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
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
