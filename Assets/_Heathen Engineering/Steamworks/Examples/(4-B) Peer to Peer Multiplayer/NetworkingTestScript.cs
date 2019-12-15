#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS && MIRROR
using HeathenEngineering.SteamTools;
using Mirror;
using UnityEngine;

namespace HeathenEngineering.Demo
{
    public class NetworkingTestScript : MonoBehaviour
    {
        public HeathenSteamLobbySettings LobbySettings;
        public UnityEngine.UI.Button ServerButton;
        public UnityEngine.UI.Text ButtonText;

        private void Update()
        {
            if(LobbySettings.IsHost)
            {
                ServerButton.interactable = true;
                if(NetworkManager.singleton.isNetworkActive)
                {
                    ButtonText.text = "Stop Hosting";
                }
                else
                {
                    ButtonText.text = "Start Hosting";
                }
            }
            else
            {
                if (NetworkManager.singleton.isNetworkActive)
                {
                    ServerButton.interactable = true;
                    ButtonText.text = "Stop Client";
                }
                else
                {
                    ServerButton.interactable = false;
                    ButtonText.text = "Waiting for host";
                }
            }
        }

        public void OnButtonClick()
        {
            if(LobbySettings.IsHost)
            {
                if (NetworkManager.singleton.isNetworkActive)
                {
                    NetworkManager.singleton.StopHost();
                }
                else
                {
                    NetworkManager.singleton.StartHost();
                }
            }
            else
            {
                if (NetworkManager.singleton.isNetworkActive)
                {
                    NetworkManager.singleton.StopClient();
                }
            }
        }

        public void OnGameReady()
        {
            if(!LobbySettings.IsHost)
            {
                Debug.Log("Recieved Game Start message from Steam Lobby ...\nStarting up the Network Client.");
                //The Heathen Steam Transport uses CSteamIDs as addresses e.g. we are connecting to Steam Users not IP addresses
                NetworkManager.singleton.networkAddress = LobbySettings.LobbyOwner.UserData.SteamId.m_SteamID.ToString();
                NetworkManager.singleton.StartClient();
                NetworkManager.singleton.OnStartClient();
            }
        }

        public void GetHelp()
        {
            Application.OpenURL("https://github.com/vis2k/Mirror");
        }
    }
}
#endif