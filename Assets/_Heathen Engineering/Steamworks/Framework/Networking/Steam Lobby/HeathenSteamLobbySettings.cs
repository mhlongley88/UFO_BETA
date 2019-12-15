#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using HeathenEngineering.Scriptable;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamTools
{
    [CreateAssetMenu(menuName = "Steamworks/Lobby Settings")]
    [Serializable]
    public class HeathenSteamLobbySettings : ScriptableObject
    {
        /// <summary>
        /// Used during Quick Match Searching this controls the further Steam distance that will be searched for a lobby
        /// </summary>
        [Header("Quick Match Settings")]
        public ELobbyDistanceFilter MaxDistanceFilter = ELobbyDistanceFilter.k_ELobbyDistanceFilterDefault;
        /// <summary>
        /// Returns true if the player is in the lobby that the Steam Lobby Settings object is tracking
        /// </summary>
        public bool InLobby
        {
            get
            {
                //Lobby Member Data is supposed to return null if the player is not in the lobby or the lobby id is invalid
                //In any other case it should return the value of the key or if none return string.empty thus if it returns null this user is not in this lobby
                if (SteamMatchmaking.GetLobbyMemberData(LobbyId, SteamUser.GetSteamID(), "anyField") == null)
                    return false;
                else
                    return true;
            }
        }
        /// <summary>
        /// Returns true if the Steam Lobby Settings object Lobby ID is valid e.g. has a lobby to track
        /// </summary>
        public bool HasLobby
        {
            get
            {
                return LobbyId != CSteamID.Nil;
            }
        }
        /// <summary>
        /// Returns true if the Steam Lobby Settings object has a valid lobby and the current player is that lobby's owner
        /// </summary>
        public bool IsHost
        {
            get
            {
                return HasLobby && SteamUser.GetSteamID() == SteamMatchmaking.GetLobbyOwner(LobbyId);
            }
        }
        /// <summary>
        /// Returns true while the system is waiting for search result responce
        /// </summary>
        public bool IsSearching
        {
            get { return standardSearch; }
        }
        /// <summary>
        /// Returns true while the system is performing a quick search
        /// </summary>
        public bool IsQuickSearching
        {
            get { return quickMatchSearch; }
        }
        public string LobbyName;
        [Tooltip("This will be set by the Lobby Manager when joining a lobby")]
        protected CSteamID LobbyId;
        public CSteamID lobbyId { get { return LobbyId; } }
        [Tooltip("Steam will only accept values between 0 and 250 but expects a datatype of int")]
        public IntReference MaxMemberCount = new IntReference(4);
        [Header("Current Lobby Data")]
        [Tooltip("These are the keys of the data that can be registered against a members metadata")]
        public List<string> MemberDataKeys;
        public LobbyMember LobbyOwner;
        public List<LobbyMember> Members;
        public LobbyMetadata Metadata;
        

        [HideInInspector]
        public ISteamworksLobbyManager Manager;

        #region Internal Data
        private bool quickMatchCreateOnFail = false;
        [NonSerialized]
        private bool standardSearch = false;
        [NonSerialized]
        private bool quickMatchSearch = false;
        [NonSerialized]
        private bool callbacksRegistered = false;
        private LobbyHunterFilter createLobbyFilter;
        private LobbyHunterFilter quickMatchFilter;
        #endregion

        #region Callbacks
        private Callback<LobbyCreated_t> m_LobbyCreated;
        private Callback<LobbyEnter_t> m_LobbyEntered;
        private Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested;
        private Callback<LobbyChatUpdate_t> m_LobbyChatUpdate;
        private CallResult<LobbyMatchList_t> m_LobbyMatchList;
        private Callback<LobbyGameCreated_t> m_LobbyGameCreated;
        private Callback<LobbyDataUpdate_t> m_LobbyDataUpdated;
        private Callback<LobbyChatMsg_t> m_LobbyChatMsg;
        #endregion

        #region Events
        /// <summary>
        /// Occures when a request to join the lobby has been recieved such as through Steam's invite friend dialog in the Steam Overlay
        /// </summary>
        [Header("Events")]
        public UnityGameLobbyJoinRequestedEvent OnGameLobbyJoinRequest = new UnityGameLobbyJoinRequestedEvent();
        /// <summary>
        /// Occures when list of Lobbies is retured from a search
        /// </summary>
        public UnityLobbyHunterListEvent OnLobbyMatchList = new UnityLobbyHunterListEvent();
        /// <summary>
        /// Occures when a lobby is created by the player
        /// </summary>
        public UnityLobbyCreatedEvent OnLobbyCreated = new UnityLobbyCreatedEvent();
        /// <summary>
        /// Occures when the owner of the currently tracked lobby changes
        /// </summary>
        public LobbyMemberEvent OnOwnershipChange = new LobbyMemberEvent();
        /// <summary>
        /// Occures when a member joins the lobby
        /// </summary>
        public LobbyMemberEvent OnMemberJoined = new LobbyMemberEvent();
        /// <summary>
        /// Occures when a member leaves the lobby
        /// </summary>
        public LobbyMemberEvent OnMemberLeft = new LobbyMemberEvent();
        /// <summary>
        /// Occures when Steam metadata for a member changes
        /// </summary>
        public LobbyMemberEvent OnMemberDataChanged = new LobbyMemberEvent();
        /// <summary>
        /// Occures when the player joins a lobby
        /// </summary>
        public UnityLobbyEnterEvent OnLobbyEnter = new UnityLobbyEnterEvent();
        /// <summary>
        /// Occures when the player leaves a lobby
        /// </summary>
        public UnityEvent OnLobbyExit = new UnityEvent();
        /// <summary>
        /// Occures when lobby metadata changes
        /// </summary>
        public UnityEvent OnLobbyDataChanged = new UnityEvent();
        /// <summary>
        /// Occures when the host of the lobby starts the game e.g. sets game server data on the lobby
        /// </summary>
        public UnityEvent OnGameServerSet = new UnityEvent();
        /// <summary>
        /// Occures when lobby chat metadata has been updated such as a kick or ban.
        /// </summary>
        public UnityLobbyChatUpdateEvent OnLobbyChatUpdate = new UnityLobbyChatUpdateEvent();
        /// <summary>
        /// Occures when a quick match search fails to return a lobby match
        /// </summary>
        public UnityEvent QuickMatchFailed = new UnityEvent();
        /// <summary>
        /// Occures when a search for a lobby has started
        /// </summary>
        public UnityEvent SearchStarted = new UnityEvent();
        /// <summary>
        /// Occures when a lobby chat message is recieved
        /// </summary>
        public LobbyChatMessageEvent OnChatMessageReceived = new LobbyChatMessageEvent();
        /// <summary>
        /// Occures when a member of the lobby chat enters the chat
        /// </summary>
        public LobbyMemberEvent ChatMemberStateChangeEntered = new LobbyMemberEvent();
        /// <summary>
        /// Occures when a member of the lobby chat leaves the chat
        /// </summary>
        public UnityPersonaEvent ChatMemberStateChangeLeft = new UnityPersonaEvent();
        /// <summary>
        /// Occures when a member of the lobby chat is disconnected from the chat
        /// </summary>
        public UnityPersonaEvent ChatMemberStateChangeDisconnected = new UnityPersonaEvent();
        /// <summary>
        /// Occures when a member of the lobby chat is kicked out of the chat
        /// </summary>
        public UnityPersonaEvent ChatMemberStateChangeKicked = new UnityPersonaEvent();
        /// <summary>
        /// Occures when a member of the lobby chat is banned from the chat
        /// </summary>
        public UnityPersonaEvent ChatMemberStateChangeBanned = new UnityPersonaEvent();
        #endregion

        /// <summary>
        /// Typically called by the HeathenSteamManager.OnEnable()
        /// This registeres the Valve callbacks and CallResult deligates
        /// </summary>
        public void RegisterCallbacks()
        {
            if (SteamworksFoundationManager.Initialized)
            {   
                if (!callbacksRegistered)
                {
                    callbacksRegistered = true;
                    m_LobbyCreated = Callback<LobbyCreated_t>.Create(HandleLobbyCreated);
                    m_LobbyEntered = Callback<LobbyEnter_t>.Create(HandleLobbyEntered);
                    m_GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(HandleGameLobbyJoinRequested);
                    m_LobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(HandleLobbyChatUpdate);
                    m_LobbyMatchList = CallResult<LobbyMatchList_t>.Create(HandleLobbyMatchList);
                    m_LobbyGameCreated = Callback<LobbyGameCreated_t>.Create(HandleLobbyGameCreated);
                    m_LobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(HandleLobbyDataUpdate);
                    m_LobbyChatMsg = Callback<LobbyChatMsg_t>.Create(HandleLobbyChatMessage);
                }
            }
        }

        /// <summary>
        /// Sets the tracked lobby ID and updates the relivent member data and metadata lists
        /// </summary>
        /// <param name="lobbyId"></param>
        public void SetLobbyId(CSteamID lobbyId)
        {
            if (lobbyId == CSteamID.Nil)
            {
                LobbyId = CSteamID.Nil;
                Members.Clear();
                LobbyOwner = null;
            }
            else if (LobbyId != lobbyId)
            {
                LobbyId = lobbyId;
                Members.Clear();
                LobbyOwner = null;
                CSteamID ownerId = SteamMatchmaking.GetLobbyOwner(LobbyId);
                var count = SteamMatchmaking.GetNumLobbyMembers(LobbyId);
                for (int i = 0; i < count; i++)
                {
                    var memberId = SteamMatchmaking.GetLobbyMemberByIndex(LobbyId, i);
                    var record = ProcessLobbyMember(memberId);
                    if (memberId == ownerId)
                        LobbyOwner = record;
                }
            }
            else
            {
                //We are already aware of this lobby so just refresh the owner and member data
                CSteamID ownerId = SteamMatchmaking.GetLobbyOwner(LobbyId);
                var count = SteamMatchmaking.GetNumLobbyMembers(LobbyId);
                for (int i = 0; i < count; i++)
                {
                    var memberId = SteamMatchmaking.GetLobbyMemberByIndex(LobbyId, i);
                    var record = ProcessLobbyMember(memberId);
                    if (memberId == ownerId)
                        LobbyOwner = record;
                }
            }
        }

        /// <summary>
        /// Forces an update on change of lobby ownership, this is automatically called when the original host is removed from the lobby
        /// </summary>
        private void HandleOwnershipChange()
        {
            var ownerId = SteamMatchmaking.GetLobbyOwner(LobbyId);
            LobbyOwner = ProcessLobbyMember(ownerId);
        }

        /// <summary>
        /// Removes the member from the settings list
        /// </summary>
        /// <param name="memberId">The member to remove</param>
        /// <returns>True if the removed member was the lobby owner, otherwise false</returns>
        public bool RemoveMember(CSteamID memberId)
        {
            var targetMember = Members.FirstOrDefault(p => p.UserData != null && p.UserData.SteamId == memberId);
            Members.Remove(targetMember);
            OnMemberLeft.Invoke(targetMember);
            //Members.RemoveAll(p => p.UserData != null || p.UserData.SteamId == memberId);

            if (memberId == SteamMatchmaking.GetLobbyOwner(LobbyId))
            {
                HandleOwnershipChange();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Called when a lobby member joins or updates metadata
        /// </summary>
        /// <param name="memberId"></param>
        public LobbyMember ProcessLobbyMember(CSteamID memberId)
        {
            LobbyMember member = Members.FirstOrDefault(p => p.UserData != null && p.UserData.SteamId == memberId);

            if (member == null)
            {
                member = new LobbyMember();
                member.UserData = SteamworksFoundationManager._GetUserData(memberId);
                Members.Add(member);
                OnMemberJoined.Invoke(member);
            }

            if (member.Metadata.Records == null)
                member.Metadata.Records = new List<MetadataRecord>();
            else
                member.Metadata.Records.Clear();

            foreach (var dataKey in MemberDataKeys)
            {
                MetadataRecord record = new MetadataRecord() { key = dataKey };
                record.value = SteamMatchmaking.GetLobbyMemberData(LobbyId, memberId, dataKey);
                member.Metadata.Records.Add(record);
            }

            OnMemberDataChanged.Invoke(member);

            return member;
        }

        #region Callbacks
        private void HandleLobbyList(SteamLobbyLobbyList lobbyList)
        {
            int lobbyCount = lobbyList.Count;

            if (quickMatchSearch)
            {
                if (lobbyCount == 0)
                {
                    if (quickMatchFilter.useSlotsAvailable)
                    {
                        if (quickMatchFilter.requiredOpenSlots + 1 < MaxMemberCount - 1)
                            quickMatchFilter.requiredOpenSlots++;
                        else
                        {
                            quickMatchFilter.requiredOpenSlots = 1;

                            if (!quickMatchFilter.useDistanceFilter)
                                quickMatchFilter.useDistanceFilter = true;

                            switch (quickMatchFilter.distanceOption)
                            {
                                case ELobbyDistanceFilter.k_ELobbyDistanceFilterClose:
                                    if ((int)MaxDistanceFilter >= 1)
                                        quickMatchFilter.distanceOption = ELobbyDistanceFilter.k_ELobbyDistanceFilterDefault;
                                    else
                                    {
                                        HandleQuickMatchFailed();
                                        return;
                                    }
                                    break;
                                case ELobbyDistanceFilter.k_ELobbyDistanceFilterDefault:
                                    if ((int)MaxDistanceFilter >= 2)
                                        quickMatchFilter.distanceOption = ELobbyDistanceFilter.k_ELobbyDistanceFilterFar;
                                    else
                                    {
                                        HandleQuickMatchFailed();
                                        return;
                                    }
                                    break;
                                case ELobbyDistanceFilter.k_ELobbyDistanceFilterFar:
                                    if ((int)MaxDistanceFilter >= 3)
                                        quickMatchFilter.distanceOption = ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide;
                                    else
                                    {
                                        HandleQuickMatchFailed();
                                        return;
                                    }
                                    break;
                                case ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide:
                                    HandleQuickMatchFailed();
                                    return;
                            }
                        }

                        FindQuickMatch();
                    }
                    else
                    {
                        //Should never happen when quick searching but just in case, treate this as a ran out of search options.
                        HandleQuickMatchFailed();
                    }
                }
                else
                {
                    //We got a hit, the top option should be the best option so join it
                    var lobby = SteamMatchmaking.GetLobbyByIndex(0);
                    JoinLobby(lobby);
                }
            }
        }

        private void HandleQuickMatchFailed()
        {
            quickMatchSearch = false;
            if (quickMatchCreateOnFail)
            {
                Debug.Log("Quick Match failed to find a lobby and will create a new one.");
                CreateLobby(quickMatchFilter, LobbyName, ELobbyType.k_ELobbyTypePublic);
            }
            else
            {
                Debug.Log("Quick Match failed to find a lobby.");
                QuickMatchFailed.Invoke();
            }
        }

        private void FindQuickMatch()
        {
            if (!callbacksRegistered)
                RegisterCallbacks();

            SetLobbyFilter(quickMatchFilter);

            var call = SteamMatchmaking.RequestLobbyList();
            m_LobbyMatchList.Set(call, HandleLobbyMatchList);

            SearchStarted.Invoke();
        }

        private void SetLobbyFilter(LobbyHunterFilter LobbyFilter)
        {
            if (LobbyFilter.useSlotsAvailable)
                SteamMatchmaking.AddRequestLobbyListFilterSlotsAvailable(LobbyFilter.requiredOpenSlots);

            if (LobbyFilter.useDistanceFilter)
                SteamMatchmaking.AddRequestLobbyListDistanceFilter(LobbyFilter.distanceOption);

            if (LobbyFilter.maxResults > 0)
                SteamMatchmaking.AddRequestLobbyListResultCountFilter(LobbyFilter.maxResults);

            if (LobbyFilter.numberValues != null)
            {
                foreach (var f in LobbyFilter.numberValues)
                    SteamMatchmaking.AddRequestLobbyListNumericalFilter(f.key, f.value, f.method);
            }

            if (LobbyFilter.nearValues != null)
            {
                foreach (var f in LobbyFilter.nearValues)
                    SteamMatchmaking.AddRequestLobbyListNearValueFilter(f.key, f.value);
            }

            if (LobbyFilter.stringValues != null)
            {
                foreach (var f in LobbyFilter.stringValues)
                    SteamMatchmaking.AddRequestLobbyListStringFilter(f.key, f.value, f.method);
            }
        }
        #endregion  

        #region Callback Handlers
        void HandleLobbyGameCreated(LobbyGameCreated_t param)
        {
            OnGameServerSet.Invoke();
        }

        void HandleLobbyMatchList(LobbyMatchList_t pCallback, bool bIOFailure)
        {
            uint numLobbies = pCallback.m_nLobbiesMatching;
            var result = new SteamLobbyLobbyList();
            Debug.Log("Lobby match list returned (" + numLobbies.ToString() + ")");
            if (numLobbies <= 0)
            {
                if (quickMatchSearch)
                {
                    HandleLobbyList(result);
                }
                else
                {
                    standardSearch = false;
                    OnLobbyMatchList.Invoke(result);
                }
            }
            else
            {
                for (int i = 0; i < numLobbies; i++)
                {
                    LobbyHunterLobbyRecord record = new LobbyHunterLobbyRecord();

                    record.metaData = new Dictionary<string, string>();
                    record.lobbyId = SteamMatchmaking.GetLobbyByIndex(i);
                    record.hostId = SteamMatchmaking.GetLobbyOwner(record.lobbyId);
                    int memberCount = SteamMatchmaking.GetNumLobbyMembers(record.lobbyId);
                    record.maxSlots = SteamMatchmaking.GetLobbyMemberLimit(record.lobbyId);
                    

                    int dataCount = SteamMatchmaking.GetLobbyDataCount(record.lobbyId);

                    if (record.lobbyId == LobbyId)
                    {
                        Debug.Log("Browsed our own lobby and found " + dataCount.ToString() + " metadata records.");
                    }

                    for (int ii = 0; ii < dataCount; ii++)
                    {
                        bool isUs = (record.lobbyId == LobbyId);
                        string key;
                        string value;
                        if (SteamMatchmaking.GetLobbyDataByIndex(record.lobbyId, ii, out key, Constants.k_nMaxLobbyKeyLength, out value, Constants.k_cubChatMetadataMax))
                        {
                            record.metaData.Add(key, value);
                            if (key == "name")
                                record.name = value;
                            if (key == "OwnerID")
                            {
                                ulong val;
                                if (ulong.TryParse(value, out val))
                                {
                                    record.hostId = new CSteamID(val);
                                }
                            }

                            if (isUs)
                            {
                                Debug.Log("My Lobby data key = [" + key + "], value = [" + value + "]");
                            }
                        }
                    }

                    result.Add(record);
                }

                if (quickMatchSearch)
                {
                    HandleLobbyList(result);
                }
                else
                {
                    standardSearch = false;
                    OnLobbyMatchList.Invoke(result);
                }
            }
        }

        void HandleLobbyChatUpdate(LobbyChatUpdate_t pCallback)
        {
            if (LobbyId.m_SteamID != pCallback.m_ulSteamIDLobby)
                return;

            if (pCallback.m_rgfChatMemberStateChange == (uint)EChatMemberStateChange.k_EChatMemberStateChangeLeft)
            {
                var memberId = new CSteamID(pCallback.m_ulSteamIDUserChanged);
                var member = Members.FirstOrDefault(p => p.UserData != null && p.UserData.SteamId == memberId);

                if (RemoveMember(memberId))
                {
                    OnOwnershipChange.Invoke(LobbyOwner);
                    //OnMemberLeft.Invoke(member);
                }
                //else
                //{
                //    OnMemberLeft.Invoke(member);
                //}
                ChatMemberStateChangeLeft.Invoke(SteamworksFoundationManager._GetUserData(memberId));
            }
            else if (pCallback.m_rgfChatMemberStateChange == (uint)EChatMemberStateChange.k_EChatMemberStateChangeEntered)
            {
                var member = ProcessLobbyMember(new CSteamID(pCallback.m_ulSteamIDUserChanged));

                //OnMemberJoined.Invoke(member);
                ChatMemberStateChangeEntered.Invoke(member);
            }
            else if (pCallback.m_rgfChatMemberStateChange == (uint)EChatMemberStateChange.k_EChatMemberStateChangeDisconnected)
            {
                var memberId = new CSteamID(pCallback.m_ulSteamIDUserChanged);
                var member = Members.FirstOrDefault(p => p.UserData != null && p.UserData.SteamId == memberId);

                if (RemoveMember(memberId))
                {
                    OnOwnershipChange.Invoke(LobbyOwner);
                    //OnMemberLeft.Invoke(member);
                }
                //else
                //{
                //    OnMemberLeft.Invoke(member);
                //}
                ChatMemberStateChangeDisconnected.Invoke(SteamworksFoundationManager._GetUserData(memberId));
            }
            else if (pCallback.m_rgfChatMemberStateChange == (uint)EChatMemberStateChange.k_EChatMemberStateChangeKicked)
            {
                var memberId = new CSteamID(pCallback.m_ulSteamIDUserChanged);
                var member = Members.FirstOrDefault(p => p.UserData != null && p.UserData.SteamId == memberId);

                if (RemoveMember(memberId))
                {
                    OnOwnershipChange.Invoke(LobbyOwner);
                    //OnMemberLeft.Invoke(member);
                }
                //else
                //{
                //    OnMemberLeft.Invoke(member);
                //}
                ChatMemberStateChangeKicked.Invoke(SteamworksFoundationManager._GetUserData(memberId));
            }
            else if (pCallback.m_rgfChatMemberStateChange == (uint)EChatMemberStateChange.k_EChatMemberStateChangeBanned)
            {
                var memberId = new CSteamID(pCallback.m_ulSteamIDUserChanged);
                var member = Members.FirstOrDefault(p => p.UserData != null && p.UserData.SteamId == memberId);

                if (RemoveMember(memberId))
                {
                    OnOwnershipChange.Invoke(LobbyOwner);
                    //OnMemberLeft.Invoke(member);
                }
                //else
                //{
                //    OnMemberLeft.Invoke(member);
                //}
                ChatMemberStateChangeBanned.Invoke(SteamworksFoundationManager._GetUserData(memberId));
            }

            OnLobbyChatUpdate.Invoke(pCallback);
        }

        void HandleGameLobbyJoinRequested(GameLobbyJoinRequested_t param)
        {
            JoinLobby(param.m_steamIDLobby);
        }

        void HandleLobbyEntered(LobbyEnter_t param)
        {
            SetLobbyId(new CSteamID(param.m_ulSteamIDLobby));

            OnLobbyEnter.Invoke(param);
        }

        void HandleLobbyCreated(LobbyCreated_t param)
        {
            SetLobbyId(new CSteamID(param.m_ulSteamIDLobby));

            SteamMatchmaking.SetLobbyMemberLimit(LobbyId, MaxMemberCount);

            if (SteamMatchmaking.SetLobbyData(LobbyId, "name", LobbyName))
                Debug.Log("Set lobby data [name] to [" + LobbyName + "]");
            else
                Debug.Log("Failed to set lobby data [name] to [" + LobbyName + "]");

            if (Metadata.Records == null)
            {
                Metadata.Records = new List<MetadataRecord>();
            }
            else
            {
                Metadata.Records.Clear();
            }

            Metadata.Records.Clear();
            foreach (var f in createLobbyFilter.stringValues)
            {
                if (SteamMatchmaking.SetLobbyData(LobbyId, f.key, f.value))
                {
                    Metadata[f.key] = f.value;
                    Debug.Log("Set lobby data [" + f.key + "] to [" + f.value + "]");
                }
                else
                    Debug.Log("Failed to set lobby data [" + f.key + "] to [" + f.value + "]");
            }
            foreach (var f in createLobbyFilter.numberValues)
            {
                if (SteamMatchmaking.SetLobbyData(LobbyId, f.key, f.value.ToString()))
                {
                    Metadata[f.key] = f.value.ToString();
                    Debug.Log("Set lobby data [" + f.key + "] to [" + f.value.ToString() + "]");
                }
                else
                    Debug.Log("Failed to set lobby data [" + f.key + "] to [" + f.value.ToString() + "]");
            }

            OnLobbyCreated.Invoke(param);
        }

        void HandleLobbyDataUpdate(LobbyDataUpdate_t param)
        {
            if (param.m_ulSteamIDLobby == param.m_ulSteamIDMember)
            {
                var count = SteamMatchmaking.GetLobbyDataCount(LobbyId);
                var key = "";
                var value = "";
                for (int i = 0; i < count; i++)
                {
                    if (SteamMatchmaking.GetLobbyDataByIndex(LobbyId, i, out key, Constants.k_nMaxLobbyKeyLength, out value, Constants.k_cubChatMetadataMax))
                    {
                        if (key == "name")
                            LobbyName = value;

                        Metadata[key] = value;
                    }
                }
                OnLobbyDataChanged.Invoke();
            }
            else
            {
                ProcessLobbyMember(new CSteamID(param.m_ulSteamIDMember));
            }
        }

        void HandleLobbyChatMessage(LobbyChatMsg_t pCallback)
        {
            var subjectLobby = (CSteamID)pCallback.m_ulSteamIDLobby;
            if (subjectLobby != LobbyId)
                return;

            CSteamID SteamIDUser;
            byte[] Data = new byte[4096];
            EChatEntryType ChatEntryType;
            int ret = SteamMatchmaking.GetLobbyChatEntry(subjectLobby, (int)pCallback.m_iChatID, out SteamIDUser, Data, Data.Length, out ChatEntryType);
            byte[] truncated = new byte[ret];
            Array.Copy(Data, truncated, ret);

            LobbyChatMessageData record = new LobbyChatMessageData();
            record.sender = Members.FirstOrDefault(p => p.UserData.SteamId == SteamIDUser);
            record.message = System.Text.Encoding.UTF8.GetString(truncated);
            record.recievedTime = DateTime.Now;
            record.chatEntryType = ChatEntryType;

            OnChatMessageReceived.Invoke(record);
        }
        #endregion
        /// <summary>
        /// Creates a new lobby according the LobbyFilter information provided
        /// </summary>
        /// <param name="LobbyFilter">Defines the base metadata in the form of a search filter</param>
        /// <param name="LobbyName">The name to be applied to the lobby, all Heathen lobbies define a lobby name if none is passed the current users display name will be used</param>
        /// <param name="lobbyType">The type of lobby to be created ... see Valve's documentation regarding ELobbyType for more informaiton</param>
        public void CreateLobby(LobbyHunterFilter LobbyFilter, string LobbyName = "", ELobbyType lobbyType = ELobbyType.k_ELobbyTypePublic)
        {
            if (LobbyName == "")
                LobbyName = SteamworksFoundationManager.Instance.Settings.UserData.DisplayName;
            else
                this.LobbyName = LobbyName;

            createLobbyFilter = LobbyFilter;

            SteamMatchmaking.CreateLobby(lobbyType, MaxMemberCount);
        }

        /// <summary>
        /// Joins a steam lobby
        /// Note this will leave any current lobby before joining the new lobby
        /// </summary>
        /// <param name="lobbyId">The ID of the lobby to join</param>
        public void JoinLobby(CSteamID lobbyId)
        {
            if (LobbyId == lobbyId)
                return;

            if (LobbyId != CSteamID.Nil)
                SteamMatchmaking.LeaveLobby(LobbyId);

            LobbyId = CSteamID.Nil;

            SteamMatchmaking.JoinLobby(lobbyId);
        }

        /// <summary>
        /// Joins a steam lobby
        /// Note this will leave any current lobby before joining the new lobby
        /// </summary>
        /// <param name="lobbyId">The ID of the lobby to join</param>
        public void JoinLobby(ulong lobbyId)
        {
            if (LobbyId == new CSteamID(lobbyId))
                return;

            if (LobbyId != CSteamID.Nil)
                SteamMatchmaking.LeaveLobby(LobbyId);

            LobbyId = CSteamID.Nil;

            SteamMatchmaking.JoinLobby(new CSteamID(lobbyId));
        }

        /// <summary>
        /// Leaves the current lobby if any
        /// </summary>
        public void LeaveLobby()
        {
            var result = false;
            if (InLobby)
                result = true;

            try
            {
                SteamMatchmaking.LeaveLobby(LobbyId);
            }
            catch { }
            LobbyId = CSteamID.Nil;
            LobbyOwner = null;
            Members = new List<LobbyMember>();
            Metadata = new LobbyMetadata();

            OnLobbyExit.Invoke();

            if (result)
                OnLobbyExit.Invoke();
        }

        /// <summary>
        /// Searches for a matching lobby according to the provided filter data.
        /// Note that a search will only start if no search is currently running.
        /// </summary>
        /// <param name="LobbyFilter">Describes the metadata to search for in a lobby</param>
        public void FindMatch(LobbyHunterFilter LobbyFilter)
        {
            if (quickMatchSearch)
            {
                Debug.LogError("Attempted to search for a lobby while a quick search is processing. This search will be ignored, you must call CancelQuickMatch to abort a search before it completes, note that results may still come back resulting in the next match list not being as expected.");
                return;
            }

            standardSearch = true;

            SetLobbyFilter(LobbyFilter);

            var call = SteamMatchmaking.RequestLobbyList();
            m_LobbyMatchList.Set(call, HandleLobbyMatchList);

            SearchStarted.Invoke();
        }

        /// <summary>
        /// Starts a staged search for a matching lobby. Search will only start if no searches are currently running.
        /// </summary>
        /// <param name="LobbyFilter">The metadata of a lobby to search for</param>
        /// <param name="autoCreate">Should the system create a lobby if no matching lobby is found</param>
        /// <returns>True if the search was started, false otherwise.</returns>
        public bool QuickMatch(LobbyHunterFilter LobbyFilter, string onCreateName, bool autoCreate = false)
        {
            if (!callbacksRegistered)
                RegisterCallbacks();

            if (quickMatchSearch || standardSearch)
            {
                return false;
            }

            LobbyName = onCreateName;
            quickMatchCreateOnFail = autoCreate;
            quickMatchSearch = true;
            quickMatchFilter = LobbyFilter;
            quickMatchFilter.distanceOption = ELobbyDistanceFilter.k_ELobbyDistanceFilterClose;
            quickMatchFilter.useDistanceFilter = true;
            quickMatchFilter.requiredOpenSlots = 1;
            quickMatchFilter.useSlotsAvailable = true;
            FindQuickMatch();

            return true;
        }

        /// <summary>
        /// Terminates a quick search process
        /// Note that lobby searches are asynchronious and result may return after the cancelation
        /// </summary>
        public void CancelQuickMatch()
        {
            if (!callbacksRegistered)
                RegisterCallbacks();

            if (quickMatchSearch)
            {
                quickMatchSearch = false;
                Debug.LogWarning("Quick Match search has been canceled, note that results may still come back resulting in the next match list not being as expected.");
            }
        }

        /// <summary>
        /// Terminates a standard search
        /// Note that lobby searches are asynchronious and result may return after the cancelation
        /// </summary>
        public void CancelStandardSearch()
        {
            if (!callbacksRegistered)
                RegisterCallbacks();

            if (standardSearch)
            {
                standardSearch = false;
                Debug.LogWarning("Search has been canceled, note that results may still come back resulting in the next match list not being as expected.");
            }
        }

        /// <summary>
        /// Sends a chat message via Valve's Lobby Chat system
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendChatMessage(string message)
        {
            if (!callbacksRegistered)
                RegisterCallbacks();

            byte[] MsgBody = System.Text.Encoding.UTF8.GetBytes(message);
            SteamMatchmaking.SendLobbyChatMsg(LobbyId, MsgBody, MsgBody.Length);
        }

        /// <summary>
        /// Sets metadata on the lobby, this can only be called by the host of the lobby
        /// </summary>
        /// <param name="key">The key of the metadata to set</param>
        /// <param name="value">The value of the metadata to set</param>
        public void SetLobbyMetadata(string key, string value)
        {
            if (!callbacksRegistered)
                RegisterCallbacks();

            if (!IsHost)
                return;

            if (key == "name")
                LobbyName = value;

            Metadata[key] = value;
            SteamMatchmaking.SetLobbyData(LobbyId, key, value);
        }

        /// <summary>
        /// Sets metadata for the player on the lobby
        /// </summary>
        /// <param name="key">The key of the metadata to set</param>
        /// <param name="value">The value of the metadata to set</param>
        public void SetMemberMetadata(string key, string value)
        {
            if (!callbacksRegistered)
                RegisterCallbacks();

            SteamMatchmaking.SetLobbyMemberData(LobbyId, key, value);
        }

        /// <summary>
        /// Sets the lobby game server e.g. game start using the lobby Host as the server ID
        /// This will trigger GameServerSet on all members of the lobby
        /// This should be called after the server is started
        /// </summary>
        public void SetLobbyGameServer()
        {
            if (!callbacksRegistered)
                RegisterCallbacks();

            SteamMatchmaking.SetLobbyGameServer(LobbyId, 0, 0, LobbyOwner.UserData.SteamId);
        }

        /// <summary>
        /// Sets the lobby game server e.g. game start 
        /// This will trigger GameServerSet on all members of the lobby
        /// This should be called after the server is started
        /// </summary>
        /// <param name="gameServerId">The CSteamID of the steam game server</param>
        public void SetLobbyGameServer(CSteamID gameServerId)
        {
            if (!callbacksRegistered)
                RegisterCallbacks();

            SteamMatchmaking.SetLobbyGameServer(LobbyId, 0, 0, LobbyOwner.UserData.SteamId);
        }

        /// <summary>
        /// Sets the lobby game server e.g. game start
        /// This will trigger GameServerSet on all members of the lobby
        /// This should be called after the server is started
        /// </summary>
        /// <param name="ipAddress">The ip address of the server</param>
        /// <param name="port">The port to be used to connect to the server</param>
        public void SetLobbyGameServer(uint ipAddress, ushort port)
        {
            if (!callbacksRegistered)
                RegisterCallbacks();

            SteamMatchmaking.SetLobbyGameServer(LobbyId, ipAddress, port, CSteamID.Nil);
        }
    }
}
#endif