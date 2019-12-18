#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using System;

namespace HeathenEngineering.SteamTools
{
    /// <summary>
    /// Defines the address of a data file as stored in Steam Remote Storage
    /// </summary>
    [Serializable]
    public struct SteamDataFileAddress : IEquatable<SteamDataFileAddress>
    {
        public int fileIndex;
        public int fileSize;
        public string fileName;
        public DateTime UtcTimestamp;
        public DateTime LocalTimestamp
        {
            get
            {
                return UtcTimestamp.ToLocalTime();
            }
            set
            {
                UtcTimestamp = value.ToUniversalTime();
            }
        }

        public static bool operator ==(SteamDataFileAddress obj1, SteamDataFileAddress obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(SteamDataFileAddress obj1, SteamDataFileAddress obj2)
        {
            return !obj1.Equals(obj2);
        }

        public bool Equals(SteamDataFileAddress other)
        {
            return fileIndex == other.fileIndex && fileName == other.fileName && fileSize == other.fileSize;
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == GetType() && Equals((SteamDataFileAddress)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = fileIndex.GetHashCode();
                hashCode = (hashCode * 397) ^ fileSize.GetHashCode();
                hashCode = (hashCode * 397) ^ fileName.GetHashCode();
                return hashCode;
            }
        }
    }
}
#endif