#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS && MIRROR
using Mirror;
using UnityEngine;

namespace HeathenEngineering.SteamTools.MirrorNetworking
{
    [CreateAssetMenu(menuName = "Steamworks/Network/Integer Message")]
    public class NetworkIntegerMessage : NetworkBaseMessage
    {
        public UnityIntegerMessageEvent OnRecieved;
        
        public void Send(int message)
        {
            if (NetworkServer.active)
            {
                NetworkServer.SendToAll(new IntegerMessage(message), ServerSendMode);
            }
            else
            {
                NetworkClient.Send(new IntegerMessage(message));
            }
        }

        public void Send(IntegerMessage message)
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

        public void Send(IntegerMessage message, int sendMode)
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
                NetworkServer.RegisterHandler<IntegerMessage>(handleRecieved);
            else
            {
                NetworkClient.RegisterHandler<IntegerMessage>(handleRecieved);
            }
        }

        public override void UnregisterHandler()
        {
            if (NetworkServer.active)
            {
                NetworkServer.UnregisterHandler<IntegerMessage>();
            }
            else
            {
                    NetworkClient.UnregisterHandler<IntegerMessage>();
            }
        }

        private void handleRecieved(NetworkConnection connection, IntegerMessage msg)
        {
            OnRecieved.Invoke(msg);
        }
    }
}
#endif