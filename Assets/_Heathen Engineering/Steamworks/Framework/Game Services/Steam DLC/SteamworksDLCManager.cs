#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HeathenEngineering.SteamTools
{
    public class SteamworksDLCManager : MonoBehaviour
    {
        public List<SteamDLCData> DLC = new List<SteamDLCData>();

        #region Callbacks
        private Callback<DlcInstalled_t> m_DlcInstalled;
        #endregion

        private void Start()
        {
            m_DlcInstalled = Callback<DlcInstalled_t>.Create(HandleDlcInstalled);

            UpdateAll();
        }

        private void HandleDlcInstalled(DlcInstalled_t param)
        {
            var target = DLC.FirstOrDefault(p => p.AppId == param.m_nAppID);
            if(target != null)
            {
                target.UpdateStatus();
            }
        }

        public void UpdateAll()
        {
            foreach (var dlc in DLC)
            {
                dlc.UpdateStatus();
            }
        }

        public SteamDLCData GetDLC(AppId_t AppId)
        {
            return DLC.FirstOrDefault(p => p.AppId == AppId);
        }

        public SteamDLCData GetDLC(string name)
        {
            return DLC.FirstOrDefault(p => p.name == name);
        }
    }
}
#endif