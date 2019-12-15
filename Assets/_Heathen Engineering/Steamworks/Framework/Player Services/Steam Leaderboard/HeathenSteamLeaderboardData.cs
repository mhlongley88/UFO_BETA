#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamTools
{
    [CreateAssetMenu(menuName = "Steamworks/Leaderboard Data")]
    public class HeathenSteamLeaderboardData : ScriptableObject
    {
        public bool createIfMissing;
        public ELeaderboardSortMethod sortMethod;
        public ELeaderboardDisplayType displayType;
        public string leaderboardName;
        public int MaxDetailEntries = 0;
        [HideInInspector]
        public SteamLeaderboard_t? LeaderboardId;
        [HideInInspector]
        public LeaderboardEntry_t? UserEntry = null;

        public UnityEvent BoardFound = new UnityEvent();
        public LeaderboardScoresDownloadedEvent OnQueryResults = new LeaderboardScoresDownloadedEvent();
        public UnityLeaderboardRankUpdateEvent UserRankLoaded = new UnityLeaderboardRankUpdateEvent();
        public UnityLeaderboardRankChangeEvent UserRankChanged = new UnityLeaderboardRankChangeEvent();
        public UnityLeaderboardRankChangeEvent UserNewHighRank = new UnityLeaderboardRankChangeEvent();

        private CallResult<LeaderboardFindResult_t> OnLeaderboardFindResultCallResult;
        private CallResult<LeaderboardScoresDownloaded_t> OnLeaderboardScoresDownloadedCallResult;
        private CallResult<LeaderboardScoreUploaded_t> OnLeaderboardScoreUploadedCallResult;

        public void Register()
        {
            OnLeaderboardFindResultCallResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResult);
            OnLeaderboardScoresDownloadedCallResult = CallResult<LeaderboardScoresDownloaded_t>.Create(OnLeaderboardScoresDownloaded);
            OnLeaderboardScoreUploadedCallResult = CallResult<LeaderboardScoreUploaded_t>.Create(OnLeaderboardScoreUploaded);

            if (createIfMissing)
                FindOrCreateLeaderboard(sortMethod, displayType);
            else
                FindLeaderboard();
        }

        private void FindOrCreateLeaderboard(ELeaderboardSortMethod sortMethod, ELeaderboardDisplayType displayType)
        {
            var handle = SteamUserStats.FindOrCreateLeaderboard(leaderboardName, sortMethod, displayType);
            OnLeaderboardFindResultCallResult.Set(handle);
        }

        private void FindLeaderboard()
        {
            var handle = SteamUserStats.FindLeaderboard(leaderboardName);
            OnLeaderboardFindResultCallResult.Set(handle);
        }

        public void RefreshUserEntry()
        {
            if (!LeaderboardId.HasValue)
            {
                Debug.LogError("The leaderboard has not been initalized and cannot download scores.");
                return;
            }

            CSteamID[] users = new CSteamID[] { SteamUser.GetSteamID() };
            var handle = SteamUserStats.DownloadLeaderboardEntriesForUsers(LeaderboardId.Value, users, 1);
            OnLeaderboardScoresDownloadedCallResult.Set(handle);
        }

        public void UploadScore(int score, ELeaderboardUploadScoreMethod method)
        {
            if (!LeaderboardId.HasValue)
            {
                Debug.LogError("The leaderboard has not been initalized and cannot upload scores.");
                return;
            }

            var handle = SteamUserStats.UploadLeaderboardScore(LeaderboardId.Value, method, score, null, 0);
            OnLeaderboardScoreUploadedCallResult.Set(handle);
        }

        public void UploadScore(int score, int[] scoreDetails, ELeaderboardUploadScoreMethod method)
        {
            if (!LeaderboardId.HasValue)
            {
                Debug.LogError("The leaderboard has not been initalized and cannot upload scores.");
                return;
            }

            var handle = SteamUserStats.UploadLeaderboardScore(LeaderboardId.Value, method, score, scoreDetails, scoreDetails.Length);
            OnLeaderboardScoreUploadedCallResult.Set(handle);
        }

        public void QueryTopEntries(int count)
        {
            QueryEntries(ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 0, count);
        }

        public void QueryFriendEntries(int aroundPlayer)
        {
            QueryEntries(ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends, -aroundPlayer, aroundPlayer);
        }

        public void QueryPeerEntries(int aroundPlayer)
        {
            QueryEntries(ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -aroundPlayer, aroundPlayer);
        }

        public void QueryEntries(ELeaderboardDataRequest requestType, int rangeStart, int rangeEnd)
        {
            if (!LeaderboardId.HasValue)
            {
                Debug.LogError("The leaderboard has not been initalized and cannot download scores.");
                return;
            }

            var handle = SteamUserStats.DownloadLeaderboardEntries(LeaderboardId.Value, requestType, rangeStart, rangeEnd);
            OnLeaderboardScoresDownloadedCallResult.Set(handle);
        }

        private void OnLeaderboardScoreUploaded(LeaderboardScoreUploaded_t param, bool bIOFailure)
        {
            if (param.m_bSuccess == 0 || bIOFailure)
                Debug.LogError("Failed to upload score to Steam", this);

            RefreshUserEntry();
        }

        private void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t param, bool bIOFailure)
        {
            bool playerIncluded = false;
            ///Check for the current users data in the record set and update accordingly
            if (!bIOFailure)
            {
                var userId = SteamUser.GetSteamID();

                for (int i = 0; i < param.m_cEntryCount; i++)
                {
                    LeaderboardEntry_t buffer;
                    int[] details = null;

                    if (MaxDetailEntries < 1)
                        SteamUserStats.GetDownloadedLeaderboardEntry(param.m_hSteamLeaderboardEntries, i, out buffer, details, MaxDetailEntries);
                    else
                    {
                        details = new int[MaxDetailEntries];
                        SteamUserStats.GetDownloadedLeaderboardEntry(param.m_hSteamLeaderboardEntries, i, out buffer, details, MaxDetailEntries);
                    }

                    if (buffer.m_steamIDUser.m_SteamID == userId.m_SteamID)
                    {
                        playerIncluded = true;
                        if (!UserEntry.HasValue || UserEntry.Value.m_nGlobalRank != buffer.m_nGlobalRank)
                        {
                            var l = new LeaderboardUserData()
                            {
                                leaderboardName = leaderboardName,
                                leaderboardId = LeaderboardId.Value,
                                entry = buffer,
                                details = details
                            };

                            var lc = new LeaderboardRankChangeData()
                            {
                                leaderboardName = leaderboardName,
                                leaderboardId = LeaderboardId.Value,
                                newEntry = buffer,
                                oldEntry = UserEntry.HasValue ? new LeaderboardEntry_t?(UserEntry.Value) : null
                            };

                            UserEntry = buffer;

                            UserRankLoaded.Invoke(l);
                            UserRankChanged.Invoke(lc);

                            if (lc.newEntry.m_nGlobalRank < (lc.oldEntry.HasValue ? lc.oldEntry.Value.m_nGlobalRank : int.MaxValue))
                            {
                                UserNewHighRank.Invoke(lc);
                            }
                        }
                        else
                        {
                            var l = new LeaderboardUserData()
                            {
                                leaderboardName = leaderboardName,
                                leaderboardId = LeaderboardId.Value,
                                entry = buffer,
                                details = details
                            };

                            UserEntry = buffer;
                            UserRankLoaded.Invoke(l);
                        }
                    }
                }
            }

            //if (param.m_cEntryCount > 1 || (param.m_cEntryCount == 1 && !playerIncluded))
            OnQueryResults.Invoke(new LeaderboardScoresDownloaded() { bIOFailure = bIOFailure, scoreData = param, playerIncluded = playerIncluded });
        }

        private void OnLeaderboardFindResult(LeaderboardFindResult_t param, bool bIOFailure)
        {
            if (param.m_bLeaderboardFound == 0 || bIOFailure)
            {
                Debug.LogError("Failed to find leaderboard", this);
                return;
            }

            if (param.m_bLeaderboardFound != 0)
            {
                LeaderboardId = param.m_hSteamLeaderboard;
                BoardFound.Invoke();
                RefreshUserEntry();
            }
        }
    }
}
#endif