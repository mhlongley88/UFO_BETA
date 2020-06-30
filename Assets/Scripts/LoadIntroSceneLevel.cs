using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadIntroSceneLevel : MonoBehaviour
{
    public static string introSceneLevel;

    public string levelName = "";
    ShowLevelTitle levelTitle;

    // Start is called before the first frame update
    void Start()
    {
        levelTitle = GetComponent<ShowLevelTitle>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ShowLevelTitle.levelStaticInt == levelTitle.levelNum && GameManager.Instance.GetActivePlayers().Count == 1)
        {
            introSceneLevel = levelName;
        }
        else
        {
            if (introSceneLevel == levelName)
                introSceneLevel = "";
        }
    }
}
