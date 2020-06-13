using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCongratulationsBeat : MonoBehaviour
{
    public GameObject congratsPanel;
    public GameObject menuPanel;
    public Transform playerCharacterSpawnPoint;
    bool verified = false;

    GameObject playerCharacterInstance;

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

                    var players = GameManager.Instance.GetActivePlayers();
                    var userPlayer = players.Find(it => !PlayerBot.chosenPlayer.Contains(it));
                    var characterPrefabIndex = GameManager.Instance.GetPlayerCharacterChoice(userPlayer);
                    var characterPrefab = GameManager.Instance.Characters[characterPrefabIndex].characterModel;

                    playerCharacterInstance = Instantiate(characterPrefab, playerCharacterSpawnPoint);
                    playerCharacterInstance.transform.localScale = Vector3.one;

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

            if(playerCharacterInstance != null)
                Destroy(playerCharacterInstance);

            if (MainMenuUIManager.Instance.currentMenu == MainMenuUIManager.Menu.Splash)
                menuPanel.SetActive(true);
        }
    }
}
