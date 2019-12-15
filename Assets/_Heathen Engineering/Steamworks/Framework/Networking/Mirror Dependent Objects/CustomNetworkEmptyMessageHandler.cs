#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS && MIRROR
using UnityEngine;
using UnityEngine.Events;
using Mirror;

namespace HeathenEngineering.SteamTools.MirrorNetworking
{
    public class CustomNetworkEmptyMessageHandler : MonoBehaviour
    {
        public NetworkEmptyMessage message;
        public UnityEvent onRecieved;

        private void Start()
        {
            message.OnRecieved.AddListener(handleMessage);
        }

        private void handleMessage(EmptyMessage arg0)
        {
            onRecieved.Invoke();
        }
    }
}
#endif