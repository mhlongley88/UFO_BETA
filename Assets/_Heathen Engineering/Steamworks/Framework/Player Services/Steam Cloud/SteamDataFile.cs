#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using System;
using UnityEngine;

namespace HeathenEngineering.SteamTools
{
    /// <summary>
    /// Defines file data relative to Steam Remote Storage
    /// </summary>
    [Serializable]
    public class SteamDataFile
    {
        public SteamDataFileAddress address;
        [HideInInspector]
        public byte[] binaryData;
        [HideInInspector]
        public SteamAPICall_t? apiCall;
        [HideInInspector]
        public EResult result = EResult.k_EResultPending;
        [HideInInspector]
        public SteamDataLibrary linkedLibrary;

        public void ReadFromLibrary(SteamDataLibrary dataLibrary)
        {
            linkedLibrary = dataLibrary;
            dataLibrary.SyncToBuffer(out binaryData);
        }

        public void WriteToLibrary(SteamDataLibrary dataLibrary)
        {
            linkedLibrary = dataLibrary;
            dataLibrary.SyncFromBuffer(binaryData);
        }

        #region Encoding
        public string EncodeUTF8()
        {
            if (binaryData.Length > 0)
                return System.Text.Encoding.UTF8.GetString(binaryData);
            else
                return string.Empty;
        }

        public string EncodeUTF32()
        {
            if (binaryData.Length > 0)
                return System.Text.Encoding.UTF32.GetString(binaryData);
            else
                return string.Empty;
        }

        public string EncodeUnicode()
        {
            if (binaryData.Length > 0)
                return System.Text.Encoding.Unicode.GetString(binaryData);
            else
                return string.Empty;
        }

        public string EncodeDefault()
        {
            if (binaryData.Length > 0)
                return System.Text.Encoding.Default.GetString(binaryData);
            else
                return string.Empty;
        }

        public string EncodeASCII()
        {
            if (binaryData.Length > 0)
                return System.Text.Encoding.ASCII.GetString(binaryData);
            else
                return string.Empty;
        }
        #endregion
    }
}
#endif