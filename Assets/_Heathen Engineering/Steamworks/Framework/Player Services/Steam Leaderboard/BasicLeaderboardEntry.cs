#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;

namespace HeathenEngineering.SteamTools
{
    public class BasicLeaderboardEntry : HeathenSteamLeaderboardEntry
    {
        public UnityEngine.UI.Text rank;
        public SteamUserFullIcon avatar;
        public string formatString;
        public UnityEngine.UI.Text score;
        public LeaderboardEntry_t data;

        public override void ApplyEntry(LeaderboardEntry_t entry)
        {
            data = entry;
            var userData = SteamworksFoundationManager._GetUserData(entry.m_steamIDUser);
            avatar.LinkSteamUser(userData);
            if (!string.IsNullOrEmpty(formatString))
                score.text = entry.m_nScore.ToString(formatString);
            else
                score.text = entry.m_nScore.ToString();

            rank.text = entry.m_nGlobalRank.ToString();
        }
    }
}
#endif
