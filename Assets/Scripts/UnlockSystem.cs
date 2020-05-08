﻿using System.Collections;
using System.Collections.Generic;
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

    public int MatchesCompleted => matchesCompleted;

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

        matchesCompleted = PlayerPrefs.GetInt("BattlesCompleted", 0);
    }

    int matchesCompleted = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < GameManager.Instance.Characters.Length; i++)
        {
            if(GameManager.Instance.Characters[i].matchThreshold > matchesCompleted)
                allMatchesThresholdForCharacters.Add(GameManager.Instance.Characters[i].matchThreshold);
        }

        for (int i = 0; i < LevelUnlockCheck.All.Count; i++)
        {
            if(LevelUnlockCheck.All[i].matchThreshold > matchesCompleted)
                allMatchesThresholdForLevels.Add(LevelUnlockCheck.All[i].matchThreshold);
        }

        //matchesCompleted = PlayerPrefs.GetInt("BattlesCompleted", 0);

        //Debug.Log("Getting matches completed at initialization: " + matchesCompleted);
    }

    public void TickleNotifications()
    {
        // unlockedCharacterNotification.Add(0);

        if (SceneManager.GetActiveScene().name == "MainMenu")
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
			PlayerPrefs.SetInt("BattlesCompleted", matchesCompleted);
			PlayerPrefs.Save();
		
			Debug.Log("Zeroed Matches Completed: " + matchesCompleted);
		}
		
		if(Input.GetKeyDown(KeyCode.P))
		{
			SaveMatchesCompleted();
			Debug.Log("Added match: " + matchesCompleted);
		}
#endif
	}

    public void SaveMatchesCompleted()
    {
        matchesCompleted++;
        PlayerPrefs.SetInt("BattlesCompleted", matchesCompleted);
        PlayerPrefs.Save();

        onBattlesCompletedChange.Invoke();

        if (HasThresholdForCharacter())
        {
            unlockedCharacterNotification.Add(matchesCompleted);
            unlockedCharacterNotificationMM.Add(matchesCompleted);

            recentlyUnlockedCharacters.Add(MatchesCompleted);

        }
        if (HasThresholdForLevels())
        {
            unlockedLevelNotification.Add(matchesCompleted);
            unlockedLevelNotificationMM.Add(matchesCompleted);

            recentlyUnlockedLevels.Add(matchesCompleted);
        }
        //Debug.Log("One more match completed!");
        //Debug.Log("Matches Completed: " + matchesCompleted);
    }

    public void SetUnlockLevelFromProgression()
    {
        unlockedLevelNotification.Add(0);
        unlockedLevelNotificationMM.Add(0);
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
