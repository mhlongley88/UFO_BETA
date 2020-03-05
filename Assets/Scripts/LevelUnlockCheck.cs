using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlockCheck : MonoBehaviour
{
    public static List<LevelUnlockCheck> All = new List<LevelUnlockCheck>();

    public int matchThreshold = 0;

    private void Awake()
    {
        All.Add(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        int matchesComplete = UnlockSystem.instance.GetMatchesCompleted();

        //Debug.Log(gameObject.name + " - Threshold " + matchThreshold + " and Matches Now " + matchesComplete + " - " + (matchesComplete >= matchThreshold).ToString());

        gameObject.SetActive(matchesComplete >= matchThreshold);

        UnlockSystem.instance.onBattlesCompletedChange.AddListener(OnRefresh);
    }

    private void OnDestroy()
    {
        All.Remove(this);

        UnlockSystem.instance.onBattlesCompletedChange.RemoveListener(OnRefresh);
    }

    // Update is called once per frame
    void OnRefresh()
    {
        int matchesComplete = UnlockSystem.instance.GetMatchesCompleted();
        gameObject.SetActive(matchesComplete >= matchThreshold);
    }
}
