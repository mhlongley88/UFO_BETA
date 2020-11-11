using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.Jobs;

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

    public Texture[] ConqueredMaterialTextures;
    public Sprite[] ConqueredMaterialSprites;
    public Material conqueredMaterial;
    public SpriteRenderer ConqueredTextSprite;

    public bool gameHasEnded = false;

    private static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            return instance;
        }
    }

    bool useDoubleMatch;

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

        Cursor.lockState = CursorLockMode.Confined;
      //  Cursor.visible = false;
    }
    public void Start()
    {
        useDoubleMatch = DoubleMatch.useDoubleMatch;
        ConqueredMaterialTextures = GameManager.Instance.ConqueredMaterialTextures;
        ConqueredMaterialSprites = GameManager.Instance.ConqueredMaterialSprites;
        //ConqueredTextSprite = GameObject.Find("Conquered_text_Sprite").GetComponent<SpriteRenderer>();
        SetConqueredMaterialTexture();

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
        UserPrefs.instance.SetInt("UFOPlayedLevel" + GameManager.Instance.selectedLevelIndex, 1);
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
                switch(PlayerBot.chosenPlayer[i])
                {
                    case Player.One:
                        GameManager.Instance.SetPlayerCharacterChoice(PlayerBot.chosenPlayer[i], BotConfigurator.instance.bot1.isRandomCharacter ? UnityEngine.Random.Range(0, 6) : BotConfigurator.instance.bot1.characterIndex);
                        break;
                    case Player.Two:
                        GameManager.Instance.SetPlayerCharacterChoice(PlayerBot.chosenPlayer[i], BotConfigurator.instance.bot2.isRandomCharacter ? UnityEngine.Random.Range(0, 6) : BotConfigurator.instance.bot1.characterIndex);
                        break;
                    case Player.Three:
                        GameManager.Instance.SetPlayerCharacterChoice(PlayerBot.chosenPlayer[i], BotConfigurator.instance.bot3.isRandomCharacter ? UnityEngine.Random.Range(0, 6) : BotConfigurator.instance.bot1.characterIndex);
                        break;
                }

                
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
            PlayerBot.aiSlots.Clear();

            if (BotConfigurator.instance.bot1.enableBot)
                AddBotP(BotConfigurator.instance.bot1.preset, BotConfigurator.instance.bot1.isRandomCharacter ? UnityEngine.Random.Range(0, 6) : BotConfigurator.instance.bot1.characterIndex, PlayerBotSlot.One);        
            
            if (BotConfigurator.instance.bot2.enableBot)
                AddBotP(BotConfigurator.instance.bot2.preset, BotConfigurator.instance.bot2.isRandomCharacter ? UnityEngine.Random.Range(0, 6) : BotConfigurator.instance.bot2.characterIndex, PlayerBotSlot.Two);

            if (BotConfigurator.instance.bot3.enableBot)   
                AddBotP(BotConfigurator.instance.bot3.preset, BotConfigurator.instance.bot3.isRandomCharacter ? UnityEngine.Random.Range(0, 6) : BotConfigurator.instance.bot3.characterIndex, PlayerBotSlot.Three);           

            void AddBotP(AIPreset preset, int characterIndex, PlayerBotSlot slot)
            {
                int index = UnityEngine.Random.Range(0, possiblePlayers.Count);
                botPlayer = possiblePlayers[index];
                possiblePlayers.RemoveAt(index);

                PlayerBot.chosenPlayer.Add(botPlayer);
                PlayerBot.aiPresets.Add(preset);
                PlayerBot.aiSlots.Add(slot);

                GameManager.Instance.AddPlayerToGame(botPlayer);
                GameManager.Instance.SetPlayerCharacterChoice(botPlayer, characterIndex);

                LevelUIManager.Instance.EnableUI(botPlayer);
                players[botPlayer].instance = Instantiate(players[botPlayer].prefab, players[botPlayer].spawnPoint);
                players[botPlayer].rank = -1;

                var bot = players[botPlayer].instance.AddComponent<PlayerBot>();
                bot.preset = preset;
                bot.slot = slot;
                spawnedPlayerDictionary.Add(botPlayer, players[botPlayer].instance);
            }
        }
    }

    void SpawnMulPlayer()
    {
        //Debug.Log("Spawning:  " + GameManager.Instance.GetActivePlayersMul(true)[0]);
        foreach (Player i in GameManager.Instance.GetActivePlayersMul(true))
        {
            GameObject temp = Photon.Pun.PhotonNetwork.Instantiate(players[i].prefab.name, players[i].spawnPoint.position, Quaternion.identity);
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

    public Player GetLastAlivePlayer_Offline()
    {
        Player p = Player.None;
        foreach (Player i in GameManager.Instance.GetActivePlayers())
        {
            if (players[i].lives > 0)
            {
                p = i;
                break;
            }
        }

        //if(p)//all yours! 

        return p;
    }
    public Player GetLastAlivePlayer_Online()
    {
        Player p = Player.None;

        foreach (Player i in GameManager.Instance.GetActivePlayersMul(false))
        {
            //Debug.Log("GetPlayerLeft" + players[i].lives + "---" + i.ToString());
            if (players[i].lives > 0)
            {
                p = i;
                break;
            }
        }

        return p;
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

    public void BossHasDied()
    {
        int playersLeft = GetPlayersLeft();
        if (playersLeft >= 2) return;

        var lastPlayerAlive = spawnedPlayerDictionary.Keys.ToList()[0];
        players[lastPlayerAlive].rank = 0;

        // Last player to kill the boss was a bot, does not count
        if (PlayerBot.chosenPlayer.Contains(lastPlayerAlive)) return;

        if (hasDoubleMatch())
            return;

        PostGameOptionsRetry.instance.retryMatchText.SetActive(false);
        PostGameOptionsRetry.instance.nextLevelMatchText.SetActive(true);

        GameManager.Instance.goesNextLevelInsteadOfRetry = true;

        if (LevelUnlockFromProgression.lastSelected != -1)
        {
            LevelUnlockFromProgression.UnlockLevel();
        }

        if (CharacterUnlockFromProgression.lastSelected != -1)
        {
            CharacterUnlockFromProgression.UnlockCharacter();
        }

        var activePlayers = GameManager.Instance.GetActivePlayers();
        foreach (Player i in activePlayers)
        {
            Debug.Log("Rank of " + i + " :  " + players[i].rank);
            RankingPostGame.instance.SubmitPlayer(players[i].rank, GameManager.Instance.GetPlayerModel(i));
        }

        DOVirtual.DelayedCall(Boss.instance.delayToShowDeathAnim, () =>
            {
                GameManager.Instance.GameEnds();
            });
    }

    bool hasDoubleMatch()
    {
        if (useDoubleMatch)
        {
            for (int e = 0; e < PlayerBot.chosenPlayer.Count; e++)
            {
                var p = PlayerBot.chosenPlayer[e];
                var lifeManager = LevelUIManager.Instance.GetLifeManager(p);
                lifeManager.gameObject.SetActive(false);
            }

            DoubleMatchCutsceneRef.instance.cutscene.SetActive(true);

            LevelUIManager.Instance.allInvincible = true;
            var seq = DOTween.Sequence();
            seq.AppendInterval(DoubleMatchCutsceneRef.instance.disableMe.seconds);
            seq.AppendCallback(() =>
            {
                LevelUIManager.Instance.allInvincible = false;

                for (int e = 0; e < PlayerBot.chosenPlayer.Count; e++)
                {
                    var p = PlayerBot.chosenPlayer[e];
                    players[p].rank = -1;
                    players[p].lives = 3;

                    var lifeManager = LevelUIManager.Instance.GetLifeManager(p);
                    lifeManager.ChangeLifeCount(players[p].lives);
                    lifeManager.gameObject.SetActive(true);

                    int characterOverrideIndex = -1;
                    switch (PlayerBot.aiSlots[e])
                    {
                        case PlayerBotSlot.One:
                            {
                                //if (!BotConfigurator.instance.bot1.isRandomCharacter)
                                //    characterOverrideIndex = UnityEngine.Random.Range(0, 7);
                                //else
                                    characterOverrideIndex = DoubleMatch.lastSelected.bot1CharacterIndex;
                            }
                            break;
                        case PlayerBotSlot.Two:
                            {
                                //if (!BotConfigurator.instance.bot2.isRandomCharacter)
                                //    characterOverrideIndex = UnityEngine.Random.Range(0, 7);
                                //else
                                    characterOverrideIndex = DoubleMatch.lastSelected.bot2CharacterIndex;
                            }
                            break;
                        case PlayerBotSlot.Three:
                            {
                               
                                    characterOverrideIndex = DoubleMatch.lastSelected.bot3CharacterIndex;
                            }
                            break;
                    }

                    if (characterOverrideIndex != -1)
                        GameManager.Instance.SetPlayerCharacterChoice(p, characterOverrideIndex);

                    StartCoroutine(SpawnCoroutine(p));

                }
            });


            useDoubleMatch = false;
            return true;
        }

        return false;
    }

    private void Update()
    {
        Debug.Log(spawnedPlayerDictionary.Count + "*************************");
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
        //Debug.Log("Lifes Left = " + players[player].lives);

        int playersLeft = GetPlayersLeft();
        // Debug.Log("Players Left = " + playersLeft);

        players[player].rank = playersLeft;
        
        spawnedPlayerDictionary.Remove(player);
        
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            if (playersLeft > 0 && playersLeft < 2)
            {
                if (spawnedPlayerDictionary.Count > 0)
                {
                    Player lastPlayerAlive = spawnedPlayerDictionary.Keys.ToList()[0];
                    
                    players[lastPlayerAlive].rank = 0;
                    Debug.Log(spawnedPlayerDictionary.Keys.ToList()[0] + "########");
                }
            }
        }
        else if (playersLeft < 2 /*&& spawnedPlayerDictionary.Count == 1*/)
        {
            
            var lastPlayerAlive = GetLastAlivePlayer_Online();//spawnedPlayerDictionary.Keys.ToList()[0];
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
                //Debug.Log("Final Rank of " + i + " is: " + players[i].rank);
                RankingPostGame.instance.SubmitPlayer(players[i].rank, GameManager.Instance.GetPlayerModel(i));
                //Debug.Log("Rank of " + i + " :  " + players[i].rank);
            }

            UnlockSystem.instance.SaveOnlineMatchesCompleted();

            GameManager.Instance.GameEnds();
        }

        bool allTheActivePlayersAreBots = true;
        var activePlayers = GameManager.Instance.GetActivePlayers();

        if (PlayerBot.active)
        {
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
        }

        //Offline handling
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode && canRespawn)
        {
            StartCoroutine(SpawnCoroutine(player));
        }
        else if (!LobbyConnectionHandler.instance.IsMultiplayerMode && (playersLeft < 2 || (PlayerBot.active && allTheActivePlayersAreBots) ))
        {
          //  var activePlayers = GameManager.Instance.GetActivePlayers();

            // There must be a non bot player alive to check for the boss, imagine the player died on a fight versus a bot and there is a boss
            // if the boss is alive and the only player left is a bot, the game is over
            var nonBotPlayer = players.Where(it => !PlayerBot.chosenPlayer.Contains(it.Key) && it.Value.lives >= 1 && it.Value.instance != null).ToList();

            // If the player is dead and the boss was or wasnt defeated, make sure it doesnt unlock the level for local/online


            if (nonBotPlayer.Count <= 0 && Boss.instance != null && !Boss.defeatedBefore)
            {
                if (Boss.instance != null && (Boss.instance.health > 0 || Boss.instance.health <= 0) )
                    LevelUnlockCheck.ResetUnlockByBoss(ShowLevelTitle.levelStaticInt);

                LevelUIManager.Instance.lostToBots.SetActive(true);
                return;
            }
         

            if(Boss.instance != null && !LevelUnlockCheck.IsUnlockedByBoss(ShowLevelTitle.levelStaticInt) && nonBotPlayer.Count > 0)
            {
                // Boss still got health and there is one more player left
                if (Boss.instance.health > 0 && playersLeft >= 1)
                    return;
            }


            var onlyNonBotPlayer = players.FirstOrDefault(it => !PlayerBot.chosenPlayer.Contains(it.Key)).Value;
            int[] ranks = new int[4] { 0, 1, 2, 3 };
            int rankIndex = 0;

            foreach (Player i in activePlayers)
            {
                if (PlayerBot.active)
                {
                    if (nonBotPlayer.Count <= 0)
                    {
                        // Dont change the player rank, let it where he died and dont change the winner bot rank
                        //if (players[i] != onlyNonBotPlayer)
                        //{
                        //    if (ranks[rankIndex] == onlyNonBotPlayer.rank) rankIndex++;//1

                        //    if (rankIndex >= ranks.Length) break;

                        //    //players[i].rank = ranks[rankIndex++];//2
                        //}

                        if (spawnedPlayerDictionary.ContainsKey(i) &&
                            spawnedPlayerDictionary[i].GetComponent<PlayerController>() &&
                            spawnedPlayerDictionary[i].GetComponent<PlayerBot>())
                        {
                            //if (ranks[rankIndex] == onlyNonBotPlayer.rank) rankIndex++;

                            if (rankIndex >= ranks.Length) break;

                            players[i].rank = ranks[rankIndex++];
                            Debug.Log(i + " rank is " + players[i].rank);
                        }
                    }
     
                    if(Boss.instance != null && !LevelUnlockCheck.IsUnlockedByBoss(ShowLevelTitle.levelStaticInt))
                    {
                        // Check if player died and boss still got health, that means player lost
                        if (Boss.instance.health > 0 && nonBotPlayer.Count <= 0)
                        {
                            GameManager.Instance.GameEnds();
                            return;
                        }
                        else if(nonBotPlayer.Count > 1)
                        {
                            return;
                        }
                    }

                    if (nonBotPlayer.Count > 0)
                    {
                        if (hasDoubleMatch())
                            return;
                    }

                    if (!PlayerBot.chosenPlayer.Contains(i))
                    {
                        if (players[i].rank == 0)
                        {

                            PostGameOptionsRetry.instance.retryMatchText.SetActive(false);
                            PostGameOptionsRetry.instance.nextLevelMatchText.SetActive(true);

                            GameManager.Instance.goesNextLevelInsteadOfRetry = true;

                            if (LevelUnlockFromProgression.lastSelected != -1)
                            {
                                LevelUnlockFromProgression.UnlockLevel();
                            }

                             if (CharacterUnlockFromProgression.lastSelected != -1)
                            {
                                CharacterUnlockFromProgression.UnlockCharacter();
                            }
                        }
                    }
                    else if(nonBotPlayer.Count <= 0)
                    {
                        //if (players[i].rank == -1)
                        {
                        //    players[i].rank = fakeRankCount++;
                        }
                    }
                }

               // Debug.Log("Rank of " + i + " :  " + players[i].rank);
                RankingPostGame.instance.SubmitPlayer(players[i].rank, GameManager.Instance.GetPlayerModel(i));
            }

           // if(!PlayerBot.active && activePlayers.Count == 4) //Full 4 player local multiplayer match
              //  SteamGameAchievements.instance.UnlockAchievement(SteamGameAchievements.Achievement.UFO_ACHIEVEMENT_1_5);

            if (GameManager.Instance.isLocalSPMode)
            {
                bool gameWon = allTheActivePlayersAreBots ? false : true;
                
                GameManager.Instance.GameEnds(gameWon);
            }
            else
            {
                GameManager.Instance.GameEnds();
            }
        }
        else if (!LobbyConnectionHandler.instance.IsMultiplayerMode && PlayerBot.active && allTheActivePlayersAreBots)
        {
            //if(allTheActivePlayersAreBots)
            {
                LevelUIManager.Instance.lostToBots.SetActive(true);
                // I tested the game yesterday. 1 and 2 worked all good. Game really never came into this if block. I oommented update to ranks just in case(3 and 4)
                //You can test now!
                int rank = 0;
                foreach (Player i in activePlayers)
                {
                    Debug.Log("Rank of " + i + " :  " + players[i].rank);

                    //if (players[i].rank < 0)
                    if(PlayerBot.chosenPlayer.Contains(i))
                    {
                       // players[i].rank = rank++; //3
                    }

                    //RankingPostGame.instance.SubmitPlayer(players[i].rank, GameManager.Instance.GetPlayerModel(i));//4
                }

                // Make sure tthat if the boss was defeated by the player and the player lost to the bots, dont unlock the level for local/online
                if (Boss.instance != null)
                {
                    LevelUnlockCheck.ResetUnlockByBoss(ShowLevelTitle.levelStaticInt);
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

    void SetConqueredMaterialTexture()//#1
    {
        conqueredMaterial = Resources.Load("Boom MAT Text RY", typeof(Material)) as Material;
        //This will set Boom mat texture. Just update language names in these two functions- you will be good. 2 functions in this script!
        switch (GameManager.Instance.selectedLanguage)
        {
            case "English":
                conqueredMaterial.mainTexture = ConqueredMaterialTextures[0];
                break;
            case "中文":
                conqueredMaterial.mainTexture = ConqueredMaterialTextures[1];
                break;
            case "Français":
                conqueredMaterial.mainTexture = ConqueredMaterialTextures[2];
                break;
            case "Deutsche":
                conqueredMaterial.mainTexture = ConqueredMaterialTextures[3];
                break;
            case "Italiano":
                conqueredMaterial.mainTexture = ConqueredMaterialTextures[4];
                break;
            case "Português":
                conqueredMaterial.mainTexture = ConqueredMaterialTextures[5];
                break;
            case "Pусский":
                conqueredMaterial.mainTexture = ConqueredMaterialTextures[6];
                break;
            case "Español":
                conqueredMaterial.mainTexture = ConqueredMaterialTextures[7];
                break;

            case "Polski":
                conqueredMaterial.mainTexture = ConqueredMaterialTextures[8];
                break;
            case "Nederlands":
                conqueredMaterial.mainTexture = ConqueredMaterialTextures[9];
                break;


        }
    }
    public void SetConqueredSpriteText(SpriteRenderer sr)//#2
    {
        // Update language names in native language - This will set Conquered sprites
        switch (GameManager.Instance.selectedLanguage)
        {
            case "English":
                sr.sprite = ConqueredMaterialSprites[0];
                break;
            case "中文":
                sr.sprite = ConqueredMaterialSprites[1];
                break;
            case "Français":
                sr.sprite = ConqueredMaterialSprites[2];
                break;
            case "Deutsche":// Update this to Deutsche
                sr.sprite = ConqueredMaterialSprites[3];
                break;
            case "Italiano":
                sr.sprite = ConqueredMaterialSprites[4];
                break;
            case "Português":
                sr.sprite = ConqueredMaterialSprites[5];
                break;
            case "Pусский":
                sr.sprite = ConqueredMaterialSprites[6];
                break;
            case "Español":
                sr.sprite = ConqueredMaterialSprites[7];
                break;

            case "Polski":
                sr.sprite = ConqueredMaterialSprites[8];
                break;
            case "Nederlands":
                sr.sprite = ConqueredMaterialSprites[9];
                break;


        }
    }

}
