using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlockCheck : MonoBehaviour
{
    public static List<LevelUnlockCheck> All = new List<LevelUnlockCheck>();

    public GameObject newVfx;
    public int matchThreshold = 0;

    [HideInInspector]
    public ShowLevelTitle levelTitle;

    string PlayerLevelKey = "UFOPlayedLevel";
    static string unlockedFromBeatingBossKey = "UFOUnlockedLevelBoss";

    public bool hasABoss = false;

    Vector3 initialPosition;

    private void Awake()
    {
        All.Add(this);

        levelTitle = GetComponent<ShowLevelTitle>();

        initialPosition = transform.position;

        //UserPrefs.instance.SetInt(unlockedFromBeatingBossKey + levelTitle.levelNum, 0);
    }

    // Start is called before the first frame update
    void Start()
    {

        Init();

    }

    public void Init()
    {
        int matchesComplete = UnlockSystem.instance.GetMatchesCompleted();

        //Debug.Log(gameObject.name + " - Threshold " + matchThreshold + " and Matches Now " + matchesComplete + " - " + (matchesComplete >= matchThreshold).ToString());

        gameObject.SetActive(matchesComplete >= matchThreshold);

        UnlockSystem.instance.onBattlesCompletedChange.AddListener(OnRefresh);

        if (newVfx != null)
        {
            int prefs = UserPrefs.instance.GetInt(PlayerLevelKey + levelTitle.levelNum);
            if (prefs == 1)
                newVfx.SetActive(false);
        }

        if (LevelUnlockFromProgression.IsUnlocked(levelTitle.levelNum))
        {
            gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if(hasABoss)
        {
            if(LobbyConnectionHandler.instance.IsMultiplayerMode)
            {
                // Its a multiplayer game, check if its unlocked from boss
                if(!IsUnlockedThroughBoss())
                    transform.position = Vector3.down * 2000;
            }
            else
            {
                bool IsRealPlayerInGame(Player p)
                {
                    if(PlayerBot.active)
                    {
                        // Bot players dont count for this statement
                        if(PlayerBot.chosenPlayer.Contains(p)) return false;
                    }

                    return GameManager.Instance.IsPlayerInGame(p); 
                }

                int playerCountInGame = 0;
                playerCountInGame += IsRealPlayerInGame(Player.One) ? 1 : 0;
                playerCountInGame += IsRealPlayerInGame(Player.Two) ? 1 : 0;
                playerCountInGame += IsRealPlayerInGame(Player.Three) ? 1 : 0;
                playerCountInGame += IsRealPlayerInGame(Player.Four) ? 1 : 0;

                // More than one player in game then its a local multiplayer, check if its unlocked from boss
                if(playerCountInGame > 1)
                {
                    if(!IsUnlockedThroughBoss())
                        transform.position = Vector3.down * 2000;
                }
                else 
                {
                    transform.position = initialPosition;
                }
            }

           
        }
    }

    public static void UnlockByBoss(int num)
    {
      //  if (hasABoss)
        {
            UserPrefs.instance.SetInt(unlockedFromBeatingBossKey + num, 1);
        }
    }

    public static void ResetUnlockByBoss(int num)
    {
        //  if (hasABoss)
        {
            UserPrefs.instance.SetInt(unlockedFromBeatingBossKey + num, 0);
        }
    }

    public static bool IsUnlockedByBoss(int num)
    {
        //  if (hasABoss)
        {
            return UserPrefs.instance.GetInt(unlockedFromBeatingBossKey + num) == 1;
        }
    }

    bool IsUnlockedThroughBoss()
    {
        return UserPrefs.instance.GetInt(unlockedFromBeatingBossKey + levelTitle.levelNum, 0) == 1;
    }

    private void OnDestroy()
    {
        All.Remove(this);

        UnlockSystem.instance.onBattlesCompletedChange.RemoveListener(OnRefresh);
    }

    void OnRefresh()
    {
        int matchesComplete = UnlockSystem.instance.GetMatchesCompleted();
        gameObject.SetActive(matchesComplete >= matchThreshold);
    }
}
