using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using static InputManager;
using Rewired;
using Cinemachine;
using  TMPro;
using OneClickLocalization;
public class MainMenuUIManager : MonoBehaviour
{

    public enum Menu
    {
        Splash,
        LevelSelect,
        CharacterSelect
    }

    public TMP_FontAsset NotoSans_Font, youngfrankconditalSDF_Font;
    public List<FontSwap> FontSwapTexts;

    public GameObject OptionsCanvas;
    public Transform LevelsContainer;

    public GameObject HostNameLevelSelect;
    public TextMeshPro HostNameLevelSelect_text;

    public GameObject PV_GameObj;
    public Button OnlineButton;
    public GameObject MainPanel;
    public GameObject OfflineCharacterSelectionPanel;
    public GameObject OnlineCharacterSelectionPanel;
    public GameObject characterSelect;
    public GameObject characterSelectMul;
    public GameObject levelSelect;
    public GameObject mainTitle;
    public GameObject NotEnoughPlayersTextObj;

    public GameObject mainTitleAlienCharacters;
    public GameObject mainTitleDust, mainTitleStars;
    public GameObject tryTutorialScreen;

    //public GameObject cameraMoveObject;

    public GameObject vCam1SplashMenu;
    public GameObject vCam2LevelSelect;
    public GameObject vCam3CharacterSelect;
    public CinemachineBrain cinemachineBrain;
    public float blendTimeTransitionToCharacterSelect = 0.7f;
    public float blendTimeDefault = 2.0f;

    public Menu currentMenu = Menu.Splash;

    public int levelInt;

    public AudioClip StartGameSFX;
    public AudioClip toLevelSelect;
    public AudioSource myAudioSource;

