using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using static InputManager;
using Rewired;

public class MainMenuUIManager : MonoBehaviour
{

    public enum Menu
    {
        Splash,
        LevelSelect,
        CharacterSelect
    }
    public GameObject PV_GameObj;
    public Button OnlineButton;
    public GameObject MainPanel;
    public GameObject OfflineCharacterSelectionPanel;
    public GameObject OnlineCharacterSelectionPanel;
    public GameObject characterSelect;
    public GameObject characterSelectMul;
    public GameObject levelSelect;
    public GameObject mainTitle;

    public GameObject mainTitleAlienCharacters;
    public GameObject mainTitleDust, mainTitleStars;
    public GameObject tryTutorialScreen;

    //public GameObject cameraMoveObject;

    public GameObject vCam1SplashMenu;
    public GameObject vCam2LevelSelect;
    public GameObject vCam3CharacterSelect;

    public Menu currentMenu = Menu.Splash;

    public int levelInt;

    public AudioClip StartGameSFX;
    public AudioClip toLevelSelect;
    public AudioSource myAudioSource;

    public CharacterSelectUI[] characterSelectMenus;
    public CharacterSelectUI[] characterSelectMenusMul;
    public CharacterSelectUI[] characterSelectMenusMulPrefabs;
    public LevelSelectCharacters levelSelectCharacters;

    public static bool goDirectlyToLevelSelect;

    private Player playerSelectFlags = Player.None;

    private static MainMenuUIManager instance;

