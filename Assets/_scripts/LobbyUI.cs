using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyUI : MonoBehaviour
{
    public static LobbyUI instance;

    public GameObject CharacterSelectMul;

    public GameObject AuthPanel;

    //Sign In
    public Text userEmailTextSI;
    public Text userPasswordTextSI;
    
    //Sign Up
    public Text userEmailTextSU;
    public Text userPasswordTextSU;
    public Text usernameTextSU;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MatchMaking()
    {
        LobbyConnectionHandler.instance.StartMatchMaking();
    }

    public void EnterMultiplayerMode()
    {
        LobbyConnectionHandler.instance.IsMultiplayerMode = true;
    }

    public void LeaveMultiplayerMode()
    {
        LobbyConnectionHandler.instance.IsMultiplayerMode = false;
    }

}
