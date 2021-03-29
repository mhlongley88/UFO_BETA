using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using static NormalWeapon;
using static SuperWeapon;
using Rewired;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public class CharacterAssets
    {
        public int characterId;
        public GameObject characterModel;
        public NormalWeaponTypes defaultNormalWeaponType;
        public SuperWeaponTypes superWeaponType;
        public int matchThreshold;
    }
    public bool gameOver;
    public bool canAdvance = false;
    public LayerMask boundaryMask;
    public GameObject pauseScreen;
    public bool paused = false;
    public bool isRewardEventsMatch = false;
    public List<CharacterSelectUI> PlayerObjsMul;
    public bool goesNextLevelInsteadOfRetry;

    public bool enterTutorial = false;
    public int selectedLevelIndex;
    public bool isLocalSPMode, IsLocalPvPMode;
    //public GameObject conquered_go;
    //private Transform conquered_t;

    public int localPlayerRank;
    public Player localPlayer;
    public int LocalPlayerId;

    private CinemachineTargetGroup MyTargetGroup;

    [SerializeField]
    private CharacterAssets[] characters;

    public CharacterAssets[] Characters
    {
        get
        {
            return characters;
        }
    }

    [SerializeField]
    private CharacterAssets[] charactersMul;

    public CharacterAssets[] CharactersMul
    {
        get
        {
            return charactersMul;
        }
    }

    //private Dictionary<Player, int> playerSelectionDict = new Dictionary<Player, int>();
    private Dictionary<Player, PlayerData> playerSelectionDict = new Dictionary<Player, PlayerData>();

    //private Dictionary<Player, PlayerData> playerSelectionDictNew = new Dictionary<Player, PlayerData>();
    // private int player1CharacterSelection = 1;
    // private int player2CharacterSelection = 1;
    // private int player3CharacterSelection = 1;
    // private int player4CharacterSelection = 1;

    private StoreItemHolder tryItem;

    private Player activePlayers = Player.None;
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public bool HasCutsceneObjectsActive { get; private set; }

    const string PlayedWithCharacterKey = "UFO_PlayedWithCharacter";
    //public UnlockSteamAchievement playedWithAllCharactersAchievement;
    public string selectedLanguage;
    public Texture[] ConqueredMaterialTextures;
    public Sprite[] ConqueredMaterialSprites;
    public string playerNameKey = "displayName";
    public int selectedCharacterIndex;



    public void Awake()
    {
        Cursor.visible = true;

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            //tryItem = new StoreItemHolder();
            instance = this;
            DontDestroyOnLoad(this);
            
        }
        PlayerObjsMul = new List<CharacterSelectUI>();
        //if (instance != null)
        //{
        //    GameObject.Destroy(instance);
        //}
        //else
        //{
        //    instance = this;
        //    DontDestroyOnLoad(this);
        //   // PlayerObjsMul = new List<CharacterSelectUI>();
        //}
        //   PlayerObjsMul = new List<CharacterSelectUI>();
        // StartCoroutine(delayCheck());
    }
    // Use this for initialization
    void Start()
    {
        Cursor.visible = true;
        GamesCompletedTally.gameWasCompleted = false;

        bool playedWithAllCharacters = true;
        for (int i = 0; i < characters.Length; i++)
        {
            var playedWithCharacter = UserPrefs.instance.GetBool(PlayedWithCharacterKey + i, false);
            if(!playedWithCharacter)
            {
                playedWithAllCharacters = false;
                break;
            }
        }

       /* if(playedWithAllCharacters)
        {
            playedWithAllCharactersAchievement.Unlock();
        }*/
    }

    IEnumerator delayCheck()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            ControllerCheck();

            //for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            //{
            //    if (!string.IsNullOrEmpty(Input.GetJoystickNames()[i]))
            //    {
            //        Debug.Log("Joystick Connected " + i);
            //        Debug.Log(Input.GetJoystickNames()[i]);
            //        i = Input.GetJoystickNames().Length;
            //       // joystickDialogue = true;
            //    }
            //    else
            //    {
            //        Debug.Log("Joystick Disconnected");
            //        i = Input.GetJoystickNames().Length;
            //        //joystickDialogue = false;
            //    }
            //}
        }
    }

    public bool playstationController, xboxController, keyboard;
    public string[] currentControllers;
    public float controllerCheckTimer = 2;
    public float controllerCheckTimerOG = 2;

    public void ControllerCheck()
    {
        Debug.Log(Input.GetJoystickNames().Length);
        System.Array.Clear(currentControllers, 0, currentControllers.Length);
        System.Array.Resize<string>(ref currentControllers, Input.GetJoystickNames().Length);
        int numberOfControllers = 0;

        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            currentControllers[i] = Input.GetJoystickNames()[i].ToLower();
            if ((currentControllers[i] == "controller (xbox 360 for windows)" || currentControllers[i] == "controller (xbox 360 wireless receiver for windows)" || currentControllers[i] == "controller (xbox one for windows)"))
            {
                xboxController = true;
                keyboard = false;
                playstationController = false;
            }
            else if (currentControllers[i] == "wireless controller")
            {
                playstationController = true; //not sure if wireless controller is just super generic but that's what DS4 comes up as.
                keyboard = false;
                xboxController = false;
            }
            else if (currentControllers[i] == "")
            {
                numberOfControllers++;
            }

            Debug.Log(currentControllers[i] + " " + i);
        }
        if (numberOfControllers == Input.GetJoystickNames().Length)
        {
            keyboard = true;
            xboxController = false;
            playstationController = false;
        }

        Debug.Log(Input.GetJoystickNames().Length);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GetTryProps());
        //Debug.Log("Total Players: " + GameManager.Instance.GetActivePlayers().Count);
        var cutsceneObjects = GameObject.FindGameObjectsWithTag("CutsceneObject");
        HasCutsceneObjectsActive = cutsceneObjects.Length > 0;

        //  Debug.Log(playerSelectionDict.Count);
        bool isRestartBtnDown = false;
        bool isGoToMenuBtnDown = false;
        bool isPauseBtnDown = false;
        bool isFromPauseToMenuDown = false;
        bool isRestartBtnDownY = false;
        bool isGotoPlayerLevelSelectBtnDown = false;

        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            var activePlayers = GameManager.Instance.GetActivePlayers();
            foreach (Player i in activePlayers)
            {
                int playerIndex = 0;
                switch (i)
                {
                    case Player.One: playerIndex = 0; break;
                    case Player.Two: playerIndex = 1; break;
                    case Player.Three: playerIndex = 2; break;
                    case Player.Four: playerIndex = 3; break;
                }

                var playerInput = ReInput.players.GetPlayer(playerIndex);

                if (!isRestartBtnDown) isRestartBtnDown = playerInput.GetButtonDown("Restart");
                if (!isGoToMenuBtnDown) isGoToMenuBtnDown = playerInput.GetButtonDown("GoToMainMenu");
                if (!isPauseBtnDown) isPauseBtnDown = playerInput.GetButtonDown("Pause");
                if (!isFromPauseToMenuDown)
                    isFromPauseToMenuDown = playerInput.GetButtonDown("FromPauseToMenu");

                if (!isRestartBtnDownY) isRestartBtnDownY = playerInput.GetButton("RetryY");
                if (!isGotoPlayerLevelSelectBtnDown) isGotoPlayerLevelSelectBtnDown = playerInput.GetButton("RetryN");
            }
        }
        else
        {
            //for(int i = 0; i < 4; i++)//Iterate over all connected joysticks - Iterating over all possible joystick inputs for now!
            {
                var playerInput = ReInput.players.GetPlayer(0);

                if (!isRestartBtnDown) isRestartBtnDown = playerInput.GetButtonDown("Restart");
                if (!isGoToMenuBtnDown) isGoToMenuBtnDown = playerInput.GetButtonDown("GoToMainMenu");
                if (!isPauseBtnDown) isPauseBtnDown = playerInput.GetButtonDown("Pause");
                if (!isFromPauseToMenuDown)
                    isFromPauseToMenuDown = playerInput.GetButtonDown("FromPauseToMenu");

                if (!isRestartBtnDownY) isRestartBtnDownY = playerInput.GetButton("RetryY");
                if (!isGotoPlayerLevelSelectBtnDown) isGotoPlayerLevelSelectBtnDown = playerInput.GetButton("RetryN");
            }
        }

        if (LevelUIManager.Instance && LevelUIManager.Instance.lostToBots.activeInHierarchy)
        {
            if ((isRestartBtnDownY || Input.GetKeyDown(KeyCode.Y)))
            {
                RestartGame();
            }

            if (isGotoPlayerLevelSelectBtnDown || Input.GetKeyDown(KeyCode.N))
            {
                goesNextLevelInsteadOfRetry = true;
                RestartGame();
            }
        }

        // Only checks in game play
        //Debug.Log("levelUI instance exists: " + (LevelUIManager.Instance == null));
        if (LevelUIManager.Instance != null)
        {
            //Debug.Log(canAdvance + "---" + isGoToMenuBtnDown + "---" + gameOver);
            if (gameOver)
            {
                
                // else
                {
                    if (canAdvance == true && (isRestartBtnDown || Input.GetKeyDown(KeyCode.R)))
                    {
                        RestartGame();
                    }

                    if (canAdvance == true && isGoToMenuBtnDown || Input.GetKeyDown(KeyCode.M))
                    {
                        EndGameAndGoToMenu();
                    }
                }
            }
            else
            {
                if (isPauseBtnDown/* && !LobbyConnectionHandler.instance.IsMultiplayerMode*/)
                {
                    if (TutorialManager.instance == null)// && !HasCutsceneObjectsActive)
                        TogglePause();
                }

                if (paused)
                {
                    //if (isFromPauseToMenuDown)
                    //{
                    //    TogglePause();

                    //    SceneManager.LoadScene("MainMenu");
                    //    gameOver = false;
                    //    canAdvance = false;
                    //}
                }
            }
        }
       /* if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }*/


    }

    public void RestartGame()
    {
        gameOver = false;
        canAdvance = false;
        if (paused) TogglePause();

        if (goesNextLevelInsteadOfRetry)
        {
            MainMenuUIManager.goDirectlyToLevelSelect = true;
            goesNextLevelInsteadOfRetry = false;

            PlayerBot.active = false;
            foreach(var bp in PlayerBot.chosenPlayer)
            {
                RemovePlayerFromGame(bp);
            }

            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                if (LobbyConnectionHandler.instance.IsMultiplayerMode && Photon.Pun.PhotonNetwork.CurrentRoom != null)
                {
                    LobbyConnectionHandler.instance.LoadSceneMaster("MainMenu");
                }
                else if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }

           /* if (DoubleMatch.useDoubleMatch && UnlockAchievementPostLevel.chosen)
            {
                //SteamGameAchievements.instance.UnlockAchievement(UnlockAchievementPostLevel.achievementSelected);
                //UnlockAchievementPostLevel.chosen = false;
            }*/
        }
        else
        {
            if (LobbyConnectionHandler.instance.IsMultiplayerMode && Photon.Pun.PhotonNetwork.CurrentRoom != null)
            {
                //SceneManager.LoadScene("MainMenu");
                //LobbyConnectionHandler.instance.LoadSceneMaster(SceneManager.GetActiveScene().name);
                //LobbyConnectionHandler.instance.LoadSceneMaster("LevelUI");
                LobbyConnectionHandler.instance.LoadSceneMaster("LoadingRoom");

            }
            else if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
            {

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
            }

        }
    }

    public void EndGameAndGoToMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

     /*   if (DoubleMatch.useDoubleMatch && UnlockAchievementPostLevel.chosen)
        {
           // SteamGameAchievements.instance.UnlockAchievement(UnlockAchievementPostLevel.achievementSelected);
            UnlockAchievementPostLevel.chosen = false;
        }*/

        PlayerBot.active = false;
        DoubleMatch.useDoubleMatch = false;

        gameOver = false;
        canAdvance = false;
        if (paused) TogglePause();

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (LobbyConnectionHandler.instance.IsMultiplayerMode && Photon.Pun.PhotonNetwork.CurrentRoom != null)
            {
                //Photon.Pun.PhotonNetwork.LeaveRoom();

                //  if (Photon.Pun.PhotonNetwork.IsMasterClient)
                {
                    //SceneManager.LoadScene("MainMenu");
                    //LobbyConnectionHandler.instance.LoadSceneMaster("MainMenu");
                    Photon.Pun.PhotonNetwork.LeaveRoom();
                    SceneManager.LoadSceneAsync("MainMenu");

                }
            }
            else if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
            {

                SceneManager.LoadScene("MainMenu");
            }

        }
    }

    void GameOverControls()
    {

    }

    public void RemoveAllPlayersFromGame()
    {
        //Debug.Log("RemoveAllPlayersFromGame ");
        activePlayers = Player.None;
    }

    public void AddPlayerToGame(Player player)
    {
        activePlayers = activePlayers | player;
    }

    public void RemovePlayerFromGame(Player player)
    {
        
        activePlayers = activePlayers & (~player);
    }

    public bool IsPlayerInGame(Player player)
    {
        return (activePlayers & player) != 0;
    }


    public void GameEnds(bool gameWon = false)
    {

        // winnerPlayer = winner;

        //winsText.SetActive(true);
        gameOver = true;
        StartCoroutine(DelayThis(gameWon));
        instanceMe.instance.gameObject.SetActive(true);
        //instanceUI.instance.gameObject.SetActive(false);

        //var lastPlayerAlive = PlayerManager.Instance.GetLastAlivePlayer_Online();
        //bool winner = lastPlayerAlive == localPlayer;
        //Debug.Log(lastPlayerAlive + " - " + localPlayer);

        GameManager.Instance.AssignRewardOnResultScreen(gameWon);

        
        LevelUIManager.Instance.DisableAllUI();

        GamesCompletedTally.gameWasCompleted = true;
        GamesCompletedTally.gamesCompleted++;
        /* conquered_go = GameObject.Find("CAMERA/Conquered_UI_PFX");
         conquered_t = conquered_go.transform;
         MyTargetGroup = GameObject.Find("CAMERA/TargetGroup1").GetComponent<CinemachineTargetGroup>();
         conquered_go.SetActive(true);
         MyTargetGroup.AddMember(conquered_t, 0.25f, 0f);*/
        GamesCompletedTally.gameWasCompleted = true;
        GamesCompletedTally.gamesCompleted++;

        UnlockSystem.instance.SaveMatchesCompleted(gameWon);

        //StartCoroutine(ReturnMainMenu());
    }


    public IEnumerator ReturnMainMenu()
    {
        while (!Input.GetButton("MenuConfirm"))
        {
            yield return new WaitForEndOfFrame();
        }
        gameOver = false;
        canAdvance = false;
        Debug.Log("Here1");
        RemoveAllPlayersFromGame();
        SceneManager.LoadScene("MainMenu");
    }
    public Player localPlayerIndex;
    public Player GetMyPlayerIndexMul()
    {
        //Player myPlayer;
        //int spawnIndex = 0;
        //int counter = 0;
        //foreach (Photon.Realtime.Player p in Photon.Pun.PhotonNetwork.PlayerList)
        //{
        //    if (p.UserId == Photon.Pun.PhotonNetwork.LocalPlayer.UserId)
        //    {
        //        spawnIndex = counter;
        //        break;
        //    }
        //    counter++;
        //}
        //switch (spawnIndex)
        //{
        //    case 0:
        //        myPlayer = Player.One;
        //        break;
        //    case 1:
        //        myPlayer = Player.Two;
        //        break;
        //    case 2:
        //        myPlayer = Player.Three;
        //        break;
        //    case 3:
        //        myPlayer = Player.Four;
        //        break;
        //    default:
        //        myPlayer = Player.One;
        //        break;
        //}
        //return myPlayer;
        return localPlayerIndex;
    }

    public Player GetPlayerByIndex(int id)
    {
        Player p = Player.None;
        switch (id)
        {
            case 0:
                p = Player.One;
                break;
            case 1:
                p = Player.Two;
                break;
            case 2:
                p = Player.Three;
                break;
            case 3:
                p = Player.Four;
                break;
            default:
                p = Player.None;
                break;
        }
        return p;
    }

    public List<Player> GetActivePlayersMul(bool onlyMine)
    {

        List<Player> players = new List<Player>();

        if (onlyMine)
        {
            int spawnIndex = 0;
            int counter = 0;
            //RemoveAllPlayersFromGame();
            
            //foreach (Photon.Realtime.Player p in Photon.Pun.PhotonNetwork.PlayerList)
            //{
                
            //    spawnIndex = counter;
            //    counter++;
            //  //  Debug.Log(spawnIndex);
            //    switch (spawnIndex)
            //    {
            //        case 0:
            //            AddPlayerToGame(Player.One);
            //           // players.Add(Player.One);
            //            break;
            //        case 1:
            //            AddPlayerToGame(Player.Two);
            //           // players.Add(Player.Two);
            //            break;
            //        case 2:
            //            AddPlayerToGame(Player.Three);
            //           // players.Add(Player.Three);
            //            break;
            //        case 3:
            //            AddPlayerToGame(Player.Four);
            //           // players.Add(Player.Four);
            //            break;
            //        default:
            //            AddPlayerToGame(Player.None);
            //          //  players.Add(Player.None);
            //            break;
            //    }
            //}
            //spawnIndex = 0;
            //counter = 0;
            foreach (Photon.Realtime.Player p in Photon.Pun.PhotonNetwork.PlayerList)
            {
                if (p.UserId == Photon.Pun.PhotonNetwork.LocalPlayer.UserId)
                {
                    spawnIndex = counter;
                    break;
                }
                counter++;
            }
           // Debug.Log("Spawn Number: " + spawnIndex);
            switch (spawnIndex)
            {
                case 0:
                    players.Add(Player.One);
                    break;
                case 1:
                    players.Add(Player.Two);
                    break;
                case 2:
                    players.Add(Player.Three);
                    break;
                case 3:
                    players.Add(Player.Four);
                    break;
                default:
                    players.Add(Player.One);
                    break;
            }
        }
        
        else
        {
            Debug.Log("Here2");
            RemoveAllPlayersFromGame();
            int spawnIndex = 0;
            int counter = 0;
            foreach (Photon.Realtime.Player p in Photon.Pun.PhotonNetwork.PlayerList)
            {
                spawnIndex = counter;
                counter++;
                switch (spawnIndex)
                {
                    case 0:
                        AddPlayerToGame(Player.One);
                        if(LevelUIManager.Instance != null)
                            LevelUIManager.Instance.EnableUI(Player.One);
                        //players.Add(Player.One);
                        break;
                    case 1:
                        AddPlayerToGame(Player.Two);
                        if (LevelUIManager.Instance != null)
                            LevelUIManager.Instance.EnableUI(Player.Two);
                        //players.Add(Player.Two);
                        break;
                    case 2:
                        AddPlayerToGame(Player.Three);
                        if (LevelUIManager.Instance != null)
                            LevelUIManager.Instance.EnableUI(Player.Three);
                        //players.Add(Player.Three);
                        break;
                    case 3:
                        AddPlayerToGame(Player.Four);
                        if (LevelUIManager.Instance != null)
                            LevelUIManager.Instance.EnableUI(Player.Four);
                        //players.Add(Player.Four);
                        break;
                    default:
                        //AddPlayerToGame(Player.None);
                        //players.Add(Player.None);
                        break;
                }
            }
        

            foreach (Player p in Player.GetValues(typeof(Player)))
            {

                if (IsPlayerInGame(p))
                {
                    
                    players.Add(p);
                }
            }
            
        }
        return players;
    }

    public List<Player> GetActivePlayers()
    {
        List<Player> players = new List<Player>();
        foreach (Player p in Player.GetValues(typeof(Player)))
        {
            if (IsPlayerInGame(p))
            {
                players.Add(p);
            }
        }
        return players;
    }

    public List<Player> GetAlivePlayers()
    {
        List<Player> players = new List<Player>();
        foreach (Player p in Player.GetValues(typeof(Player)))
        {
            if (IsPlayerInGame(p) && PlayerManager.Instance.players[p].lives > 0)
            {
                players.Add(p);
            }
        }
        return players;
    }

    public void SetPlayerCharacterChoice(Player p, PlayerData pd)
    {
        if (playerSelectionDict.ContainsKey(p))
        {
            playerSelectionDict[p] = pd;
            
           // Debug.Log(choice);
        }
        else
        {
            playerSelectionDict.Add(p, pd);
        }
    }

    public void SetPlayerCharacterChoice(Player p, int choice)
    {
        if (playerSelectionDict.ContainsKey(p))
        {
            playerSelectionDict[p].charId = choice;

            // Debug.Log(choice);
        }
        else
        {
            PlayerData pd = new PlayerData();
            pd.charId = choice;
            playerSelectionDict.Add(p, pd);
        }
    }

    public int GetPlayerCharacterChoice(Player p)
    {
       // Debug.Log(p.ToString());
        try
        {
            return playerSelectionDict[p].charId;
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogWarning(e.Message);
            return 0;
        }

    }

    public PlayerData GetPlayerDataChoice(Player p)
    {
        try
        {
            return playerSelectionDict[p];
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogWarning(e.Message);
            return null;
        }
    }

    public PlayerData GetUFODataChoice(int id)
    {
        UFOAttributes attr = GameManager.Instance.GetUfoAttribute(id);
        PlayerData data = GameManager.Instance.GetNewPlayerData(LobbyConnectionHandler.instance.myDisplayName, id, attr.ufoLevel, attr.ufoXP);
        return data;
    }

    public PlayerData GetNewPlayerData(string dispName, int charId, int level, float currLevelProg)
    {
        PlayerData pd = new PlayerData();
        pd.charId = charId;
        pd.currLevelProgress = currLevelProg;
        pd.plevel = level.ToString();
        pd.pname = dispName;
        return pd;
    }

    public GameObject GetPlayerModel(Player player)
    {
        //Debug.Log(this.name);
        return characters[GetPlayerCharacterChoice(player)].characterModel;
    }

    public GameObject GetLocalPlayerModel()
    {
        //Debug.Log(this.name);
        return characters[GetPlayerCharacterChoice(localPlayer)].characterModel;
    }

    public void SetPlayerPlayedWithThisModel(Player player)
    {
        int characterIndex = GetPlayerCharacterChoice(player);
        UserPrefs.instance.SetBool(PlayedWithCharacterKey + characterIndex, true);
    }

    public SuperWeaponTypes GetCharacterSuperWeapon(int index)
    {
        return characters[index].superWeaponType;
    }

    internal NormalWeaponTypes GetCharacterNormalWeapon(int index)
    {
        return characters[index].defaultNormalWeaponType;
    }

    public void TogglePause()
    {
        if (LoadLevelPostCutscene.instance != null) return;

        if(TutorialManager.instance != null)
        {
            if (!TutorialManager.instance.canGoToMenu)
                return;
        }

        paused = !paused;
        //DOTween.TogglePauseAll();
        
        if (PauseMenu.instance != null && PauseMenu.instance.menuCanvasObj != null)
            PauseMenu.instance.menuCanvasObj.SetActive(paused);

        //if (paused)
        //{
        //    pauseScreen.SetActive(true);
        //}
        //else
        //{
        //    pauseScreen.SetActive(false);
        //}
    }

    public void postGame()
    {
        new WaitForSeconds(2.0f);
        instancePostGame.instance.gameObject.SetActive(true);
    }

    IEnumerator DelayThis (bool gameWon)
    {
        yield return new WaitForSeconds(3f);
        TouchGameUI.instance.ResultScreenControls.SetActive(true);
        PlayerManager.Instance.resultScreenUI.EnableResultScreen(gameWon);
        yield return new WaitForSeconds(5.0f);
        
        canAdvance = true;
    }

    public string GetDisplayName()
    {
        return UserPrefs.instance.GetString(playerNameKey);
        
    }
    public void SetDisplayName(string s)
    {
        UserPrefs.instance.SetString(playerNameKey, s);
    }

    public UFOAttributes GetUfoAttribute(int index)
    {
        //UFOAttributes attr = UserPrefs.instance.GetUFOProps().ufoData.Find(item => item.ufoIndex == index).ufoXP;
        return UserPrefs.instance.GetUFOProps().ufoData.Find(item => item.ufoIndex == index);
    }

    public UFOAttributes GetSelectedUfoAttribute()
    {
        //UFOAttributes attr = UserPrefs.instance.GetUFOProps().ufoData.Find(item => item.ufoIndex == index).ufoXP;
        return UserPrefs.instance.GetUFOProps().ufoData.Find(item => item.ufoIndex == selectedCharacterIndex);
    }

    public void SetUfoAttribute(int index, UFOAttributes val)
    {
        UFOAttributes attr = UserPrefs.instance.GetUFOProps().ufoData.Find(item => item.ufoIndex == index);
        attr.ufoXP = val.ufoXP;
        attr.ufoLevel = val.ufoLevel;
        attr.RateOfFire = val.RateOfFire;
        attr.Damage = val.Damage;
        attr.Accuracy = val.Accuracy;
        UserPrefs.instance.Save();
    }

    public void SetSelectedUfoAttribute(UFOAttributes val)
    {

        UFOAttributes attr = UserPrefs.instance.GetUFOProps().ufoData.Find(item => item.ufoIndex == selectedCharacterIndex);
        attr.ufoXP = val.ufoXP;
        attr.ufoLevel = val.ufoLevel;
        attr.RateOfFire = val.RateOfFire;
        attr.Damage = val.Damage;
        attr.Accuracy = val.Accuracy;
        UserPrefs.instance.Save();
    }

    public void UnlockUFO_Purchase(int index, int priceGems)
    {
        UFOAttributes attr = UserPrefs.instance.GetUFOProps().ufoData.Find(item => item.ufoIndex == index);
        //Debug.Log(index);
        if (!attr.isUnlocked)
        {
            attr.isUnlocked = true;
            AddGems(-priceGems);
            UserPrefs.instance.Save();
        }
        
    }

    public int GetCoins()
    {
        return UserPrefs.instance.GetInt("gold");
    }

    public void SetCoins(int val)
    {
        UserPrefs.instance.SetInt("gold", val);
    }

    public void AddCoins(int val, bool updateUI = true)
    {
        UserPrefs.instance.SetInt("gold", (GetCoins() + val));
        if (MainMenuUIManager.Instance && updateUI)
        {
            MainMenuUIManager.Instance.touchMenuUI.DisplayGemsCoinsMainHub();
        }
    }

    public int GetGems()
    {
        return UserPrefs.instance.GetInt("gems");
    }

    public void SetGems(int val)
    {
        UserPrefs.instance.SetInt("gems", val);
    }

    public void AddGems(int val, bool updateUI = true)
    {
        UserPrefs.instance.SetInt("gems", (GetGems() + val));
        if (MainMenuUIManager.Instance && updateUI)
        {
            MainMenuUIManager.Instance.touchMenuUI.DisplayGemsCoinsMainHub();
        }
    }

    public void PurchaseSkin(int skinId, int characterId, int price)
    {
        UFOAttributes attr = GetUfoAttribute(characterId);
        //Debug.Log(characterId + "--" + skinId);
        if (!attr.Skins[skinId].isUnlocked)
        {
            attr.Skins[skinId].isUnlocked = true;
            AddGems(-price);
            UserPrefs.instance.Save();
        }
    }

    public void SetCurrentSkin(int skinId, int characterId)
    {
        UFOAttributes attr = GetUfoAttribute(characterId);
        attr.currSkinId = skinId;
        UserPrefs.instance.Save();
    }

    public void SetTryProps(StoreItem item)
    {
        tryItem = new StoreItemHolder();
        tryItem.itemId = item.itemId;
        tryItem.skin_characterId = item.skin_characterId;
        tryItem.type = item.type;
        tryItem.requiredGems = item.requiredGems;
    }

    public StoreItemHolder GetTryProps()
    {
        return tryItem;
    }

    public void SetTryProps(StoreItemHolder item)
    {
        tryItem = item;
    }

    public void ResetTryProps()
    {
        tryItem = null;
    }

    public void AssignRewardOnResultScreen(bool gameWon)
    {
        //int lowerBound = (4 - localPlayerRank)* 20;
        //int upperBound = (4 - localPlayerRank) * 50;
        UFOAttributes attr = GetUfoAttribute(selectedCharacterIndex);
        if (gameWon)
        {
            SetGems((GetGems() + UnityEngine.Random.Range(8, 13)));
            GameManager.Instance.SetCoins((GameManager.Instance.GetCoins() + UnityEngine.Random.Range(150, 223)));
            attr.ufoXP += UnityEngine.Random.Range(0.15f, 0.22f);
        }
        else
        {
            GameManager.Instance.SetCoins((GameManager.Instance.GetCoins() + UnityEngine.Random.Range(66, 89)));
            attr.ufoXP += UnityEngine.Random.Range(0.04f, 0.12f);
        }

        if (isRewardEventsMatch)
        {
            SetGems((GetGems() + UnityEngine.Random.Range(10, 20)));
        }
        SetUfoAttribute(selectedCharacterIndex, attr);
        
    }
}