    public static MainMenuUIManager Instance
    {
        get
        {
            return instance;
        }
    }

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            Debug.Log("PC");
            isConsole = false;
            isPC = true;
        }
        else if(Application.platform == RuntimePlatform.PS4 || Application.platform == RuntimePlatform.XboxOne)
        {
            Debug.Log("Console");
            isPC = false;
            isConsole = true;
        }

        SetCameraView(vCam1SplashMenu);
        //vCam1.SetActive(true);;
        //vCam2.SetActive(false);


       // MainPanel.SetActive(false);

        characterSelect.SetActive(false);

        //goDirectlyToLevelSelect = true;
        if (goDirectlyToLevelSelect)
        {
            MainPanel.SetActive(false);

            //vCam1.SetActive(false);
            //vCam2.SetActive(true);
            SetCameraView(vCam2LevelSelect);

            myAudioSource.PlayOneShot(toLevelSelect);
            levelSelect.SetActive(true);
            levelSelectCharacters.AddActivePlayers();
            characterSelect.SetActive(false);
            currentMenu = Menu.LevelSelect;
            goDirectlyToLevelSelect = false;
        }

    }

    public void OnlineButtonDisabledListener()
    {
        if (Photon.Pun.PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.ConnectedToMasterServer)
        {
            OnlineButton.interactable = false;
        }
    }

    private bool CharacterSelectPlayersReady()
    {
        bool ready = false;
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
           // bool ready = false;
            foreach (var c in characterSelectMenus)
            {
                if (c.GetCurSelectState() == CharacterSelectUI.CharacterSelectState.ReadyToStart)
                {
                    ready = true;
                }
            }
            if (ready == true)
            {
                foreach (var c in characterSelectMenus)
                {
                    if (c.GetCurSelectState() == CharacterSelectUI.CharacterSelectState.SelectingCharacter)
                    {
                        ready = false;
                    }
                }
            }
        }
        else
        {
            //bool ready = false;
            foreach (var c in GameManager.Instance.PlayerObjsMul/*characterSelectMenusMul*/)
            {
                if (c.GetCurSelectState() == CharacterSelectUI.CharacterSelectState.ReadyToStart)
                {
                    ready = true;
                }
            }
            if (ready == true)
            {
                foreach (var c in GameManager.Instance.PlayerObjsMul/*characterSelectMenusMul*/)
                {
                    if (c.GetCurSelectState() == CharacterSelectUI.CharacterSelectState.SelectingCharacter)
                    {
                        ready = false;
                    }
                }
            }
        }
        return ready;
    }


    bool isConsole = false, isPC = false;
    // Update is called once per frame
    void Update()
    {
        mainTitle.SetActive(!levelSelect.activeInHierarchy);

        mainTitleAlienCharacters.SetActive(mainTitle.activeInHierarchy);
        mainTitleDust.SetActive(mainTitle.activeInHierarchy);
        mainTitleStars.SetActive(mainTitle.activeInHierarchy);

        levelInt = ShowLevelTitle.levelStaticInt;
        if (isPC)
        {
            // Debug.Log("PC_______");
            //PC_Controls();
            ConsoleControls();
        }
        else if(isConsole)
        {
            ConsoleControls();
        }
        // if (inCharSelect && CharacterSelect.instance)
        // CharacterSelect.instance.BeginCharacterSelect();
        
    }

    void PC_Controls()
    {
        foreach (Player p in Enum.GetValues(typeof(Player)))
        {
            if (p != Player.None)
            {
                int rewirePlayerId = 0;
                Rewired.Player rewirePlayer;

                switch (p)
                {
                    case Player.One: rewirePlayerId = 0; break;
                    case Player.Two: rewirePlayerId = 1; break;
                    case Player.Three: rewirePlayerId = 2; break;
                    case Player.Four: rewirePlayerId = 3; break;
                }

                rewirePlayer = ReInput.players.GetPlayer(rewirePlayerId);

                switch (currentMenu)
                {
                    case Menu.Splash:
                        //if (InputManager.Instance.GetButtonDownKB(ButtonEnum.Submit, p))
                        if (rewirePlayer.GetButtonDown("Submit"))
                        {

                           // cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("moveToChar");
                            //SetCameraView(vCam1)

                          //  characterSelect.SetActive(true);
                            myAudioSource.PlayOneShot(StartGameSFX);
                            currentMenu = Menu.CharacterSelect;
                        }
                        break;
                    case Menu.CharacterSelect:
                        //if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDownKB(ButtonEnum.Back, p))
                        if (GameManager.Instance.IsPlayerInGame(p) && rewirePlayer.GetButtonDown("Back"))
                        {
                            


                            currentMenu = Menu.Splash;
                        }
                        if (CharacterSelectPlayersReady())
                        {
                            // if (CharacterSelectPlayersReady())
                            // {
                            //vCam1.SetActive(false);
                            //vCam2.SetActive(true);
                            SetCameraView(vCam2LevelSelect);

                            myAudioSource.PlayOneShot(toLevelSelect);
                            levelSelect.SetActive(true);
                            levelSelectCharacters.AddActivePlayers();
                            characterSelect.SetActive(false);
                            currentMenu = Menu.LevelSelect;
                            // }
                        }
                        break;
                    case Menu.LevelSelect:
                        //if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDownKB(ButtonEnum.Back, p))
                        if (GameManager.Instance.IsPlayerInGame(p) && rewirePlayer.GetButtonDown("Back"))
                        {
                            //vCam2.SetActive(false);
                            //vCam1.SetActive(true);
                            SetCameraView(vCam1SplashMenu);

                            currentMenu = Menu.CharacterSelect;
                            levelSelectCharacters.RemoveAllPlayers();
                            levelSelect.SetActive(false);
                            characterSelect.SetActive(true);
                            foreach (var c in characterSelectMenus)
                            {
                                c.ReturnFromLevelSelect();
                            }
                        }
                        //if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDownKB(ButtonEnum.Submit, p) && ShowLevelTitle.levelStaticInt != 0)
                        if (GameManager.Instance.IsPlayerInGame(p) && rewirePlayer.GetButtonDown("Submit") && ShowLevelTitle.levelStaticInt != 0)
                        {
                            if (UnlockSystem.instance.MatchesCompleted <= 0)
                                tryTutorialScreen.SetActive(true);
                            else
                                SceneManager.LoadScene("LoadingRoom");
                        }
                        break;
                }
            }
        }
    }

    public void OfflineButtonListener()
    {
        LobbyConnectionHandler.instance.IsMultiplayerMode = false;
    }

    public void SwitchToCharacterSelect()
    {
        LobbyConnectionHandler.instance.IsMultiplayerMode = false;
        GameManager.Instance.RemoveAllPlayersFromGame();
        //cameraMoveObject.transform.position = new Vector3(cameraMoveObject.transform.position.x, cameraMoveObject.transform.position.y,
        //                          cameraMoveObject.transform.position.z - 24f);

        SetCameraView(vCam3CharacterSelect);

        // characterSelect.transform.localScale = new Vector3(0.3229257f, 0.3229257f, 0.3229257f); 
        //cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("moveToChar");
        characterSelect.SetActive(true);
        //characterSelect.transform.localScale = new Vector3(0.3229257f, 0.3229257f, 0.3229257f);
        myAudioSource.PlayOneShot(StartGameSFX);
        currentMenu = Menu.CharacterSelect;
        foreach (var c in characterSelectMenus)
        {
            c.gameObject.SetActive(true);
            c.gameObject.transform.localScale = Vector3.one;
        }
        OnlineCharacterSelectionPanel.SetActive(false);
        OfflineCharacterSelectionPanel.SetActive(true);
    }

    public void SwitchBackToCharacterSelectMul()
    {
        //cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("moveToChar");
        if(LobbyConnectionHandler.instance.isPrivateMatch)
            LobbyUI.instance.FriendsListButton.SetActive(true);
        Photon.Pun.PhotonNetwork.CurrentRoom.IsOpen = true;
        //vCam2.SetActive(false);
        //vCam1.SetActive(true);

        SetCameraView(vCam3CharacterSelect);

        currentMenu = Menu.CharacterSelect;
        levelSelectCharacters.RemoveAllPlayers();
        levelSelect.SetActive(false);
        characterSelect.SetActive(true);
        //foreach (var c in characterSelectMenus)
        //{
        //    c.ReturnFromLevelSelect();
        //}

        LobbyConnectionHandler.instance.myPlayerInGame.GetComponent<CharacterSelectUI>().ReturnFromLevelSelect();

        if (levelSelectCharacters.myLevelPlayerMul)
        {
            Photon.Pun.PhotonNetwork.Destroy(levelSelectCharacters.myLevelPlayerMul);
        }
        //LobbyConnectionHandler.instance.IsMultiplayerMode = true;
        GameManager.Instance.RemoveAllPlayersFromGame();
        ////  LobbyConnectionHandler.instance.gameObject.AddComponent<Photon.Pun.PhotonView>();

        //cameraMoveObject.transform.position = new Vector3(cameraMoveObject.transform.position.x, cameraMoveObject.transform.position.y,
        //                          cameraMoveObject.transform.position.z + 24f); Debug.Log("Left call");
        //characterSelect.SetActive(true);
        myAudioSource.PlayOneShot(StartGameSFX);
        //currentMenu = Menu.CharacterSelect;
        //foreach (var c in characterSelectMenusMul)
        //{
        //    c.gameObject.SetActive(true);
        //    c.gameObject.transform.localScale = Vector3.one;
        //}
        OfflineCharacterSelectionPanel.SetActive(false);
        OnlineCharacterSelectionPanel.SetActive(true);
    }

    public void SwitchToCharacterSelectMul()
    {
        //cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("moveToChar");
        LobbyConnectionHandler.instance.IsMultiplayerMode = true;
       // LobbyUI.instance.FriendsListButton.SetActive(true);
        GameManager.Instance.RemoveAllPlayersFromGame();
        //  LobbyConnectionHandler.instance.gameObject.AddComponent<Photon.Pun.PhotonView>();

        //cameraMoveObject.transform.position = new Vector3(cameraMoveObject.transform.position.x, cameraMoveObject.transform.position.y,
        //                          cameraMoveObject.transform.position.z - 24f);

        SetCameraView(vCam3CharacterSelect);

        characterSelect.SetActive(true);
        myAudioSource.PlayOneShot(StartGameSFX);
        currentMenu = Menu.CharacterSelect;
        //foreach (var c in characterSelectMenusMul)
        //{
        //    c.gameObject.SetActive(true);
        //    c.gameObject.transform.localScale = Vector3.one;
        //}
        OfflineCharacterSelectionPanel.SetActive(false);
        OnlineCharacterSelectionPanel.SetActive(true);
    }
    public Player myPlayerMul;
    public void PlayerEnterMul(Player p)
    {
        //myPlayerMul = p;
        foreach (var c in characterSelectMenusMul)
        {
            if(c.player == p)
            {
                c.PlayerEnterGame();
            }
        }
    }

    void ConsoleControls()
    {
        if (LobbyConnectionHandler.instance.IsMultiplayerMode)
            MulMode();
        else
            OfflineMode();
    }
    public bool selectingCharacters = false;
    void OfflineMode()
    {
        foreach (Player p in Enum.GetValues(typeof(Player)))
        {
            if (p != Player.None)
            {
                int rewirePlayerId = 0;
                Rewired.Player rewirePlayer;

                switch (p)
                {
                    case Player.One: rewirePlayerId = 0; break;
                    case Player.Two: rewirePlayerId = 1; break;
                    case Player.Three: rewirePlayerId = 2; break;
                    case Player.Four: rewirePlayerId = 3; break;
                }

                rewirePlayer = ReInput.players.GetPlayer(rewirePlayerId);

                switch (currentMenu)
                {
                    //case Menu.Splash:

                    //    if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Submit, p))
                    //    {

                    //        cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("moveToChar");
                    //        characterSelect.SetActive(true);
                    //        myAudioSource.PlayOneShot(StartGameSFX);
                            
                    //        currentMenu = Menu.CharacterSelect;
                    //    }
                    //    break;
                    case Menu.CharacterSelect:
                        //if (/*GameManager.Instance.IsPlayerInGame(p) && */!selectingCharacters && InputManager.Instance.GetButtonDown(ButtonEnum.Back, p))
                        if (/*GameManager.Instance.IsPlayerInGame(p) && */!selectingCharacters && rewirePlayer.GetButtonDown("Back"))
                        {
                          //  cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("movetoMainMenu");
                            currentMenu = Menu.Splash;
                           
                            characterSelect.SetActive(false);

                            //cameraMoveObject.transform.position = new Vector3(cameraMoveObject.transform.position.x, cameraMoveObject.transform.position.y, 
                            //cameraMoveObject.transform.position.z + 24f);
                            SetCameraView(vCam1SplashMenu);

                            MainPanel.SetActive(true);
                        }
                        if (CharacterSelectPlayersReady())
                        {
                            // if (CharacterSelectPlayersReady())
                            // {
                            //vCam1.SetActive(false);
                            //vCam2.SetActive(true);
                            SetCameraView(vCam2LevelSelect);

                            myAudioSource.PlayOneShot(toLevelSelect);
                            levelSelect.SetActive(true);
                            levelSelectCharacters.AddActivePlayers();
                            characterSelect.SetActive(false);
                            
                            currentMenu = Menu.LevelSelect;
                            // }
                        }
                        break;
                    case Menu.LevelSelect:
                        
                        //if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDown(ButtonEnum.Back, p))
                        if (GameManager.Instance.IsPlayerInGame(p) && rewirePlayer.GetButtonDown("Back"))
                        {
                            //vCam2.SetActive(false);
                            //vCam1.SetActive(true);
                            SetCameraView(vCam3CharacterSelect);

                            currentMenu = Menu.CharacterSelect;
                            levelSelectCharacters.RemoveAllPlayers();
                            levelSelect.SetActive(false);
                            characterSelect.SetActive(true);

                            if (PlayerBot.active)
                            {
                               // SetCameraView(vCam3CharacterSelect);

                                foreach (var bot in PlayerBot.chosenPlayer)
                                {
                                    GameManager.Instance.RemovePlayerFromGame(bot);
                                }

                                PlayerBot.active = false;
                            }

                            foreach (var c in characterSelectMenus)
                            {
                                c.gameObject.SetActive(true);
                                c.gameObject.transform.localScale = Vector3.one;

                                c.ReturnFromLevelSelect();
                            }

                            OnlineCharacterSelectionPanel.SetActive(false);
                            OfflineCharacterSelectionPanel.SetActive(true);
                        }

                        //if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDown(ButtonEnum.Submit, p) && ShowLevelTitle.levelStaticInt != 0)
                        if (GameManager.Instance.IsPlayerInGame(p) && rewirePlayer.GetButtonDown("Submit") && ShowLevelTitle.levelStaticInt != 0)
                        {
                            if (UnlockSystem.instance.MatchesCompleted <= 0)
                                tryTutorialScreen.SetActive(true);
                            else
                                SceneManager.LoadScene("LoadingRoom");
                        }
                        break;
                }
            }
        }
    }


    void MulMode()
    {
        foreach (Player p in Enum.GetValues(typeof(Player)))
        {
            if (p != Player.None)
            {
                int rewirePlayerId = 0;
                Rewired.Player rewirePlayer;

                switch (p)
                {
                    case Player.One: rewirePlayerId = 0; break;
                    case Player.Two: rewirePlayerId = 1; break;
                    case Player.Three: rewirePlayerId = 2; break;
                    case Player.Four: rewirePlayerId = 3; break;
                }

                rewirePlayer = ReInput.players.GetPlayer(rewirePlayerId);

                switch (currentMenu)
                {
                    //case Menu.Splash:
                    //    if (InputManager.Instance.GetButtonDown(ButtonEnum.Submit, p))
                    //    {

                    //        cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("moveToChar");
                    //        characterSelectMul.SetActive(true);
                    //        myAudioSource.PlayOneShot(StartGameSFX);
                    //        currentMenu = Menu.CharacterSelect;
                    //    }
                    //    break;
                    case Menu.CharacterSelect:
                        //if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDown(ButtonEnum.Back, p))
                        //{
                        //    //cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("movetoMainMenu");
                        //    //currentMenu = Menu.Splash;
                        //    Photon.Pun.PhotonNetwork.LeaveRoom();
                        //}
                        if (CharacterSelectPlayersReady() && Photon.Pun.PhotonNetwork.CurrentRoom.PlayerCount > 1)
                        {
                            // if (CharacterSelectPlayersReady())
                            // {
                            //vCam1.SetActive(false);
                            //vCam2.SetActive(true);
                            SetCameraView(vCam2LevelSelect);

                            myAudioSource.PlayOneShot(toLevelSelect);
                            levelSelect.SetActive(true);
                            levelSelectCharacters.AddActivePlayers();
                            characterSelect.SetActive(false);
                            currentMenu = Menu.LevelSelect;
                            Photon.Pun.PhotonNetwork.CurrentRoom.IsOpen = false;
                            LobbyUI.instance.FriendsListButton.SetActive(false);
                            Debug.Log("shouldnt be here");
                            // }
                        }
                        break;
                    case Menu.LevelSelect:
                        //if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDown(ButtonEnum.Back, p))
                        if (GameManager.Instance.IsPlayerInGame(p) && rewirePlayer.GetButtonDown("Back"))
                        {
                            //vCam2.SetActive(false);
                            //vCam1.SetActive(true);
                            //currentMenu = Menu.CharacterSelect;
                            levelSelectCharacters.RemoveAllPlayers();
                            //levelSelect.SetActive(false);
                            //characterSelect.SetActive(true);
                            //foreach (var c in characterSelectMenus)
                            //{
                            //    c.ReturnFromLevelSelect();
                            //}
                        }
                        //if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDown(ButtonEnum.Submit, p) && ShowLevelTitle.levelStaticInt != 0)
                        if (GameManager.Instance.IsPlayerInGame(p) && rewirePlayer.GetButtonDown("Submit") && ShowLevelTitle.levelStaticInt != 0)
                        {
                            //SceneManager.LoadScene("LoadingRoom");
                            LobbyConnectionHandler.instance.LoadSceneMaster("LoadingRoom");
                        }
                        break;
                }
            }
        }
    }

    public void SetCameraView(GameObject obj)
    {
        vCam1SplashMenu.SetActive(false);
        vCam2LevelSelect.SetActive(false);
        vCam3CharacterSelect.SetActive(false);

        obj.SetActive(true);
    }

    public void LoadRoom()
    {
        SceneManager.LoadScene("LoadingRoom");
    }
}
