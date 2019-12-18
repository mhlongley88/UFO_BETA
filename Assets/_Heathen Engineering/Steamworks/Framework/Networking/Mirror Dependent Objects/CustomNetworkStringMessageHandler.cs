#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS && MIRROR
using HeathenEngineering.Serializable;
using UnityEngine;
using Mirror;

namespace HeathenEngineering.SteamTools.MirrorNetworking
{
    public class CustomNetworkStringMessageHandler : MonoBehaviour
    {
        public NetworkStringMessage message;
        public UnityStringEvent onRecieved;

        private void Start()
        {
            message.OnRecieved.AddListener(handleMessage);
        }

        private void handleMessage(StringMessage arg0)
        {
            onRecieved.Invoke(arg0.value);
        }
    }
}
#endif