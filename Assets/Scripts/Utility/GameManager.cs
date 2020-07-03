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

    public List<CharacterSelectUI> PlayerObjsMul;
    public bool goesNextLevelInsteadOfRetry;
    //public GameObject conquered_go;
    //private Transform conquered_t;

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

    private Dictionary<Player, int> playerSelectionDict = new Dictionary<Player, int>();

    // private int player1CharacterSelection = 1;
    // private int player2CharacterSelection = 1;
    // private int player3CharacterSelection = 1;
    // private int player4CharacterSelection = 1;


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

    public void Awake()
    {
        Cursor.visible = true;

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
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
        Cursor.visible = false;
        GamesCompletedTally.gameWasCompleted = false;

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
        // Debug.Log(Input.GetJoystickNames().Length);

        var cutsceneObjects = GameObject.FindGameObjectsWithTag("CutsceneObject");
        HasCutsceneObjectsActive = cutsceneObjects.Length > 0;

        //  Debug.Log(playerSelectionDict.Count);
        bool isRestartBtnDown = false;
        bool isGoToMenuBtnDown = false;
        bool isPauseBtnDown = false;
        bool isFromPauseToMenuDown = false;
        bool isRestartBtnDownY = false;
        bool isGotoPlayerLevelSelectBtnDown = false;

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
        if (LevelUIManager.Instance != null)
        {
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
                if (isPauseBtnDown && !LobbyConnectionHandler.instance.IsMultiplayerMode)
                {
                    if (TutorialManager.instance == null)// && !HasCutsceneObjectsActive)
                        TogglePause();
                }

                if (paused)
                {
                    if (isFromPauseToMenuDown)
                    {
                        TogglePause();

                        SceneManager.LoadScene("MainMenu");
                        gameOver = false;
                        canAdvance = false;
                    }
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
        if (DoubleMatch.useDoubleMatch)
            SteamGameAchievements.instance.UnlockAchievement(SteamGameAchievements.Achievement.UFO_ACHIEVEMENT_1_1);

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
                    LobbyConnectionHandler.instance.LoadSceneMaster("MainMenu");

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


    public void GameEnds()
    {
        // winnerPlayer = winner;

        //winsText.SetActive(true);
        gameOver = true;
        StartCoroutine(DelayThis());
        instanceMe.instance.gameObject.SetActive(true);
        instanceUI.instance.gameObject.SetActive(false);
        GamesCompletedTally.gameWasCompleted = true;
        GamesCompletedTally.gamesCompleted++;
        /* conquered_go = GameObject.Find("CAMERA/Conquered_UI_PFX");
         conquered_t = conquered_go.transform;
         MyTargetGroup = GameObject.Find("CAMERA/TargetGroup1").GetComponent<CinemachineTargetGroup>();
         conquered_go.SetActive(true);
         MyTargetGroup.AddMember(conquered_t, 0.25f, 0f);*/
        GamesCompletedTally.gameWasCompleted = true;
        GamesCompletedTally.gamesCompleted++;

        UnlockSystem.instance.SaveMatchesCompleted();

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
        RemoveAllPlayersFromGame();
        SceneManager.LoadScene("MainMenu");
    }

    public List<Player> GetActivePlayersMul(bool onlyMine)
    {

        List<Player> players = new List<Player>();

        if (onlyMine)
        {
            int spawnIndex = 0;
            int counter = 0;
            RemoveAllPlayersFromGame();
            
            foreach (Photon.Realtime.Player p in Photon.Pun.PhotonNetwork.PlayerList)
            {
                
                spawnIndex = counter;
                counter++;
              //  Debug.Log(spawnIndex);
                switch (spawnIndex)
                {
                    case 0:
                        AddPlayerToGame(Player.One);
                       // players.Add(Player.One);
                        break;
                    case 1:
                        AddPlayerToGame(Player.Two);
                       // players.Add(Player.Two);
                        break;
                    case 2:
                        AddPlayerToGame(Player.Three);
                       // players.Add(Player.Three);
                        break;
                    case 3:
                        AddPlayerToGame(Player.Four);
                       // players.Add(Player.Four);
                        break;
                    default:
                        AddPlayerToGame(Player.None);
                      //  players.Add(Player.None);
                        break;
                }
            }
            spawnIndex = 0;
            counter = 0;
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
        //else
        //{
        //    int spawnIndex = 0;
        //    int counter = 0;
        //    foreach (Photon.Realtime.Player p in Photon.Pun.PhotonNetwork.PlayerList)
        //    {
        //      //  if (p.UserId == Photon.Pun.PhotonNetwork.LocalPlayer.UserId)
        //        {
        //            spawnIndex = counter;
        //        //    break;
        //        }
        //        counter++;
        //        switch (spawnIndex)
        //        {
        //            case 0:
        //                players.Add(Player.One);
        //                break;
        //            case 1:
        //                players.Add(Player.Two);
        //                break;
        //            case 2:
        //                players.Add(Player.Three);
        //                break;
        //            case 3:
        //                players.Add(Player.Four);
        //                break;
        //            default:
        //                players.Add(Player.One);
        //                break;
        //        }
        //    }
        //    Debug.Log("Spawn Number: " + spawnIndex);

        //}
        //List<Player> players = new List<Player>();
        else
        {
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

    public void SetPlayerCharacterChoice(Player p, int choice)
    {
        if (playerSelectionDict.ContainsKey(p))
        {
            playerSelectionDict[p] = choice;
            
            Debug.Log(choice);
        }
        else
        {
            playerSelectionDict.Add(p, choice);
        }

        if (LobbyConnectionHandler.instance.playerSelectionDict.ContainsKey(p))
        {
            LobbyConnectionHandler.instance.playerSelectionDict[p] = choice;

            Debug.Log(choice);
        }
        else
        {
            LobbyConnectionHandler.instance.playerSelectionDict.Add(p, choice);
        }
        

    }


    public int GetPlayerCharacterChoice(Player p)
    {
        Debug.Log(p.ToString());
        try
        {
            return /*LobbyConnectionHandler.instance.*/playerSelectionDict[p];
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogWarning(e.Message);
            return 0;
        }

    }

    public GameObject GetPlayerModel(Player player)
    {
        Debug.Log(this.name);
        return characters[GetPlayerCharacterChoice(player)].characterModel;
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

    IEnumerator DelayThis ()
    {
        yield return new WaitForSeconds(8.0f);
        canAdvance = true;
    }
}
