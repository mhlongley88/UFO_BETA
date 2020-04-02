﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        if (unlockedLevelNotification.Count > 0)
        {
            // For now like this, after that, put the integer
            //unlockedLevelNotification.Clear();
            unlockedLevelNotification.RemoveAt(0);

            UnlockNotification.instance.SignalUnlockLevel();
        }


        if (unlockedCharacterNotification.Count > 0)
        {
            // For now like this, after that, put the integer
            //unlockedCharacterNotification.Clear();
            unlockedCharacterNotification.RemoveAt(0);

            UnlockNotification.instance.SignalUnlockCharacter();
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
            unlockedCharacterNotification.Add(matchesCompleted);
        }
        if (HasThresholdForLevels())
        {
            unlockedLevelNotification.Add(matchesCompleted);
            unlockedLevelNotification.Add(matchesCompleted);
        }
        //Debug.Log("One more match completed!");
        //Debug.Log("Matches Completed: " + matchesCompleted);
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
