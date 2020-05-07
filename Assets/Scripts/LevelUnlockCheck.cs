using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlockCheck : MonoBehaviour
{
    public static List<LevelUnlockCheck> All = new List<LevelUnlockCheck>();
    public GameObject newVfx;
    public int matchThreshold = 0;
    ShowLevelTitle levelTitle;

    private void Awake()
    {
        All.Add(this);


        levelTitle = GetComponent<ShowLevelTitle>();
    }

    // Start is called before the first frame update
    void Start()
    {
        int matchesComplete = UnlockSystem.instance.GetMatchesCompleted();

        //Debug.Log(gameObject.name + " - Threshold " + matchThreshold + " and Matches Now " + matchesComplete + " - " + (matchesComplete >= matchThreshold).ToString());

        gameObject.SetActive(matchesComplete >= matchThreshold);

        UnlockSystem.instance.onBattlesCompletedChange.AddListener(OnRefresh);

        if (newVfx != null)
        {
            int prefs = PlayerPrefs.GetInt("PlayedLevel" + levelTitle.levelNum);
            if (prefs == 1)
                newVfx.SetActive(false);
        }
        
        if(LevelUnlockFromProgression.IsUnlocked(levelTitle.levelNum))
        {
            gameObject.SetActive(true);
        }
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
