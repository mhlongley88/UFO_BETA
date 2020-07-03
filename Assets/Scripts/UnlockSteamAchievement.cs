using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSteamAchievement : MonoBehaviour
{
    public SteamGameAchievements.Achievement achievement;

    public void Unlock()
    {
        SteamGameAchievements.instance.UnlockAchievement(achievement);
    }
}
