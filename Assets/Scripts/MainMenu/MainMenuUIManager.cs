using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
using static InputManager;

public class MainMenuUIManager : MonoBehaviour
{

    public enum Menu
    {
        Splash,
        LevelSelect,
        CharacterSelect
    }

    public GameObject characterSelect;
    public GameObject levelSelect;

    public GameObject cameraMoveObject;

    public GameObject vCam1;
    public GameObject vCam2;

    private Menu currentMenu = Menu.Splash;

    public int levelInt;

    public AudioClip StartGameSFX;
    public AudioClip toLevelSelect;
    public AudioSource myAudioSource;

    public CharacterSelectUI[] characterSelectMenus;

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


        vCam1.SetActive(true);
        vCam2.SetActive(false);


        characterSelect.SetActive(false);

    }


    private bool CharacterSelectPlayersReady()
    {

        bool ready = false;
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
        return ready;
    }



    // Update is called once per frame
    void Update()
    {


        levelInt = ShowLevelTitle.levelStaticInt;

        // if (inCharSelect && CharacterSelect.instance)
        // CharacterSelect.instance.BeginCharacterSelect();
        foreach(Player p in Enum.GetValues(typeof(Player)))
        {
            if(p != Player.None)
            {
                switch (currentMenu)
                {
                    case Menu.Splash:
                        if (InputManager.Instance.GetButtonDown(ButtonEnum.Submit, p))
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
                            foreach(var c in characterSelectMenus)
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
}
