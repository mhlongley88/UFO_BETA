#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS && MIRROR
using Mirror;
using System;
using UnityEngine;

namespace HeathenEngineering.SteamTools.MirrorNetworking
{
    [Serializable]
    [CreateAssetMenu(menuName = "Steamworks/Network/String Message")]
    public class NetworkStringMessage : NetworkBaseMessage
    {
        public UnityStringMessageEvent OnRecieved;
        
        public void Send(string message)
        {
            if (NetworkServer.active)
            {
                NetworkServer.SendToAll(new StringMessage(message), ServerSendMode);
            }
            else
            {
                NetworkClient.Send(new StringMessage(message));
            }
        }

        public void Send(StringMessage message)
        {
            if (NetworkServer.active)
            {
                NetworkServer.SendToAll(message, ServerSendMode);
            }
            else
            {
                NetworkClient.Send(message);
            }
        }

        public void Send(StringMessage message, int sendMode)
        {
            if (NetworkServer.active)
            {
                NetworkServer.SendToAll(message, sendMode);
            }
            else
            {
                NetworkClient.Send(message);
            }
        }

        public override void RegisterHandler()
        {
            if (NetworkServer.active)
            {
                NetworkServer.RegisterHandler<StringMessage>(handleRecieved);
            }
            else
            {
                NetworkClient.RegisterHandler<StringMessage>(handleRecieved);
            }
        }

        public override void UnregisterHandler()
        {
            if (NetworkServer.active)
            {
                NetworkServer.UnregisterHandler<StringMessage>();
            }
            else
            {
                NetworkClient.UnregisterHandler<StringMessage>();
            }
        }

        private void handleRecieved(NetworkConnection connection, StringMessage msg)
        {
            OnRecieved.Invoke(msg);
        }
    }
}
#endif