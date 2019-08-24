using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectCharacters : MonoBehaviour
{

    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform p4;

    // Start is called before the first frame update
    public void AddActivePlayers()
    {

        AddPlayer(Player.One);
        AddPlayer(Player.Two);
        AddPlayer(Player.Three);
        AddPlayer(Player.Four);

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

    private void AddPlayer(Player player)
    {
        if(GameManager.Instance.IsPlayerInGame(player))
        {
            switch (player)
            {
                case Player.One:
                        Instantiate(GameManager.Instance.GetPlayerModel(player), p1.position, p1.rotation, p1);
                    break;
                case Player.Two:
                        Instantiate(GameManager.Instance.GetPlayerModel(player), p2.position, p2.rotation, p2);
                    break;
                case Player.Three:
                        Instantiate(GameManager.Instance.GetPlayerModel(player), p3.position, p3.rotation, p3);
                    break;
                case Player.Four:
                        Instantiate(GameManager.Instance.GetPlayerModel(player), p4.position, p4.rotation, p4);
                    break;
            }
        }
    }
}
