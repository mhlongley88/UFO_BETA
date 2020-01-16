using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public GameObject menuCanvasObj;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void Toggle()
    {
        GameManager.Instance.TogglePause();
        menuCanvasObj.SetActive(GameManager.Instance.paused);
    }

    public void ButtonQuitGame()
    {
        GameManager.Instance.TogglePause();
        Application.Quit();
    }
}
