using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using static NormalWeapon;
using static SuperWeapon;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public class CharacterAssets
    {
        public GameObject characterModel;
        public NormalWeaponTypes defaultNormalWeaponType;
        public SuperWeaponTypes superWeaponType;
    }
    public bool gameOver;
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


    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    // Use this for initialization
    void Start()
    {
        GamesCompletedTally.gameWasCompleted = false;

    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetKeyDown(KeyCode.R))
        {
            gameOver = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if(SceneManager.GetActiveScene().name != "MainMenu")
            {
                SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
            }

        }

        if (Input.GetKeyDown(KeyCode.M))
        {

            SceneManager.LoadScene("MainMenu");
            gameOver = false;

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Application.Quit();

        }


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

        StartCoroutine(ReturnMainMenu());
    }


    private IEnumerator ReturnMainMenu()
    {
        while (!Input.GetButton("MenuConfirm"))
        {
            yield return new WaitForEndOfFrame();
        }
        gameOver = false;
        RemoveAllPlayersFromGame();
        SceneManager.LoadScene("MainMenu");
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
        if(playerSelectionDict.ContainsKey(p))
        {
            playerSelectionDict[p] = choice;
        }
        else
        {
        playerSelectionDict.Add(p, choice);
        }
    }


    public int GetPlayerCharacterChoice(Player p)
    {
        try
        {
            return playerSelectionDict[p];
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogWarning(e.Message);
            return 0;
        }

    }

    public GameObject GetPlayerModel(Player player)
    {
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

    public void postGame()
    {
        new WaitForSeconds(2.0f);
        instancePostGame.instance.gameObject.SetActive(true);
    }
}
