using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using System.Linq;

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
        if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            foreach (Player p in GameManager.Instance.GetActivePlayersMul(false))
            {
                LevelUIManager.Instance.ChangeLifeCount(p, players[p].lives);
            }

        }
        else
        {
            foreach (Player p in GameManager.Instance.GetActivePlayers())
            {
                LevelUIManager.Instance.ChangeLifeCount(p, players[p].lives);
            }
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

    public void SpawnPlayer(Player p)
    {
        LevelUIManager.Instance.EnableUI(p);

        players[p].instance = Instantiate(players[p].prefab, players[p].spawnPoint);
        players[p].rank = -1;
        spawnedPlayerDictionary.Add(p, players[p].instance);
    }

    void SpawnLocalPlayers()
    {
        Player lastPlayerSpawned = Player.Three;
        var activePlayers = GameManager.Instance.GetActivePlayers();

        if (TutorialManager.instance != null)
            PlayerBot.active = false;

        if (PlayerBot.active)
        {
            for (int i = 0; i < PlayerBot.chosenPlayer.Count; i++)
            {
                GameManager.Instance.SetPlayerCharacterChoice(PlayerBot.chosenPlayer[i], UnityEngine.Random.Range(1, 6));
            }
        }

        foreach (Player i in activePlayers)
        {
            SpawnPlayer(i);

            lastPlayerSpawned = i;

            if(PlayerBot.active && PlayerBot.chosenPlayer.Contains(i))
            {
                var bot =  players[i].instance.AddComponent<PlayerBot>();
                bot.preset = PlayerBot.aiPresets[PlayerBot.chosenPlayer.IndexOf(i)];
            }
        }

        if (TutorialManager.instance != null) return;

        if (activePlayers.Count == 1) //If it started with one player, activates the bot, if there is a rematch it will just respawn the player that the bot is controlling
        {
            PlayerBot.active = true;

            List<Player> possiblePlayers = new List<Player>();
            possiblePlayers.Add(Player.One);
            possiblePlayers.Add(Player.Two);
            possiblePlayers.Add(Player.Three);
            possiblePlayers.Add(Player.Four);

            possiblePlayers.Remove(lastPlayerSpawned);

            Player botPlayer = Player.One;
            int playerIndex = (int)botPlayer;
            while(playerIndex == (int)lastPlayerSpawned)
            {
                playerIndex *= 2;
                if (playerIndex >= (int)Player.Four)
                    playerIndex = 1;
            }

            PlayerBot.chosenPlayer.Clear();
            PlayerBot.aiPresets.Clear();

            if (BotConfigurator.instance.bot1.enableBot)
                AddBotP(BotConfigurator.instance.bot1.preset);        
            
            if (BotConfigurator.instance.bot2.enableBot)
                AddBotP(BotConfigurator.instance.bot2.preset);

            if (BotConfigurator.instance.bot3.enableBot)   
                AddBotP(BotConfigurator.instance.bot3.preset);           

            void AddBotP(AIPreset preset)
            {
                int index = UnityEngine.Random.Range(0, possiblePlayers.Count);
                botPlayer = possiblePlayers[index];
                possiblePlayers.RemoveAt(index);

                PlayerBot.chosenPlayer.Add(botPlayer);
                PlayerBot.aiPresets.Add(preset);

                GameManager.Instance.AddPlayerToGame(botPlayer);
                GameManager.Instance.SetPlayerCharacterChoice(botPlayer, UnityEngine.Random.Range(1, 6));

                LevelUIManager.Instance.EnableUI(botPlayer);
                players[botPlayer].instance = Instantiate(players[botPlayer].prefab, players[botPlayer].spawnPoint);
                players[botPlayer].rank = -1;

                var bot = players[botPlayer].instance.AddComponent<PlayerBot>();
                bot.preset = preset;

                spawnedPlayerDictionary.Add(botPlayer, players[botPlayer].instance);
            }
        }
    }

    void SpawnMulPlayer()
    {
        //Debug.Log("Spawning:  " + GameManager.Instance.GetActivePlayersMul(true)[0]);
        foreach (Player i in GameManager.Instance.GetActivePlayersMul(true))
        {
            Debug.Log("Spawning11" + playersMul[i].prefab.name);
            

            GameObject temp = Photon.Pun.PhotonNetwork.Instantiate(playersMul[i].prefab.name, playersMul[i].spawnPoint.position, Quaternion.identity);
           // temp.tag = "Player";
            //temp.GetComponent<PlayerController>().enabled = true;
            temp.transform.SetParent(players[i].spawnPoint);

            players[i].instance = temp;
            if (spawnedPlayerDictionary.ContainsKey(i))
                spawnedPlayerDictionary[i] = players[i].instance;
            else
                spawnedPlayerDictionary.Add(i, players[i].instance);

            //spawnedPlayerDictionary.Add(i, players[i].instance);

            Cursor.visible = true;
            
        }
    }

    public int GetPlayersLeft()
    {
        int playersLeft = 0;
        if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            //PlayerController[] temp = FindObjectsOfType<PlayerController>();
            //playersLeft = temp.Length;
            foreach (Player i in GameManager.Instance.GetActivePlayersMul(false))
            {
                Debug.Log("GetPlayerLeft" + players[i].lives + "---" + i.ToString());
                if (players[i].lives > 0)
                {
                    Debug.Log("Alive player spotted" + players[i].lives + "---" + i.ToString());
                    playersLeft++;
                }
            }

        }
        else
        {
            foreach (Player i in GameManager.Instance.GetActivePlayers())
            {
                if (players[i].lives > 0)
                {
                    playersLeft++;
                }
            }
        }
      //  Debug.Log("GetPlayerLeft" + playersLeft);
        return playersLeft;
    }

    public void PlayerDied(Player player, Transform playerModel)
    {
        if (GameManager.Instance.gameOver) return;

        //GameManagerScript.Instance.PlayerDied(player);

        int currentLife = players[player].lives;

        bool canRespawn = currentLife > 1;
        //Debug.Log(player + "???????");
        players[player].lives--;

        if (TutorialManager.instance != null)
            players[player].lives++;

       // Debug.Log(players[player].lives + "???????");
        LevelUIManager.Instance.ChangeLifeCount(player, players[player].lives);
        Debug.Log("Lifes Left = " + players[player].lives);

        int playersLeft = GetPlayersLeft();
        Debug.Log("Players Left = " + playersLeft);

        players[player].rank = playersLeft;
        spawnedPlayerDictionary.Remove(player);

        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            if (playersLeft < 2)
            {
                var lastPlayerAlive = spawnedPlayerDictionary.Keys.ToList()[0];
                players[lastPlayerAlive].rank = 0;
            }
        }
        else if (playersLeft < 2 && spawnedPlayerDictionary.Count == 1)
        {
            var lastPlayerAlive = spawnedPlayerDictionary.Keys.ToList()[0];
            players[lastPlayerAlive].rank = 0;
        }

        //Debug.Log(playerModel.gameObject.name);

        // Online handling
        if (LobbyConnectionHandler.instance.IsMultiplayerMode && playerModel.gameObject.GetComponentInParent<Photon.Pun.PhotonView>().IsMine && canRespawn)
        {
            //Photon.Pun.PhotonNetwork.Destroy(playerModel.gameObject.GetComponentInParent<Photon.Pun.PhotonView>().gameObject);
            //playerModel.gameObject.GetComponentInParent<Photon.Pun.PhotonView>().RPC("Death", Photon.Pun.RpcTarget.All);
            StartCoroutine(SpawnCoroutine(player));
        }
        else
        if (LobbyConnectionHandler.instance.IsMultiplayerMode && playersLeft < 2)
        {
            foreach (Player i in GameManager.Instance.GetActivePlayersMul(false))
            {
                RankingPostGame.instance.SubmitPlayer(players[i].rank, GameManager.Instance.GetPlayerModel(i));
                Debug.Log("Rank of " + i + " :  " + players[i].rank);
            }

            GameManager.Instance.GameEnds();
        }

        //Offline handling
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode &&  canRespawn)
        {
            StartCoroutine(SpawnCoroutine(player));
        }
        else if (!LobbyConnectionHandler.instance.IsMultiplayerMode && playersLeft < 2)
        {
            var activePlayers = GameManager.Instance.GetActivePlayers();
            foreach (Player i in activePlayers)
            {
                Debug.Log("Rank of " + i + " :  " + players[i].rank);
                RankingPostGame.instance.SubmitPlayer(players[i].rank, GameManager.Instance.GetPlayerModel(i));

                if (PlayerBot.active)
                {
                    if (!PlayerBot.chosenPlayer.Contains(i))
                    {
                        if (players[i].rank == 0)
                        {
                            if (DoubleMatch.useDoubleMatch)
                            {
                                for (int e = 0; e < PlayerBot.chosenPlayer.Count; e++)
                                {
                                    var p = PlayerBot.chosenPlayer[e];
                                    players[p].rank = -1;
                                    players[p].lives = 3;
                                    LevelUIManager.Instance.ChangeLifeCount(p, players[p].lives);

                                    StartCoroutine(SpawnCoroutine(p));
                                }

                                DoubleMatch.useDoubleMatch = false;
                                return;
                            }

                            PostGameOptionsRetry.instance.retryMatchText.SetActive(false);
                            PostGameOptionsRetry.instance.nextLevelMatchText.SetActive(true);

                            GameManager.Instance.goesNextLevelInsteadOfRetry = true;


                            if (LevelUnlockFromProgression.lastSelected != -1)
                            {
                                LevelUnlockFromProgression.UnlockLevel();
                            }
                        }
                    }
                }
            }

            GameManager.Instance.GameEnds();      
        }
        else if (!LobbyConnectionHandler.instance.IsMultiplayerMode && PlayerBot.active)
        {
            bool allTheActivePlayersAreBots = true;
            var activePlayers = GameManager.Instance.GetActivePlayers();
            foreach (Player i in activePlayers)
            {       
                // RankingPostGame.instance.SubmitPlayer(players[i].rank, GameManager.Instance.GetPlayerModel(i));
                if (players[i].lives > 0)
                {
                    if (!PlayerBot.chosenPlayer.Contains(i))
                    {
                        allTheActivePlayersAreBots = false;
                    }
                }
            }

            if(allTheActivePlayersAreBots)
            {
                // LevelUIManager.Instance.lostToBots.SetActive(true);

                int rank = 0;
                foreach (Player i in activePlayers)
                {
                    Debug.Log("Rank of " + i + " :  " + players[i].rank);

                    if (players[i].rank < 0)
                    {
                        players[i].rank = rank++;
                    }

                    RankingPostGame.instance.SubmitPlayer(players[i].rank, GameManager.Instance.GetPlayerModel(i));
                }

              
                GameManager.Instance.GameEnds();
            }
        }
    }

    private IEnumerator SpawnCoroutine(Player player)
    {
        yield return new WaitForSeconds(spawnTimer);
        if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            SpawnMulPlayer();Debug.Log("MulRespawn");
        }
        else
        {
            players[player].instance = Instantiate(players[player].prefab, players[player].spawnPoint); Debug.Log("OfflineRespawn");
            if (PlayerBot.chosenPlayer.Contains(player) && PlayerBot.active)
            {
                var bot = players[player].instance.AddComponent<PlayerBot>();
                bot.preset = PlayerBot.aiPresets[PlayerBot.chosenPlayer.IndexOf(player)];
            }

            if (spawnedPlayerDictionary.ContainsKey(player))
                spawnedPlayerDictionary[player] = players[player].instance;
            else
                spawnedPlayerDictionary.Add(player, players[player].instance);
        }

    }

}
