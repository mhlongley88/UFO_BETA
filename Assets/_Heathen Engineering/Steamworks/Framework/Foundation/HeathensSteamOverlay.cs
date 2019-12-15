#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using System;

namespace HeathenEngineering.SteamTools
{
    [Serializable]
    public class HeathensSteamOverlay
    {
        public bool IsEnabled
        {
            get
            {
                return Steamworks.SteamUtils.IsOverlayEnabled();
            }
        }

        private bool _OverlayOpen = false;
        public bool IsOpen
        {
            get
            {
                return _OverlayOpen;
            }
        }

        public void HandleOnOverlayOpen(GameOverlayActivated_t data)
        {
            _OverlayOpen = data.m_bActive == 1;
        }

        public void Invite(CSteamID lobbyId)
        {
            Steamworks.SteamFriends.ActivateGameOverlayInviteDialog(lobbyId);
        }

        /// <summary>
        /// Opens the overlay to the current games store page
        /// </summary>
        public void OpenStore()
        {
            OpenStore(SteamworksFoundationManager.Instance.Settings != null ? SteamworksFoundationManager.Instance.Settings.ApplicationId : AppId_t.Invalid, EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
        }

        /// <summary>
        /// Opens the overlay to the indicated applicaitons store page
        /// </summary>
        /// <param name="appId">Steam App Id of the applicaiton to open</param>
        public void OpenStore(uint appId)
        {
            OpenStore(new AppId_t(appId), EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
        }

        /// <summary>
        /// Opens the store page to the indicated applications store page with store options
        /// </summary>
        /// <param name="appId">Steam App Id of the applicaiton to open</param>
        /// <param name="flag">Modifies behavior of the page when opened</param>
        public void OpenStore(uint appId, EOverlayToStoreFlag flag)
        {
            OpenStore(new AppId_t(appId), flag);
        }

        /// <summary>
        /// Opens the store page to the indicated applications store page with store options
        /// </summary>
        /// <param name="appId">Steam App Id of the applicaiton to open</param>
        /// <param name="flag">Modifies behavior of the page when opened</param>
        public void OpenStore(AppId_t appId, EOverlayToStoreFlag flag)
        {
            Steamworks.SteamFriends.ActivateGameOverlayToStore(appId, flag);
        }

        /// <summary>
        /// From Steamworks ActivateGameOverlay
        /// </summary>
        /// <remarks>
        /// See https://partner.steamgames.com/doc/api/ISteamFriends#ActivateGameOverlay for details
        /// </remarks>
        /// <param name="dialog"></param>
        public void Open(string dialog)
        {
            Steamworks.SteamFriends.ActivateGameOverlay(dialog);
        }

        public void OpenWebPage(string URL)
        {
            Steamworks.SteamFriends.ActivateGameOverlayToWebPage(URL);
        }

        public void OpenFriends()
        {
            Steamworks.SteamFriends.ActivateGameOverlay("friends");
        }

        public void OpenCommunity()
        {
            Steamworks.SteamFriends.ActivateGameOverlay("community");
        }

        public void OpenPlayers()
        {
            Steamworks.SteamFriends.ActivateGameOverlay("players");
        }

        public void OpenSettings()
        {
            Steamworks.SteamFriends.ActivateGameOverlay("settings");
        }

        public void OpenOfficialGameGroup()
        {
            Steamworks.SteamFriends.ActivateGameOverlay("officialgamegroup");
        }

        public void OpenStats()
        {
            Steamworks.SteamFriends.ActivateGameOverlay("stats");
        }

        public void OpenAchievements()
        {
            Steamworks.SteamFriends.ActivateGameOverlay("achievements");
        }

        public void OpenChat(CSteamID user)
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("Chat", user);
        }

        public void OpenProfile(CSteamID user)
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("steamid", user);
        }

        public void OpenTrade(CSteamID user)
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("jointrade", user);
        }

        public void OpenStats(CSteamID user)
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("stats", user);
        }

        public void OpenAchievements(CSteamID user)
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("achievements", user);
        }

        public void OpenFriendAdd(CSteamID user)
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("friendadd", user);
        }

        public void OpenFriendRemove(CSteamID user)
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("friendremove", user);
        }

        public void OpenRequestAccept(CSteamID user)
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("friendrequestaccept", user);
        }

        public void OpenRequestIgnore(CSteamID user)
        {
            Steamworks.SteamFriends.ActivateGameOverlayToUser("friendrequestignore", user);
        }
    }
}
#endif