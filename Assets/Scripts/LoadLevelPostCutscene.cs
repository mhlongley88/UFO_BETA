using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelPostCutscene : MonoBehaviour
{
    public static LoadLevelPostCutscene instance;

    private void Awake()
    {
        instance = this;
    }

    public void Load()
    {
        LoadIntroSceneLevel.introSceneLevel = "";
        SceneManager.LoadScene("LoadingRoom");

        //SceneManager.LoadScene(LevelLoad.lastLoadedLevel);
        //SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
    }

    void Update()
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

        if(forwardToLevelBtn)
            Load();
    }
}
