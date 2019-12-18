﻿#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using System.Collections.Generic;
using System;

namespace HeathenEngineering.SteamTools
{
    [Serializable]
    public struct LobbyHunterLobbyRecord
    {
        public string name;
        public CSteamID lobbyId;
        public int maxSlots;
        public CSteamID hostId;
        public Dictionary<string, string> metaData;
    }
}
#endif