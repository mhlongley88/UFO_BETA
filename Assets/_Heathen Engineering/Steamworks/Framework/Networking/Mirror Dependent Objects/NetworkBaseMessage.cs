#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS && MIRROR
using UnityEngine;

namespace HeathenEngineering.SteamTools.MirrorNetworking
{
    public class NetworkBaseMessage : ScriptableObject
    {
        /*
         * sould have a TypeId of type short and an OnRecieved of a type derived from UnityEvent<T> where T is derived from BaseMessage
         * 
         * Example:
         * public short TypeId;
         * public UnityStringMessageEvent OnRecieved;
         *  
         * Note:
         * This enables the custom editor script to manage the message
         */
        public short typeId;
        [Tooltip("The default mode the server will use when sending this message to clients")]
        public int ServerSendMode = 0;
        public virtual void RegisterHandler()
        { }
        public virtual void UnregisterHandler()
        { }
    }
}
#endif