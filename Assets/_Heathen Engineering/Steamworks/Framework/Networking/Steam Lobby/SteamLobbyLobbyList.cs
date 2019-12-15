#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using System.Collections.Generic;
using System;

namespace HeathenEngineering.SteamTools
{
    [Serializable]
    public class SteamLobbyLobbyList : List<LobbyHunterLobbyRecord>
    {
        Dictionary<string, string> GetLobbyMetaData(CSteamID id)
        {
            if (this.Exists(p => p.lobbyId.m_SteamID == id.m_SteamID))
                return this.Find(p => p.lobbyId.m_SteamID == id.m_SteamID).metaData;
            else
                return new Dictionary<string, string>();
        }
    }
}
#endif
