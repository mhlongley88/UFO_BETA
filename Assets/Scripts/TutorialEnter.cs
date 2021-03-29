using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialEnter : MonoBehaviour
{
    public ShowLevelTitle laLevelTitle;
    public LoadIntroSceneLevel laLoadIntro;
    public LevelUnlockFromProgression laLevelUnlockFromProgression;
    public LevelSetBotPreset laPreset;

    public void LoadTutorial()
    {
        BotConfigurator.instance.bot1 = laPreset.bot1;
        BotConfigurator.instance.bot2 = laPreset.bot2;
        BotConfigurator.instance.bot3 = laPreset.bot3;

        TutorialManager.levelIntAfterSkipTutorial = laLevelTitle.levelNum;
        TutorialManager.introLevelNameAfterSkipTutorial = laLoadIntro.levelName;
        laLevelUnlockFromProgression.SetThisProgression();

        if(GameManager.Instance.GetActivePlayers().Count == 0)
            GameManager.Instance.AddPlayerToGame(Player.Four);

        // Dont show intro now, just when going to LA really
        LoadIntroSceneLevel.introSceneLevel = "";

        ShowLevelTitle.levelStaticInt = 11;
        GameManager.Instance.enterTutorial = true;
        //GameManager.Instance.selectedLevelIndex = 11;
        SceneManager.LoadScene("LoadingRoom");
    }
}
