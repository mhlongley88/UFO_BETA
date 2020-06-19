using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCongratulationsBeat : MonoBehaviour
{
    public GameObject congratsPanel;
    public GameObject menuPanel;
    public GameObject tutorialButton;
    public Transform playerCharacterSpawnPoint;
    public Transform[] croniesSpawnPoints;
    bool verified = false;

    List<GameObject> characterInstances = new List<GameObject>();

    void Update()
    {
        if(!verified)
        {
            if (MainMenuUIManager.Instance.currentMenu == MainMenuUIManager.Menu.Splash || MainMenuUIManager.Instance.currentMenu == MainMenuUIManager.Menu.LevelSelect)
            {
                if (Boss.hadJustBeenDefeated)
                {
                    Boss.hadJustBeenDefeated = false;
                    congratsPanel.SetActive(true);
                    menuPanel.SetActive(false);
                    tutorialButton.SetActive(false);

                    var players = GameManager.Instance.GetActivePlayers();
                    var userPlayer = players.Find(it => !PlayerBot.chosenPlayer.Contains(it));

                    void SpawnCharacterOnEndScreen(Player player, Transform spawnPoint)
                    {
                        var characterPrefabIndex = GameManager.Instance.GetPlayerCharacterChoice(player);
                        var characterPrefab = GameManager.Instance.Characters[characterPrefabIndex].characterModel;

                        var playerCharacterInstance = Instantiate(characterPrefab, spawnPoint);
                        playerCharacterInstance.transform.localScale = Vector3.one;

                        characterInstances.Add(playerCharacterInstance);
                    }

                    SpawnCharacterOnEndScreen(userPlayer, playerCharacterSpawnPoint);

                    //int cronieSpawnIndex = 0;
                    foreach (var spawnPoint in croniesSpawnPoints)
                    {
                        SpawnCharacterOnEndScreen(userPlayer, spawnPoint);
                    }

                    verified = true;
                }
            }
        }
        else
        {
            bool forwardToLevelBtn = false;

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

                if (!forwardToLevelBtn) forwardToLevelBtn = playerInput.GetButtonDown("SkipCutscene");
            }

            if (congratsPanel.activeInHierarchy)
            {
                menuPanel.SetActive(false);
                tutorialButton.SetActive(false);
            }

            if (forwardToLevelBtn)
                Continue();
        }
    }

    public void Continue()
    {
        if (congratsPanel.activeInHierarchy)
        {
            congratsPanel.SetActive(false);

            if (characterInstances != null && characterInstances.Count > 0)
            {
                foreach(var p in characterInstances)
                    Destroy(p);
            }

            if (MainMenuUIManager.Instance.currentMenu == MainMenuUIManager.Menu.Splash)
            {
                menuPanel.SetActive(true);
                tutorialButton.SetActive(true);
            }
        }
    }
}
