#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using HeathenEngineering.SteamTools;
using UnityEngine;

public class ExampleLeaderboardScoring : MonoBehaviour
{
    public HeathenSteamLeaderboardData leaderboardData;

    public void UpdateScore(int score)
    {
        //This just sends the current score and lets Steam decide if this is better than the previous or not
        leaderboardData.UploadScore(score, Steamworks.ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest);
        Debug.Log("Set leaderboard: " + leaderboardData.leaderboardName + " score to: " + score.ToString() + " with instruction to keep the best value (comparing current vs new)");
    }

    public void ForceUpdateScore(int score)
    {
        //This sends the current score and tells Steam to overwrite the old score
        leaderboardData.UploadScore(score, Steamworks.ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate);
        Debug.Log("Set leaderboard: " + leaderboardData.leaderboardName + " score to: " + score.ToString() + " with instruction to overwrite the current value");
    }

    public void AddToScore(int score)
    {
        //This gets whatever the last score was and adds the new score to it ... which is odd for a leaderboard but what you asked for
        int currentScore = leaderboardData.UserEntry.HasValue ? leaderboardData.UserEntry.Value.m_nScore : 0;
        leaderboardData.UploadScore(currentScore + score, Steamworks.ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest);
        Debug.Log("Set leaderboard: " + leaderboardData.leaderboardName + " score to: " + (currentScore + score).ToString() + " with instruction to keep the best value (comparing current vs new)");
    }

    public void GetHelp()
    {
        Application.OpenURL("https://partner.steamgames.com/doc/features/leaderboards");
    }
}
#endif