    public CharacterSelectUI[] characterSelectMenus;
    public CharacterSelectUI[] characterSelectMenusMul;
    public CharacterSelectUI[] characterSelectMenusMulPrefabs;
    public LevelSelectCharacters levelSelectCharacters;
    public Sprite[] CountryFlags;
    public Texture[] ConqueredMaterialTextures;
    public Sprite[] ConqueredMaterialSprites;
    public TMP_Dropdown languageDropdown;

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
        //PopulateLanguageSelectDropdown();//Before-
    }
    void DisablePlayersLeftTextObj()
    {
        NotEnoughPlayersTextObj.SetActive(false);//here
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
        if (LobbyConnectionHandler.instance.showPlayersLeftText)
        {
            LobbyConnectionHandler.instance.showPlayersLeftText = false;
            NotEnoughPlayersTextObj.SetActive(true);
            Invoke("DisablePlayersLeftTextObj", 4f);//Here
        }
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
        GameManager.Instance.ConqueredMaterialTextures = ConqueredMaterialTextures;
        GameManager.Instance.ConqueredMaterialSprites = ConqueredMaterialSprites;
        ControllerConnectionManager.instance.AssignAllJoySticksToPlayers();
        PopulateLanguageSelectDropdown();//After-
    }

    public void OnlineButtonDisabledListener()
    {
        if (Photon.Pun.PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.ConnectedToMasterServer)
        {
            OnlineButton.interactable = false;
        }
    }

    public int CountPlayersEnteringMatch()
    {
        int i = 0;
        foreach (var c in characterSelectMenus)
        {
            if (c.GetCurSelectState() == CharacterSelectUI.CharacterSelectState.ReadyToStart)
            {
                i++;
            }
        }

        return i;
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
        if(vCam1SplashMenu.activeInHierarchy)
        {
            cinemachineBrain.m_DefaultBlend.m_Time = blendTimeTransitionToCharacterSelect;
        }else
            cinemachineBrain.m_DefaultBlend.m_Time = blendTimeDefault;

        mainTitle.SetActive(!levelSelect.activeInHierarchy);

        mainTitleAlienCharacters.SetActive(mainTitle.activeInHierarchy);
        mainTitleDust.SetActive(mainTitle.activeInHierarchy);
        mainTitleStars.SetActive(mainTitle.activeInHierarchy);

        levelInt = ShowLevelTitle.levelStaticInt;
        //This wasdupl
        if (isPC) 
        {
            //If isPC = true,
            // Debug.Log("PC_______");
            //PC_Controls();
            ConsoleControls();
        }
        else if (isConsole)
        {
            //If isPC = false, There's no difference at all! Both conditions are calling consolecontrols. 
            //You can leave it as it is :)
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
                        //Debug.Log("LevelSelect");
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

    public void OfflineButtonListener(int i)
    {
        LobbyConnectionHandler.instance.IsMultiplayerMode = false;
        GameManager.Instance.isLocalSPMode = i == 0 ? true : false;
        GameManager.Instance.IsLocalPvPMode = i == 1 ? true : false;
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
        HostNameLevelSelect.SetActive(false);
        currentMenu = Menu.CharacterSelect;
        levelSelectCharacters.RemoveAllPlayers();
        levelSelect.SetActive(false);
        characterSelect.SetActive(true);
        //foreach (var c in characterSelectMenus)
        //{
        //    c.ReturnFromLevelSelect();
        //}

        LobbyConnectionHandler.instance.myPlayerInGame.GetComponent<CharacterSelectUI>().ReturnFromLevelSelect();
        CharacterSelectUI[] AllMenus = FindObjectsOfType<CharacterSelectUI>();
        foreach (CharacterSelectUI character in AllMenus)
        {
            character.selectState = CharacterSelectUI.CharacterSelectState.SelectingCharacter;
            character.readyObject.SetActive(true);
        }

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
        GameManager.Instance.isLocalSPMode = GameManager.Instance.IsLocalPvPMode = false;
        // LobbyUI.instance.FriendsListButton.SetActive(true);
        GameManager.Instance.RemoveAllPlayersFromGame();
        //  LobbyConnectionHandler.instance.gameObject.AddComponent<Photon.Pun.PhotonView>();

        //cameraMoveObject.transform.position = new Vector3(cameraMoveObject.transform.position.x, cameraMoveObject.transform.position.y,
        //                          cameraMoveObject.transform.position.z - 24f);
        //Rewired.Player p;
        //p.controllers.ClearAllControllers();
        //rewirePlayer = ReInput.players.GetPlayer(3);

        

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
    public bool menuOpen;
    public void ButtonToggleOptionsMenu()
    {
        if (!menuOpen)
        {
            // previousTimescale = Time.timeScale;//getting the current timescale
            // Time.timeScale = 0;//Pausing time
            OptionsCanvas.SetActive(true);
            GameManager.Instance.paused = true;
            menuOpen = true;
        }
        else
        {
            // Time.timeScale = previousTimescale;//unpausing time
            if (SimpleMenuSelection.currentFocused && SimpleMenuSelection.currentFocused.isOptionsMenu)
            {
                SimpleMenuSelection.currentFocused.CloseOptionsMenu();
                //OptionsCanvas.SetActive(false);
                //GameManager.Instance.paused = false;
                //menuOpen = false;
            }
        }
    }


    public bool selectingCharacters = false;
    void OfflineMode()
    {
        //if (GameManager.Instance.paused)//Temporary!!! Remove this
        //    return;
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

                if (/*Input.GetKeyDown(KeyCode.Escape)*/ rewirePlayer.GetButtonDown("Pause") /*&& !menuOpen*/)
                {
                    Debug.Log("Pause pressed");
                    ButtonToggleOptionsMenu();
                }
                //Debug.Log(GameManager.Instance.paused);
                
                //rewirePlayer.controllers.maps.SetAllMapsEnabled(true);
                // Debug.Log("Rewired" + rewirePlayerId);
                //rewirePlayer = ReInput.players.GetPlayer(3);
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
                        if (/*GameManager.Instance.IsPlayerInGame(p) && */!selectingCharacters && rewirePlayer.GetButtonDown("Back") && !GameManager.Instance.paused)
                        {
                          //  cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("movetoMainMenu");

                            foreach(CharacterSelectUI characterSelect in characterSelectMenus)
                            {
                                characterSelect.EnableCharacterSelectSplashScreen();

                            }

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

                            if (GameManager.Instance.isLocalSPMode && CountPlayersEnteringMatch() == 1)
                            {
                                //Allocate all joysticks to P4
                                //Debug.Log("Allocate all joysticks to P4");
                                ControllerConnectionManager.instance.AssignAllSurplusJoysticksToP4();
                            }

                            currentMenu = Menu.LevelSelect;
                            // }
                        }
                        break;
                    case Menu.LevelSelect:

                        //if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDown(ButtonEnum.Back, p))
                        if (HostNameLevelSelect.activeSelf)
                        {
                            HostNameLevelSelect.SetActive(false);
                        }
                        if (GameManager.Instance.IsPlayerInGame(p) && rewirePlayer.GetButtonDown("Back") && !GameManager.Instance.paused)
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
                            if (GameManager.Instance.isLocalSPMode)
                            {
                                //Allocate all joysticks to Players
                                Debug.Log("Allocate all joysticks to Players");
                                ControllerConnectionManager.instance.AssignAllJoySticksToPlayers();
                            }
                            OnlineCharacterSelectionPanel.SetActive(false);
                            OfflineCharacterSelectionPanel.SetActive(true);
                        }

                        //if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDown(ButtonEnum.Submit, p) && ShowLevelTitle.levelStaticInt != 0)
                        if (GameManager.Instance.IsPlayerInGame(p) && rewirePlayer.GetButtonDown("Submit") && ShowLevelTitle.levelStaticInt != 0 && !GameManager.Instance.paused)
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
    public void ReEnableAllUnlockedLevels()
    {
        for (int i = 0; i < LevelUnlockCheck.All.Count; i++)
        {
            LevelUnlockCheck.All[i].gameObject.SetActive(true);
            LevelUnlockCheck.All[i].Init();
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

                //rewirePlayer = ReInput.players.GetPlayer(rewirePlayerId);
                rewirePlayer = ReInput.players.GetPlayer(3);

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

                            if (Photon.Pun.PhotonNetwork.IsMasterClient)
                            {
                                for (int i = 0; i < LevelUnlockCheck.All.Count; i++)
                                {
                                    Debug.Log(LevelUnlockCheck.All[i].gameObject.name + " is " + LevelUnlockCheck.All[i].gameObject.activeSelf);
                                    LobbyConnectionHandler.instance.pv.RPC("SyncVisibleLevels_LevelSelect", Photon.Pun.RpcTarget.Others, LevelUnlockCheck.All[i].gameObject.name, LevelUnlockCheck.All[i].gameObject.activeSelf);
                                }
                            }

                            Photon.Pun.PhotonNetwork.CurrentRoom.IsOpen = false;
                            LobbyUI.instance.FriendsListButton.SetActive(false);

                            if(Photon.Pun.PhotonNetwork.IsMasterClient){
                                LobbyConnectionHandler.instance.pv.RPC("SyncHostName_LevelSelect", Photon.Pun.RpcTarget.All, LobbyConnectionHandler.instance.myDisplayName);
                            }
                            
                            Debug.Log("shouldnt be here");
                            // }
                        }
                        break;
                    case Menu.LevelSelect:
                        //if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDown(ButtonEnum.Back, p))
                        if (GameManager.Instance.IsPlayerInGame(p) && rewirePlayer.GetButtonDown("Back") 
                        && Photon.Pun.PhotonNetwork.IsMasterClient)
                        {
                            //vCam2.SetActive(false);
                            //vCam1.SetActive(true);
                            //currentMenu = Menu.CharacterSelect;
                            //levelSelectCharacters.RemoveAllPlayers();
                            LobbyConnectionHandler.instance.pv.RPC("SwitchBackToCharacterSelect", Photon.Pun.RpcTarget.All);
                            //levelSelect.SetActive(false);
                            //characterSelect.SetActive(true);
                            //foreach (var c in characterSelectMenus)
                            //{
                            //    c.ReturnFromLevelSelect();
                            //}
                        }
                        //if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDown(ButtonEnum.Submit, p) && ShowLevelTitle.levelStaticInt != 0)
                        Debug.Log("LevelSelect?" + ShowLevelTitle.levelStaticInt);
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

    public void SimulateBackFromOnlineMode()
    {
        Photon.Pun.PhotonNetwork.LeaveRoom();
    }
   
    public string GetLocalLanguageRepresentation(string lang)//#1
    {
        string s = "";

        switch (lang)// This one? Order doesn't matter in this one!
        {
            case "English":
                s = "English";
                break;
            case "German":
                s = "Deutsche";
                break;
            case "ChineseSimplified":
                s = "中文";//Update to chinese --
                break;
            case "French":
                s = "Français";//Update to French
                break;
            case "Italian":
                s = "Italiano";
                break;
            case "Portuguese":
                s = "Português";
                break;
            case "Russian":
                s = "Pусский";
                break;
            case "Spanish":
                s = "Español";
                break;
            case "Polish":
                s = "Polski";
                break;
            case "Dutch":
                s = "Nederlands";
                break;
        }

        return s;
    }

    public string GetEnglishRepresentation(string lang)//#2
    {
        string s = "";

        switch (lang)// This one? Order doesn't matter in this one!
        {
            case "English":
                s = "English";
                break;
            case "Deutsche":
                s = "German";
                break;
            case "中文"://Update to chinese
                s = "ChineseSimplified";
                break;
            case "Français"://Update to French
                s = "French";
                break;
            case "Italiano":
                s = "Italian";
                break;
            case "Português":
                s = "Portuguese";
                break;
            case "Pусский":
                s = "Russian";
                break;
            case "Español":
                s = "Spanish";
                break;
            case "Polski":
                s = "Polish";
                break;
            case "Nederlands":
                s = "Dutch";
                break;
        }

        return s;
    }

    public void PopulateLanguageSelectDropdown()//#3
    {
        languageDropdown.ClearOptions();
        List<string> languagesStrings = new List<string>();
        languagesStrings.Add("English");
        // Add supported languages
        //Debug.Log(OCL.GetLanguages().Count + " Total languages");

        foreach (SystemLanguage supportedLanguage in OCL.GetLanguages())
        {
            if (!languagesStrings.Contains(supportedLanguage.ToString()))
                languagesStrings.Add(GetLocalLanguageRepresentation(supportedLanguage.ToString()));
        }
        languageDropdown.AddOptions(languagesStrings);
        Debug.Log(languageDropdown.value + "" + languageDropdown.options.Count);
        GameManager.Instance.selectedLanguage = languageDropdown.options[languageDropdown.value].text;
        foreach (TMP_Dropdown.OptionData item in languageDropdown.options)
        {
            //item.image = CountryFlags[i++];
            // 0th item - English Flag
            // 1st item - Chinese 
            // 2- french
            // 3- German
            // 4 Italian
            // 5 Portugese
            // 6 Russian
            // 7 Spanish
            // 8 Polish
            // 9 Dutch
            // Update language names in native language
            switch (item.text)
            {
                case "English":
                    item.image = CountryFlags[0];
                    break;
                case "中文"://Update to chinese
                    item.image = CountryFlags[1];
                    break;
                case "Français"://Update to French
                    item.image = CountryFlags[2];
                    break;
                case "Deutsche"://Update to Deutsche
                    item.image = CountryFlags[3];
                    break;
                case "Italiano":
                    item.image = CountryFlags[4];
                    break;
                case "Português":
                    item.image = CountryFlags[5];
                    break;
                case "Pусский":
                    item.image = CountryFlags[6];
                    break;
                case "Español":
                    item.image = CountryFlags[7];
                    break;
                case "Polski":
                    item.image = CountryFlags[8];
                    break;
                case "Nederlands":
                    item.image = CountryFlags[9];
                    break;
            }
        }
    }
    //2
    public void OnLanguageDropdown_ValueChange(int value)
    {
        Debug.Log(value + "-" + languageDropdown.options[languageDropdown.value].text);
        string selectedLanguage = languageDropdown.options[languageDropdown.value].text;
        GameManager.Instance.selectedLanguage = selectedLanguage;
        OCL.SetLanguage((SystemLanguage)Enum.Parse(typeof(SystemLanguage), GetEnglishRepresentation(selectedLanguage)));

        foreach (FontSwap obj in FontSwapTexts)
        {
            obj.GetComponent<TextMeshProUGUI>().font = languageDropdown.options[languageDropdown.value].text == "Russian" ? NotoSans_Font : youngfrankconditalSDF_Font;
            //obj.swapFont(languageDropdown.options[languageDropdown.value].text == "Russian" ? NotoSans_Font : youngfrankconditalSDF_Font);
        }

    }

}
