using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAchievementPostLevel : MonoBehaviour
{
    public SteamGameAchievements.Achievement achievement;
    public static SteamGameAchievements.Achievement achievementSelected;
    public static bool chosen;

    public void Set()
    {
        achievementSelected = achievement;
        chosen = true;
    }
    
    public void Unset()
    {
        chosen = false;
    }
}
