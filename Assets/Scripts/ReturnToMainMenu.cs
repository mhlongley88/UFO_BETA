using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToMainMenu()
    {
        if (GameManager.Instance.paused)
            GameManager.Instance.TogglePause();
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        { 
            Application.LoadLevel(0);
        }
        else
        {
            GameManager.Instance.EndGameAndGoToMenu();
        }
    }
}
