#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS && MIRROR
using Mirror;
using System;
using UnityEngine.Events;

namespace HeathenEngineering.SteamTools.MirrorNetworking
{
    [Serializable]
    public class UnityNetworkClientEvent : UnityEvent<NetworkClient>
    { }
}
#endif