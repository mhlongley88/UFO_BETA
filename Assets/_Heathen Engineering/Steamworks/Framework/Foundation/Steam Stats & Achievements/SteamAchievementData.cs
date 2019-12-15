#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamTools
{
    [CreateAssetMenu(menuName = "Steamworks/Steam Achievement Data")]
    public class SteamAchievementData : ScriptableObject
    {
        public string achievementId;
        [NonSerialized]
        public bool isAchieved;
        [NonSerialized]
        public string displayName;
        [NonSerialized]
        public string displayDescription;
        [NonSerialized]
        public bool hidden;

        public UnityEvent OnUnlock;
        
        public void Unlock()
        { 
            if (!isAchieved)
            {
                isAchieved = true;
                SteamUserStats.SetAchievement(achievementId);
                OnUnlock.Invoke();
            }
        }
    }
}
#endif