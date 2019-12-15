#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using HeathenEngineering.SteamTools;
using HeathenEngineering.SteamTools.Networking;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamTools
{
    public class LobbyRecordBehvaiour : MonoBehaviour
    {
        public UnitySteamIdEvent OnSelected;

        public virtual void SetLobby(LobbyHunterLobbyRecord record, HeathenSteamLobbySettings lobbySettings)
        {

        }
    }
}
#endif
