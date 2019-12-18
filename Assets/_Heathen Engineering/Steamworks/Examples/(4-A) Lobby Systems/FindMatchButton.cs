#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using HeathenEngineering.SteamTools;
using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.Demo
{
    public class FindMatchButton : MonoBehaviour
    {
        public HeathenSteamLobbySettings LobbySettings;
        public LobbyHunterFilter QuickMatchFilter;
        public Button quickMatchButton;
        public Text quickMatchLabel;

        void Update()
        {
            quickMatchButton.interactable = !LobbySettings.Manager.IsSearching && !LobbySettings.Manager.IsQuickSearching;
            if (quickMatchButton.interactable)
            {
                if (!LobbySettings.InLobby)
                    quickMatchLabel.text = "Quick Match";
                else
                    quickMatchLabel.text = "Leave Lobby";
            }
            else
            {
                quickMatchLabel.text = "Searching";
            }
        }

        public void SimpleFindMatch()
        {
            if (!LobbySettings.InLobby)
                LobbySettings.Manager.QuickMatch(QuickMatchFilter, SteamworksFoundationManager.Instance.UserData.DisplayName, true);
            else
                LobbySettings.Manager.LeaveLobby();
        }

        public void GetHelp()
        {
            Application.OpenURL("https://partner.steamgames.com/doc/features/multiplayer/matchmaking");
        }
    }
}
#endif