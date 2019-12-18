#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using HeathenEngineering.SteamTools;
using UnityEngine;

public class ExampleStatsUpdate : MonoBehaviour
{
    public SteamSettings SteamSettings;
    public SteamFloatStatData StatDataObject;
    public SteamAchievementData WinnerAchievement;
    public UnityEngine.UI.Text StatValue;
    public UnityEngine.UI.Text WinnerAchievmentStatus;

    private void Update()
    {
        StatValue.text = "Feet Traveled = " + StatDataObject.Value.ToString();
        WinnerAchievmentStatus.text = WinnerAchievement.displayName + "\n" + (WinnerAchievement.isAchieved ? "(Unlocked)" : "(Locked)");
    }

    public void UpdateStatValue(float amount)
    {
        StatDataObject.SetFloatStat(StatDataObject.Value + amount);
        SteamSettings.StoreStatsAndAchievements();
    }

    public void GetHelp()
    {
        Application.OpenURL("https://partner.steamgames.com/doc/features/achievements");
    }

    public void GetOverlayHelp()
    {
        Application.OpenURL("https://partner.steamgames.com/doc/features/overlay");
    }

    public void OnRetrieveStatsAndAchievements()
    {
        Debug.Log("[ExampleStatsUpdate.OnRetrieveStatsAndAchievement]\nStats loaded!");
    }

    public void OnStoredStatsAndAchievements()
    {
        Debug.Log("[ExampleStatsUpdate.OnStoredStatsAndAchievements]\nStats stored!");
    }
}
#endif