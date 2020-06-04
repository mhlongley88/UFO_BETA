using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelPostCutscene : MonoBehaviour
{
    public void Load()
    {
        LoadIntroSceneLevel.introSceneLevel = "";

        SceneManager.LoadScene(LevelLoad.lastLoadedLevel);
        SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
    }
}
