#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using System;
using UnityEngine;

namespace HeathenEngineering.SteamTools
{
    [Serializable]
    [CreateAssetMenu(menuName = "Steamworks/Steam Int Stat Data")]
    public class SteamIntStatData : SteamStatData
    {
        public int Value;

        public override StatDataType DataType { get { return StatDataType.Int; } }

        public override float GetFloatValue()
        {
            return Value;
        }

        public override int GetIntValue()
        {
            return Value;
        }

        public override void SetFloatStat(float value)
        {
            Value = (int)value;
            SteamUserStats.SetStat(statName, Value);
        }

        public override void SetIntStat(int value)
        {
            Value = value;
            SteamUserStats.SetStat(statName, value);
        }

        public override void UpdateValue(int value)
        {
            if (value != Value)
            {
                Value = value;
                ValueChanged.Invoke(this);
            }
        }

        public override void UpdateValue(float value)
        {
            var v = (int)value;
            if (v != Value)
            {
                Value = v;
                ValueChanged.Invoke(this);
            }
        }
    }
}
#endif