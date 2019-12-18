#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using System;
using UnityEngine;

namespace HeathenEngineering.SteamTools
{
    [Serializable]
    public abstract class SteamStatData : ScriptableObject
    {
        public string statName;
        public abstract StatDataType DataType { get; }
        public abstract void UpdateValue(int value);
        public abstract void UpdateValue(float value);
        public abstract int GetIntValue();
        public abstract float GetFloatValue();
        public abstract void SetIntStat(int value);
        public abstract void SetFloatStat(float value);
        public UnityStatEvent ValueChanged;

        public enum StatDataType
        {
            Int,
            Float
        }
    }
}
#endif
