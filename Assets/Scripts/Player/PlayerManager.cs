using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    public int lives;
    public GameObject prefab;
    public Transform spawnPoint;
    public GameObject instance;

    public int rank;
    //public LifeManager lifeManager;

    // public int isAlive;
}

[Flags]
public enum Player
{
    None = 0,
    One = 1,
    Two = 2,
    Three = 4,
    Four = 8
}



public class PlayerManager : MonoBehaviour
{

    [Serializable]
    public class PlayerStatsDic : SerializableDictionaryBase<Player, PlayerStats> { }
    public PlayerStatsDic players;

    [Serializable]
    public class PlayerStatsDicMul : SerializableDictionaryBase<Player, PlayerStats> { }
    public PlayerStatsDicMul playersMul;


    public float spawnTimer = 5.0f;
    public bool debugP1join;
    public bool debugP2join;
    public bool debugP3join;
    public bool debugP4join;
    public Dictionary<Player,GameObject> spawnedPlayerDictionary = new Dictionary<Player,GameObject>();

    public bool gameHasEnded = false;

    private static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            return instance;
        }
    }


    public void Awake()
    {
        if (debugP1join == true)
        {
            GameManager.Instance.AddPlayerToGame(Player.One);
        }

        if (debugP2join == true)
        {
            GameManager.Instance.AddPlayerToGame(Player.Two);
        }
        if (debugP3join == true)
        {
            GameManager.Instance.AddPlayerToGame(Player.Three);
        }
        if (debugP4join == true)
        {
            GameManager.Instance.AddPlayerToGame(Player.Four);
        }

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    public void Start()
    {
        foreach (Player p in GameManager.Instance.GetActivePlayers())
        {
            LevelUIManager.Instance.ChangeLifeCount(p, players[p].lives);
        }
        if (gameHasEnded != true)
        {
            if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
            {
                SpawnLocalPlayers();
            }
            else
            {
                SpawnMulPlayer();
                //foreach (Player i in GameManager.Instance.GetActivePlayers())
                //{
                //    LevelUIManager.Instance.EnableUI(i);
                //    Instantiate(players[i].prefab, players[i].spawnPoint);
                //}
            }
        }
    }

    void SpawnLocalPlayers()
    {
        
            foreach (Player i in GameManager.Instance.GetActivePlayers())
            {
                LevelUIManager.Instance.EnableUI(i);
                players[i].instance = Instantiate(players[i].prefab, players[i].spawnPoint);

                spawnedPlayerDictionary.Add(i, players[i].instance);
            }
        
    }

    void SpawnMulPlayer()
    {
        foreach (Player i in GameManager.Instance.GetActivePlayersMul())
        {
            Debug.Log("Spawning11");
            LevelUIManager.Instance.EnableUI(i);

            GameObject temp = Photon.Pun.PhotonNetwork.Instantiate(playersMul[i].prefab.name, playersMul[i].spawnPoint.position, Quaternion.identity);
            //temp.GetComponent<PlayerController>().enabled = true;
            temp.transform.SetParent(players[i].spawnPoint);
        }
    }

    public int GetPlayersLeft()
    {
        int playersLeft = 0;
        foreach (Player i in GameManager.Instance.GetActivePlayers())
        {
            if (players[i].lives >= 0)
            {
                playersLeft++;
            }
        }
        return playersLeft;
    }

    public void PlayerDied(Player player, Transform playerModel)
    {
        if (GameManager.Instance.gameOver) return;

        //GameManagerScript.Instance.PlayerDied(player);

        int currentLife = players[player].lives;

        bool canRespawn = currentLife >= 1;

        players[player].lives--;
        LevelUIManager.Instance.ChangeLifeCount(player, players[player].lives);

        int playersLeft = GetPlayersLeft();
        players[player].rank = playersLeft;
        spawnedPlayerDictionary.Remove(player);

        if (canRespawn)
        {
            StartCoroutine(SpawnCoroutine(player));
        }
        else if (playersLeft == 1)
        {
            foreach (Player i in GameManager.Instance.GetActivePlayers())
            {
                RankingPostGame.instance.SubmitPlayer(players[i].rank, GameManager.Instance.GetPlayerModel(i));
            }

            GameManager.Instance.GameEnds();
        }
    }

    private IEnumerator SpawnCoroutine(Player player)
    {
        yield return new WaitForSeconds(spawnTimer);
        if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            SpawnMulPlayer();
        }
        else
        {
            players[player].instance = Instantiate(players[player].prefab, players[player].spawnPoint);
        }

        spawnedPlayerDictionary.Add(player, players[player].instance);
    }

}
