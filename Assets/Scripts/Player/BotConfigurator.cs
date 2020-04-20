using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotConfigurator : MonoBehaviour
{
    public static BotConfigurator instance;

    public LevelSetBot bot1;
    public LevelSetBot bot2;
    public LevelSetBot bot3;

    //public AIPreset currentPreset;

    [Header("Presets")]
    public AIPreset easyPreset;
    public AIPreset medPreset;
    public AIPreset hardPreset;
    public AIPreset veryHardPreset;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
