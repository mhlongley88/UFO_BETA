using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetBotPreset : MonoBehaviour
{
    public AIPreset preset;
    public ShowLevelTitle levelTitle;

    void Start()
    {
        
    }

    void Update()
    {
        if(ShowLevelTitle.levelStaticInt == levelTitle.levelNum)
        {
            BotConfigurator.instance.currentPreset = preset;
        }
    }
}
