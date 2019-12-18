#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using System;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamTools
{
    public class SteamAchievementHandler : MonoBehaviour
    {
        public SteamAchievementData achievement;
        public UnityEvent onUnlock;

        private void Start()
        {
            achievement.OnUnlock.AddListener(handleUnlock);
        }

        private void handleUnlock()
        {
            onUnlock.Invoke();
        }
    }
}
#endif