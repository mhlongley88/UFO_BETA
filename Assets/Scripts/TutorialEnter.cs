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

        GameManager.Instance.AddPlayerToGame(Player.One);

        ShowLevelTitle.levelStaticInt = 17;
        SceneManager.LoadScene("LoadingRoom");
    }
}
