using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using static InputManager;

public class MainMenuUIManager : MonoBehaviour
{

    public enum Menu
    {
        Splash,
        LevelSelect,
        CharacterSelect
    }

    public Text debugText;
    public Text debugText2;
    public Text debugText3;
    public Text debugText4;
    public GameObject MainPanel;
    public GameObject OfflineCharacterSelectionPanel;
    public GameObject OnlineCharacterSelectionPanel;
    public GameObject characterSelect;
    public GameObject characterSelectMul;
    public GameObject levelSelect;
    public GameObject mainTitle;

    public GameObject cameraMoveObject;

    public GameObject vCam1;
    public GameObject vCam2;

    public Menu currentMenu = Menu.Splash;

    public int levelInt;

    public AudioClip StartGameSFX;
    public AudioClip toLevelSelect;
    public AudioSource myAudioSource;

    public CharacterSelectUI[] characterSelectMenus;
    public CharacterSelectUI[] characterSelectMenusMul;
    public CharacterSelectUI[] characterSelectMenusMulPrefabs;
    public LevelSelectCharacters levelSelectCharacters;

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

            vCam1.SetActive(true);
        vCam2.SetActive(false);


        characterSelect.SetActive(false);

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
                switch (currentMenu)
                {
                    case Menu.Splash:
                        if (InputManager.Instance.GetButtonDownKB(ButtonEnum.Submit, p))
                        {

                            cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("moveToChar");
                            characterSelect.SetActive(true);
                            myAudioSource.PlayOneShot(StartGameSFX);
                            currentMenu = Menu.CharacterSelect;
                        }
                        break;
                    case Menu.CharacterSelect:
                        if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDownKB(ButtonEnum.Back, p))
                        {
                            cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("movetoMainMenu");
                            currentMenu = Menu.Splash;
                        }
                        if (CharacterSelectPlayersReady())
                        {
                            // if (CharacterSelectPlayersReady())
                            // {
                            vCam1.SetActive(false);
                            vCam2.SetActive(true);
                            myAudioSource.PlayOneShot(toLevelSelect);
                            levelSelect.SetActive(true);
                            levelSelectCharacters.AddActivePlayers();
                            characterSelect.SetActive(false);
                            currentMenu = Menu.LevelSelect;
                            // }
                        }
                        break;
                    case Menu.LevelSelect:
                        if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDownKB(ButtonEnum.Back, p))
                        {
                            vCam2.SetActive(false);
                            vCam1.SetActive(true);
                            currentMenu = Menu.CharacterSelect;
                            levelSelectCharacters.RemoveAllPlayers();
                            levelSelect.SetActive(false);
                            characterSelect.SetActive(true);
                            foreach (var c in characterSelectMenus)
                            {
                                c.ReturnFromLevelSelect();
                            }
                        }
                        if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDownKB(ButtonEnum.Submit, p) && ShowLevelTitle.levelStaticInt != 0)
                        {
                            SceneManager.LoadScene("LoadingRoom");
                        }
                        break;
                }
            }
        }
    }

    public void SwitchToCharacterSelect()
    {
        cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("moveToChar");
        characterSelect.SetActive(true);
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

    public void SwitchToCharacterSelectMul()
    {
        cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("moveToChar");
        characterSelect.SetActive(true);
        myAudioSource.PlayOneShot(StartGameSFX);
        currentMenu = Menu.CharacterSelect;
        foreach (var c in characterSelectMenusMul)
        {
            c.gameObject.SetActive(true);
            c.gameObject.transform.localScale = Vector3.one;
        }
        OfflineCharacterSelectionPanel.SetActive(false);
        //OnlineCharacterSelectionPanel.SetActive(true);
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

    void OfflineMode()
    {
        foreach (Player p in Enum.GetValues(typeof(Player)))
        {
            if (p != Player.None)
            {
                switch (currentMenu)
                {
                    case Menu.Splash:

                        


                        if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Submit, p))
                        {

                            cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("moveToChar");
                            characterSelect.SetActive(true);
                            myAudioSource.PlayOneShot(StartGameSFX);
                            
                            currentMenu = Menu.CharacterSelect;
                        }
                        break;
                    case Menu.CharacterSelect:
                        if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDown(ButtonEnum.Back, p))
                        {
                            cameraMoveObject.GetComponent<DOTweenAnimation>().DOPlayById("movetoMainMenu");
                            currentMenu = Menu.Splash;
                            MainPanel.SetActive(true);
                        }
                        if (CharacterSelectPlayersReady())
                        {
                            // if (CharacterSelectPlayersReady())
                            // {
                            vCam1.SetActive(false);
                            vCam2.SetActive(true);
                            myAudioSource.PlayOneShot(toLevelSelect);
                            levelSelect.SetActive(true);
                            levelSelectCharacters.AddActivePlayers();
                            characterSelect.SetActive(false);
                            currentMenu = Menu.LevelSelect;
                            // }
                        }
                        break;
                    case Menu.LevelSelect:
                        if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDown(ButtonEnum.Back, p))
                        {
                            vCam2.SetActive(false);
                            vCam1.SetActive(true);
                            currentMenu = Menu.CharacterSelect;
                            levelSelectCharacters.RemoveAllPlayers();
                            levelSelect.SetActive(false);
                            characterSelect.SetActive(true);
                            foreach (var c in characterSelectMenus)
                            {
                                c.ReturnFromLevelSelect();
                            }
                        }
                        if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDown(ButtonEnum.Submit, p) && ShowLevelTitle.levelStaticInt != 0)
                        {
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
                        if (CharacterSelectPlayersReady())
                        {
                            // if (CharacterSelectPlayersReady())
                            // {
                            vCam1.SetActive(false);
                            vCam2.SetActive(true);
                            myAudioSource.PlayOneShot(toLevelSelect);
                            levelSelect.SetActive(true);
                            levelSelectCharacters.AddActivePlayers();
                            characterSelect.SetActive(false);
                            currentMenu = Menu.LevelSelect;
                            Debug.Log("shouldnt be here");
                            // }
                        }
                        break;
                    case Menu.LevelSelect:
                        if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDown(ButtonEnum.Back, p))
                        {
                            //vCam2.SetActive(false);
                            //vCam1.SetActive(true);
                            //currentMenu = Menu.CharacterSelect;
                            //levelSelectCharacters.RemoveAllPlayers();
                            //levelSelect.SetActive(false);
                            //characterSelect.SetActive(true);
                            //foreach (var c in characterSelectMenus)
                            //{
                            //    c.ReturnFromLevelSelect();
                            //}
                        }
                        if (GameManager.Instance.IsPlayerInGame(p) && InputManager.Instance.GetButtonDown(ButtonEnum.Submit, p) && ShowLevelTitle.levelStaticInt != 0)
                        {
                            SceneManager.LoadScene("LoadingRoom");
                        }
                        break;
                }
            }
        }
    }
}
