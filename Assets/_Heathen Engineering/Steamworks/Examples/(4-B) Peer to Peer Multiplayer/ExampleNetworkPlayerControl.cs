#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS && MIRROR
using HeathenEngineering.SteamTools;
using Steamworks;
using UnityEngine;
using Mirror;

namespace HeathenEngineering.Demo
{
    /// <summary>
    /// Demonstrates the use of the common Network Behaviour and Steam Network Behaviour features
    /// </summary>
    public class ExampleNetworkPlayerControl : NetworkBehaviour
    {
        public HeathenSteamLobbySettings LobbySettings;
        public Transform selfTransform;
        public SteamUserData authorityUser;
        public float speed = 0.25f;
        public SteamUserFullIcon SteamIcon;
        /// <summary>
        /// This is to work around an odd bug in UNET where the first SyncVar on a behaviour is getting a random value assigned
        /// We are looking into the caues and have a Unity Bug submitted for it
        /// </summary>
        [SyncVar]
        public int unityBug = 0;
        [SyncVar]
        public ulong steamId = CSteamID.Nil.m_SteamID;

        /// <summary>
        /// Only called on the client that has authority over this behaviour
        /// </summary>
        public override void OnStartAuthority()
        {
            Debug.Log("On Start Authority");
            //On start where we have authority we set the steamId to be our steamId
            //This must be done on the server so the host simply sets directly, the clients must set the value via command
            //This is true because the value is a SyncVar which updates from Server -> Client ... hosts being both but effectivly server in this case.
            if (LobbySettings.IsHost)
            {
                steamId = SteamUser.GetSteamID().m_SteamID;
            }
            else
            {
                CmdSetSteamId(SteamUser.GetSteamID().m_SteamID);
            }
        }

        //public override void OnSpawnObject(CSteamID authoritySteamId)
        //{
        //    base.OnSpawnObject(authoritySteamId);
        //    steamId = authoritySteamId.m_SteamID;
        //}

        /// <summary>
        /// Called on all clients
        /// </summary>
        public override void OnStartClient()
        {
            Debug.Log("On Start Client");
        }

        private void SetSteamIconData()
        {
            authorityUser = SteamworksFoundationManager.Instance.Settings.GetUserData(new CSteamID(steamId));
            SteamIcon.LinkSteamUser(authorityUser);

            if (string.IsNullOrEmpty(authorityUser.DisplayName))
                authorityUser.DisplayName = "Unknown User";

            Debug.Log("Linking persona data for: [" + steamId.ToString() + "] " + (string.IsNullOrEmpty(authorityUser.DisplayName) ? "Unknown User" : authorityUser.DisplayName));
        }

        private void Update()
        {
            if (steamId > 0 && (authorityUser == null || authorityUser.SteamId.m_SteamID != steamId))
            {
                SetSteamIconData();
            }

            //Only do this if we have authority
            if (hasAuthority)
            {
                //Simple input controls for moving left and right, this is simply to help prove that updates are being sent between clients
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    selfTransform.position += Vector3.left * speed * Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    selfTransform.position += Vector3.right * speed * Time.deltaTime;
                }
            }
        }

        [Command(channel = 0)]
        void CmdSetSteamId(ulong steamId)
        {
            this.steamId = steamId;
        }
    }
}
#endif