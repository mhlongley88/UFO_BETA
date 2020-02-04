using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[System.Serializable]
public class PlayersReadyToJoint
{
    public Player player;
    public Rewired.Player input;
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public PlayersReadyToJoint[] players;
    public Animator instructionsAnimator;
    public bool canGoToMenu = false;
    bool allPlayersInGame = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            PlayersReadyToJoint p = players[i];

            int rewirePlayerId = 1;
            switch (p.player)
            {
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
        if (!allPlayersInGame)
        {
            allPlayersInGame = true;
            for (int i = 0; i < 3; i++)
            {
                PlayersReadyToJoint p = players[i];
                if (!GameManager.Instance.IsPlayerInGame(p.player))
                {
                    allPlayersInGame = false;

                    if (p.input.GetButtonDown("Submit"))
                    {
                        GameManager.Instance.AddPlayerToGame(p.player);
                        GameManager.Instance.SetPlayerCharacterChoice(p.player, UnityEngine.Random.Range(1, 4));

                        PlayerManager.Instance.SpawnPlayer(p.player);
                    }
                }
            }

            if (allPlayersInGame)
            {
                instructionsAnimator.SetTrigger("Play");
            }
        }
        else
        {
            if (canGoToMenu)
            {
                int playerCount = 0;
                var activePlayers = GameManager.Instance.GetActivePlayers();
                foreach (Player i in activePlayers)
                {
                    var playerInput = ReInput.players.GetPlayer(playerCount);
                    if (playerInput.GetButtonDown("GoToMainMenu"))
                        GameManager.Instance.EndGameAndGoToMenu();

                    playerCount++;
                }
            }
        }
    }
}
