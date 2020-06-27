using DG.Tweening;
using HeathenEngineering.SteamTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamGameAchievements : MonoBehaviour
{
    public SteamAchievementData achivementData;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (!SteamManager.Initialized) yield return null;

      //  achivementData.isAchieved = true;
        achivementData.Unlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
