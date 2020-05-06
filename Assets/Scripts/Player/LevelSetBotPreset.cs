using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSetBot
{
    public AIPreset preset;
    public bool enableBot = false;
    public int characterIndex = 0;
}

public class LevelSetBotPreset : MonoBehaviour
{
    public LevelSetBot bot1;
    public LevelSetBot bot2;
    public LevelSetBot bot3;

   // public AIPreset preset;

    public ShowLevelTitle levelTitle;

    void Start()
    {
        bot1.enableBot = true;
    }

    void Update()
    {
        bot1.enableBot = true;

        if (ShowLevelTitle.levelStaticInt == levelTitle.levelNum)
        {
           // BotConfigurator.instance.currentPreset = preset;

            BotConfigurator.instance.bot1 = bot1;
            BotConfigurator.instance.bot2 = bot2;
            BotConfigurator.instance.bot3 = bot3;
        }
    }
}
