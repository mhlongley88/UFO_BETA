#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using System;
using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.SteamTools
{
    public class SteamDataFileRecord : Button
    {
        [Header("Display Data")]
        public Text FileName;
        public Text Timestamp;
        public GameObject SelectedIndicator;
        public SteamDataFileAddress Address;

        [HideInInspector]
        public SteamDataFileList parentList;

        protected override void Start()
        {
            onClick.AddListener(HandleClick);
        }

        private void HandleClick()
        {
            if (parentList != null)
                parentList.SelectedFile = Address;
        }

        private void Update()
        {
            if(parentList != null && parentList.SelectedFile.HasValue && parentList.SelectedFile.Value.fileName == Address.fileName)
            {
                if (!SelectedIndicator.activeSelf)
                    SelectedIndicator.SetActive(true);
            }
            else
            {
                if (SelectedIndicator.activeSelf)
                    SelectedIndicator.SetActive(false);
            }
        }
    }
}
#endif