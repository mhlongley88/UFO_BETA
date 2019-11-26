using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnlockSystem : MonoBehaviour
{
    public static UnlockSystem instance;

    [HideInInspector]
    public UnityEvent onBattlesCompletedChange = new UnityEvent();

    private void Awake()
    {
        if(instance)
        {
            if (instance != this)
                Destroy(instance);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    int matchesCompleted = 0;

    // Start is called before the first frame update
    void Start()
    {
        matchesCompleted = PlayerPrefs.GetInt("BattlesCompleted", 0);

        Debug.Log("Getting matches completed at initialization: " + matchesCompleted);
    }

    public void SaveMatchesCompleted()
    {
        matchesCompleted++;
        PlayerPrefs.SetInt("BattlesCompleted", matchesCompleted);

        PlayerPrefs.Save();

        onBattlesCompletedChange.Invoke();

        Debug.Log("One more match completed!");
        Debug.Log("Matches Completed: " + matchesCompleted);
    }

    public int GetMatchesCompleted()
    {
        return matchesCompleted;
    }
}
