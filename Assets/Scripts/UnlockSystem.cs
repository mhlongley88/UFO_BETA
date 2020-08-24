using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UnlockSystem : MonoBehaviour
{
    public static UnlockSystem instance;

    [HideInInspector]
    public UnityEvent onBattlesCompletedChange = new UnityEvent();

    [HideInInspector]
    public List<int> allMatchesThresholdForCharacters = new List<int>();
    [HideInInspector]
    public List<int> allMatchesThresholdForLevels = new List<int>();

    public List<int> unlockedCharacterNotification = new List<int>();
    public List<int> unlockedLevelNotification = new List<int>();

    public List<int> unlockedCharacterNotificationMM = new List<int>();
    public List<int> unlockedLevelNotificationMM = new List<int>();

    public List<int> recentlyUnlockedCharacters = new List<int>();
    public List<int> recentlyUnlockedLevels = new List<int>();

    int matchesCompleted = 0;
    public int MatchesCompleted => matchesCompleted;

    const string BattlesCompletedPrefsKey = "UFOBattlesCompleted";
    const string OnlineBattlesCompletedPrefsKey = "UFOOnlineBattlesCompleted";

    int onlineMatchesCompleted = 0;
    public int OnlineMatchesCompleted => onlineMatchesCompleted;

    Dictionary<int, List<int>> allLevelNums = new Dictionary<int, List<int>>();

    private void Awake()
    {
        if(instance)
        {
            if (instance != this)
                Destroy(gameObject);

            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        matchesCompleted = UserPrefs.instance.GetInt(BattlesCompletedPrefsKey, 0);
        onlineMatchesCompleted = UserPrefs.instance.GetInt(OnlineBattlesCompletedPrefsKey, 0);
    }


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < GameManager.Instance.Characters.Length; i++)
        {
            if (GameManager.Instance.Characters[i].matchThreshold > matchesCompleted)
            {
                if(!CharacterUnlockFromProgression.IsUnlocked(i))
                    allMatchesThresholdForCharacters.Add(GameManager.Instance.Characters[i].matchThreshold);
            }
        }

        for (int i = 0; i < LevelUnlockCheck.All.Count; i++)
        {
            if(!allLevelNums.ContainsKey(LevelUnlockCheck.All[i].matchThreshold))
                allLevelNums.Add(LevelUnlockCheck.All[i].matchThreshold, new List<int>() { LevelUnlockCheck.All[i].levelTitle.levelNum });
            else
                allLevelNums[LevelUnlockCheck.All[i].matchThreshold].Add(LevelUnlockCheck.All[i].levelTitle.levelNum);

            if (LevelUnlockCheck.All[i].matchThreshold > matchesCompleted && !LevelUnlockFromProgression.IsUnlocked(LevelUnlockCheck.All[i].levelTitle.levelNum))
                allMatchesThresholdForLevels.Add(LevelUnlockCheck.All[i].matchThreshold);
        }

        //matchesCompleted = UserPrefs.instance.GetInt(BattlesCompletedPrefsKey, 0);

        //Debug.Log("Getting matches completed at initialization: " + matchesCompleted);
    }

    public void TickleNotifications()
    {
        // unlockedCharacterNotification.Add(0);

        if (SceneManager.GetActiveScene().name == "MainMenu" && MainMenuUIManager.Instance.currentMenu == MainMenuUIManager.Menu.Splash)
        {
            if (unlockedLevelNotificationMM.Count > 0)
            {
                unlockedLevelNotificationMM.Clear();

                UnlockNotification.instance.SignalUnlockLevel();
            }

            if (unlockedCharacterNotificationMM.Count > 0)
            {
                unlockedCharacterNotificationMM.Clear();

                UnlockNotification.instance.SignalUnlockCharacter();
            }
        }
        else
        {
            if (unlockedLevelNotification.Count > 0)
            {
                unlockedLevelNotification.Clear();

                UnlockNotification.instance.SignalUnlockLevel();
            }

            if (unlockedCharacterNotification.Count > 0)
            {
                unlockedCharacterNotification.Clear();

                UnlockNotification.instance.SignalUnlockCharacter();
            }
        }
    }
	
	void Update()
	{
#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.I))
		{
			Debug.Log("Current Matches Completed: " + matchesCompleted);
		}
		
		if(Input.GetKeyDown(KeyCode.O))
		{
			matchesCompleted = 0;
			UserPrefs.instance.SetInt(BattlesCompletedPrefsKey, matchesCompleted);
			UserPrefs.instance.Save();
		
			Debug.Log("Zeroed Matches Completed: " + matchesCompleted);
		}
		
		if(Input.GetKeyDown(KeyCode.P))
		{
			SaveMatchesCompleted();
			Debug.Log("Added match: " + matchesCompleted);
		}
#endif
	}

    public void SaveOnlineMatchesCompleted()
    {
        if(onlineMatchesCompleted == 0)
            SteamGameAchievements.instance.UnlockAchievement(SteamGameAchievements.Achievement.UFO_ACHIEVEMENT_1_4);

        onlineMatchesCompleted++;

        UserPrefs.instance.SetInt(OnlineBattlesCompletedPrefsKey, onlineMatchesCompleted);
        UserPrefs.instance.Save();

        if(onlineMatchesCompleted == 50)
            SteamGameAchievements.instance.UnlockAchievement(SteamGameAchievements.Achievement.UFO_ACHIEVEMENT_1_10);
    }

    public void SaveMatchesCompleted()
    {
        if(matchesCompleted == 0)
            SteamGameAchievements.instance.UnlockAchievement(SteamGameAchievements.Achievement.UFO_ACHIEVEMENT_1_0);

        matchesCompleted++;
        UserPrefs.instance.SetInt(BattlesCompletedPrefsKey, matchesCompleted);
        UserPrefs.instance.Save();

        if(matchesCompleted == 100)
            SteamGameAchievements.instance.UnlockAchievement(SteamGameAchievements.Achievement.UFO_ACHIEVEMENT_1_8);
        if (matchesCompleted == 200)
            SteamGameAchievements.instance.UnlockAchievement(SteamGameAchievements.Achievement.UFO_ACHIEVEMENT_1_9);


        onBattlesCompletedChange.Invoke();

        if (HasThresholdForCharacter())
        {
            var allUnlockedOnes = Array.FindAll(GameManager.Instance.Characters, it => it.matchThreshold <= matchesCompleted);
            for (int i = 0; i < allUnlockedOnes.Length; i++)
            {
                var characterIndex = Array.FindIndex(GameManager.Instance.Characters, it => it == allUnlockedOnes[i]);
                if (characterIndex >= 0 && !CharacterUnlockFromProgression.IsUnlocked(characterIndex))
                {
                    recentlyUnlockedCharacters.Add(characterIndex);
                }
            }

            if (recentlyUnlockedCharacters.Count > 0)
            {
                unlockedCharacterNotification.Add(matchesCompleted);
                unlockedCharacterNotificationMM.Add(matchesCompleted);
            }
        }
        if (HasThresholdForLevels())
        {
            if(allLevelNums.ContainsKey(matchesCompleted))
            {
                var allNums = allLevelNums[matchesCompleted];
                for (int i = 0; i < allNums.Count; i++)
                {
                    if(!LevelUnlockFromProgression.IsUnlocked(i))
                    {
                        unlockedLevelNotification.Add(matchesCompleted);
                        unlockedLevelNotificationMM.Add(matchesCompleted);
                    }
                }
            }

            if(unlockedLevelNotification.Count > 0)
                recentlyUnlockedLevels.Add(matchesCompleted);
        }
        //Debug.Log("One more match completed!");
        //Debug.Log("Matches Completed: " + matchesCompleted);
    }

    public void SetUnlockLevelFromProgression()
    {
        unlockedLevelNotification.Add(0);
        unlockedLevelNotificationMM.Add(0);

       // allMatchesThresholdForLevels.Remove(matches);
    }

    public void SetUnlockCharacterFromProgression(int matches)
    {
        unlockedCharacterNotification.Add(0);
        unlockedCharacterNotificationMM.Add(0);

        recentlyUnlockedCharacters.Add(matches);
    }

    public bool HasThresholdForCharacter()
    {
        if (allMatchesThresholdForCharacters.FindIndex(it => it == matchesCompleted) >= 0)
        { 
            allMatchesThresholdForCharacters.Remove(matchesCompleted);
            return true;
        }

        return false;
    }

    public bool HasThresholdForLevels()
    {
        if (allMatchesThresholdForLevels.FindIndex(it => it == matchesCompleted) >= 0)
        {
            allMatchesThresholdForLevels.Remove(matchesCompleted);
            return true;
        }

        return false;
    }

    public int GetMatchesCompleted()
    {
        return matchesCompleted;
    }
}
