using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialEnter : MonoBehaviour
{
    public void LoadTutorial()
    {
        GameManager.Instance.AddPlayerToGame(Player.Four);

        ShowLevelTitle.levelStaticInt = 17;
        SceneManager.LoadScene("LoadingRoom");
    }
}
