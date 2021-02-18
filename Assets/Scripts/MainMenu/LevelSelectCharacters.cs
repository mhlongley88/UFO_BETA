using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectCharacters : MonoBehaviour
{

    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform p4;
    public static LevelSelectCharacters instance;
    // Start is called before the first frame update

    private void Start()
    {
        instance = this;

    }

    public void AddActivePlayers()
    {

        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            AddPlayer(Player.One);
            AddPlayer(Player.Two);
            AddPlayer(Player.Three);
            AddPlayer(Player.Four);
        }
        else
        {
            AddPlayer(Player.None);
        }

    }
    private void RemoveChildren(Transform t)
    {
        foreach (Transform child in t)
        {
            Destroy(child.gameObject);
        }
    }
    public void RemoveAllPlayers()
    {
        RemoveChildren(p1);
        RemoveChildren(p2);
        RemoveChildren(p3);
        RemoveChildren(p4);
    }
    public GameObject myLevelPlayerMul;
    public void AddPlayer(Player player)
    {
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            if(PlayerBot.active)
            {
                if (PlayerBot.chosenPlayer.Contains(player)) return;
            }

            if (GameManager.Instance.IsPlayerInGame(player))
            {
                GameObject temp;
                switch (player)
                {
                    case Player.One:
                        temp = Instantiate(GameManager.Instance.GetPlayerModel(player), p1.position, p1.rotation, p1);

                        break;
                    case Player.Two:
                        temp = Instantiate(GameManager.Instance.GetPlayerModel(player), p2.position, p2.rotation, p2);
                        break;
                    case Player.Three:
                        temp = Instantiate(GameManager.Instance.GetPlayerModel(player), p3.position, p3.rotation, p3);
                        break;
                    case Player.Four:
                        temp = Instantiate(GameManager.Instance.GetPlayerModel(player), p4.position, p4.rotation, p4);
                        break;
                }

            }
        }
        else
        {
            Debug.Log("??");
            Player myplayerNumber = GameManager.Instance.GetMyPlayerIndexMul();

            switch (myplayerNumber)
            {
                case Player.One:
                    myLevelPlayerMul = Photon.Pun.PhotonNetwork.Instantiate(GameManager.Instance.GetPlayerModel(myplayerNumber).name, p1.position, p1.rotation);
                    myLevelPlayerMul.GetComponent<LevelSelectPlayers>().myPlayer = Player.One;
                    myLevelPlayerMul.GetComponent<LevelSelectPlayers>().SyncTransformsOnOtherInstances();
                    //myLevelPlayerMul.transform.SetParent(p1);
                    break;
                case Player.Two:
                    myLevelPlayerMul = Photon.Pun.PhotonNetwork.Instantiate(GameManager.Instance.GetPlayerModel(myplayerNumber).name, p1.position, p1.rotation);
                    myLevelPlayerMul.GetComponent<LevelSelectPlayers>().myPlayer = Player.Two;
                    myLevelPlayerMul.GetComponent<LevelSelectPlayers>().SyncTransformsOnOtherInstances();
                    //myLevelPlayerMul.transform.SetParent(p2);
                    break;
                case Player.Three:
                    myLevelPlayerMul = Photon.Pun.PhotonNetwork.Instantiate(GameManager.Instance.GetPlayerModel(myplayerNumber).name, p1.position, p1.rotation);
                    myLevelPlayerMul.GetComponent<LevelSelectPlayers>().myPlayer = Player.Three;
                    myLevelPlayerMul.GetComponent<LevelSelectPlayers>().SyncTransformsOnOtherInstances();
                    //myLevelPlayerMul.transform.SetParent(p3);
                    break;
                case Player.Four:
                    myLevelPlayerMul = Photon.Pun.PhotonNetwork.Instantiate(GameManager.Instance.GetPlayerModel(myplayerNumber).name, p1.position, p1.rotation);
                    myLevelPlayerMul.GetComponent<LevelSelectPlayers>().myPlayer = Player.Four;
                    myLevelPlayerMul.GetComponent<LevelSelectPlayers>().SyncTransformsOnOtherInstances();
                    //myLevelPlayerMul.transform.SetParent(p4);
                    break;
            }
        }
    }



}
