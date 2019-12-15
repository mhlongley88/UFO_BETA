#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using HeathenEngineering.Tools;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace HeathenEngineering.SteamTools
{

    public class SteamworksLeaderboardList : HeathenUIBehaviour
    {
        public HeathenSteamLeaderboardData Settings;
        public bool ignorePlayerRefresh = true;
        [Tooltip("The prototype to be spawned for each entry downloaded. This should contain a componenet derived from HeathenSteamLeaderboardEntry")]
        public GameObject entryPrototype;
        [Tooltip("The collection transform such as a Scroll Rect's 'container', this will be the 'parent' of the spawned entries.")]
        public RectTransform collection;
        [Tooltip("If true and if a scroll rect is found the players record will be scrolled to be as center of the view as possible on load of records.\n\nThis does not apply if the Override On Downloaded event is used.")]
        public bool focusPlayer = true;
        public LeaderboardEntry_t? UserEntry
        {
            get
            {
                if (Settings != null)
                    return Settings.UserEntry;
                else
                    return null;
            }
        }
        
        /// <summary>
        /// Stores a copy of the entries loaded on the last read
        /// This is not automatically updated if Override On Download is used
        /// </summary>
        [HideInInspector]
        public List<LeaderboardEntry_t> Entries;
        
        private UnityEngine.UI.ScrollRect scrollRect;

        private void Start()
        {
            scrollRect = collection.GetComponentInParent<UnityEngine.UI.ScrollRect>();

            if (Settings != null)
                RegisterBoard(Settings);
        }

        public void RegisterBoard(HeathenSteamLeaderboardData data)
        {
            if(Settings != null)
            {
                Settings.OnQueryResults.RemoveListener(HandleQuerryResult);
            }
            Settings = data;
            Settings.OnQueryResults.AddListener(HandleQuerryResult);
        }

        private void HandleQuerryResult(LeaderboardScoresDownloaded scores)
        {
            if (ignorePlayerRefresh && scores.scoreData.m_cEntryCount == 1 && scores.playerIncluded)
                return;

            float playerPosition = 1;
            if (scores.bIOFailure)
            {
                Debug.LogError("Failed to download score from Steam", this);
                return;
            }

            if (Entries == null)
                Entries = new List<LeaderboardEntry_t>();
            else
                Entries.Clear();

            //Clear the collection if we have one
            if (collection != null)
            {
                var dList = new List<GameObject>();
                foreach (Transform t in collection)
                {
                    dList.Add(t.gameObject);
                }

                while (dList.Count > 0)
                {
                    var t = dList[0];
                    dList.RemoveAt(0);
                    Destroy(t);
                }
            }

            var userId = SteamUser.GetSteamID();

            for (int i = 0; i < scores.scoreData.m_cEntryCount; i++)
            {
                LeaderboardEntry_t buffer;
                SteamUserStats.GetDownloadedLeaderboardEntry(scores.scoreData.m_hSteamLeaderboardEntries, i, out buffer, null, 0);
                Entries.Add(buffer);

                if(focusPlayer)
                {
                    if(userId.m_SteamID == buffer.m_steamIDUser.m_SteamID)
                    {
                        playerPosition = i / (float)scores.scoreData.m_cEntryCount;
                    }
                }

                if (entryPrototype != null && collection != null)
                {
                    var go = Instantiate(entryPrototype, collection);
                    var entry = go.GetComponent<HeathenSteamLeaderboardEntry>();
                    if (entry != null)
                    {
                        entry.selfTransform.localPosition = Vector3.zero;
                        entry.selfTransform.localRotation = Quaternion.identity;
                        entry.selfTransform.localScale = Vector3.one;
                        entry.ApplyEntry(buffer);
                    }
                }
            }

            if (focusPlayer && scrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = playerPosition;
                Canvas.ForceUpdateCanvases();
            }
        }
        
        public void UploadScore(int score, ELeaderboardUploadScoreMethod method)
        {
            Settings.UploadScore(score, method);
        }

        public void QueryEntries(ELeaderboardDataRequest requestType, int rangeStart, int rangeEnd)
        {
            Settings.QueryEntries(requestType, rangeStart, rangeEnd);
        }

        public void QueryTopEntries(int count)
        {
            Settings.QueryTopEntries(count);
        }

        public void QueryPeerEntries(int aroundPlayer)
        {
            Settings.QueryPeerEntries(aroundPlayer);
        }

        public void QueryFriendEntries(int aroundPlayer)
        {
            Settings.QueryFriendEntries(aroundPlayer);
        }

        public void RefreshUserEntry()
        {
            Settings.RefreshUserEntry();
        }
    }
}
#endif