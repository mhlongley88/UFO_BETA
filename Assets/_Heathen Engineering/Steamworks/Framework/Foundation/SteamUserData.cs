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
    [Serializable]
    [CreateAssetMenu(menuName = "Steamworks/Steam User Data")]
    public class SteamUserData : ScriptableObject
    {
        public CSteamID SteamId;
        public string DisplayName;
        public bool IconLoaded = false;
        public Texture2D Avatar;
        public EPersonaState State;
        public bool InGame;
        public FriendGameInfo_t GameInfo;

        public UnityEvent OnAvatarLoaded = new UnityEvent();
        public UnityEvent OnAvatarChanged = new UnityEvent();
        public UnityEvent OnNameChanged = new UnityEvent();
        public UnityEvent OnStateChange = new UnityEvent();
        public UnityEvent OnComeOnline = new UnityEvent();
        public UnityEvent OnGoneOffline = new UnityEvent();
        public UnityEvent OnGameChanged = new UnityEvent();

        public void ClearData()
        {
            SteamId = new CSteamID();
            DisplayName = string.Empty;
            IconLoaded = false;
            Avatar = null;
            State = EPersonaState.k_EPersonaStateOffline;
            InGame = false;
            GameInfo = new FriendGameInfo_t();
        }

        public void OpenChat()
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("Chat", SteamId);
        }

        public void OpenProfile()
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("steamid", SteamId);
        }

        public void OpenTrade()
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("jointrade", SteamId);
        }

        public void OpenStats()
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("stats", SteamId);
        }

        public void OpenAchievements()
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("achievements", SteamId);
        }

        public void OpenFriendAdd()
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("friendadd", SteamId);
        }

        public void OpenFriendRemove()
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("friendremove", SteamId);
        }

        public void OpenRequestAccept()
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("friendrequestaccept", SteamId);
        }

        public void OpenRequestIgnore()
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("friendrequestignore", SteamId);
        }

        /// <summary>
        /// Send this user a Steam Friend Chat message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool SendMessage(string message)
        {
            return SteamFriends.ReplyToFriendMessage(SteamId, message);
        }
    }
}
#endif