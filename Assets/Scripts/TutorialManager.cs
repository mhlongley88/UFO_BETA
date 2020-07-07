using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayersReadyToJoint
{
    public Player player;
    public Rewired.Player input;
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public GameObject skipPanel;
    public PlayersReadyToJoint[] players;
    public TutorialAnimations currentTutorialAnimations;
    public TutorialAnimations tutorialAnimationsPrefab;
    public Transform tutorialAnimationsParent;
    public GameObject cityDesign;
    public bool canGoToMenu = false;
    bool allPlayersInGame = false;

    public static int levelIntAfterSkipTutorial;
    public static string introLevelNameAfterSkipTutorial;

    Vector3 firstTutorialAnimationsPos;
    Quaternion firstTutorialAnimationsRot;

    private void Awake()
    {
        instance = this;

        firstTutorialAnimationsPos = currentTutorialAnimations.transform.localPosition;
        firstTutorialAnimationsRot = currentTutorialAnimations.transform.localRotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        LevelUIManager.Instance.livesUIObject.SetActive(false);
        LevelUIManager.Instance.initialReadyFightAnim.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            PlayersReadyToJoint p = players[i];

            int rewirePlayerId = 1;
            switch (p.player)
            {
                case Player.One: rewirePlayerId = 0; break;
                case Player.Two: rewirePlayerId = 1; break;
                case Player.Three: rewirePlayerId = 2; break;
                case Player.Four: rewirePlayerId = 3; break;
            }

            p.input = ReInput.players.GetPlayer(rewirePlayerId);
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.T))
        {
            var newTutoAnim = Instantiate(tutorialAnimationsPrefab, tutorialAnimationsParent);
            newTutoAnim.city = cityDesign;

            newTutoAnim.transform.localPosition = firstTutorialAnimationsPos;
            newTutoAnim.transform.localRotation = firstTutorialAnimationsRot;

            currentTutorialAnimations = newTutoAnim;
        }
#endif

        if (!allPlayersInGame)
        {
            allPlayersInGame = true;
            for (int i = 0; i < 3; i++)
            {
                PlayersReadyToJoint p = players[i];
                if (!GameManager.Instance.IsPlayerInGame(p.player))
                {
                    allPlayersInGame = false;

                    if (p.input.GetButtonDown("EnterTutorial"))
                    {
                        GameManager.Instance.AddPlayerToGame(p.player);
                        GameManager.Instance.SetPlayerCharacterChoice(p.player, UnityEngine.Random.Range(1, 4));

                        PlayerManager.Instance.SpawnPlayer(p.player);
                    }
                }
            }
        }
        
        
        if (canGoToMenu)
        {
            skipPanel.SetActive(true);

            int rewirePlayerId = 1;
            var activePlayers = GameManager.Instance.GetActivePlayers();
            foreach (Player i in activePlayers)
            {
                switch (i)
                {
                    case Player.One: rewirePlayerId = 0; break;
                    case Player.Two: rewirePlayerId = 1; break;
                    case Player.Three: rewirePlayerId = 2; break;
                    case Player.Four: rewirePlayerId = 3; break;
                }

                var playerInput = ReInput.players.GetPlayer(rewirePlayerId);
                if (playerInput.GetButtonDown("ExitTutorial"))
                    GameManager.Instance.EndGameAndGoToMenu();
            }
        }
        
    }

    bool loading = false;
    public void GoToLA()
    {
        if (loading) return;

        loading = true;

        canGoToMenu = false;
        skipPanel.SetActive(false);

        var activePlayers = GameManager.Instance.GetActivePlayers();
        GameManager.Instance.RemoveAllPlayersFromGame();

        if(activePlayers.Count == 1)
        {
           // if (activePlayers[0] == Player.One)
                GameManager.Instance.AddPlayerToGame(Player.Four);
        }
        else
        {
            foreach (Player i in activePlayers)
                GameManager.Instance.AddPlayerToGame(i);
        }

        LoadIntroSceneLevel.introSceneLevel = introLevelNameAfterSkipTutorial;
        ShowLevelTitle.levelStaticInt = levelIntAfterSkipTutorial;
        SceneManager.LoadScene("LoadingRoom");
    }
}
