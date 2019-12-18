#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS && MIRROR
using Mirror;
using UnityEngine;

namespace HeathenEngineering.SteamTools.MirrorNetworking
{
    [CreateAssetMenu(menuName = "Steamworks/Network/Empty Message")]
    public class NetworkEmptyMessage : NetworkBaseMessage
    {
        public UnityEmptyMessageEvent OnRecieved;
        
        public void Send()
        {
            if (NetworkServer.active)
            {
                NetworkServer.SendToAll(new EmptyMessage(), ServerSendMode);
            }
            else
            {
                NetworkClient.Send(new EmptyMessage());
            }
        }

        public void Send(EmptyMessage message)
        {
            if (NetworkServer.active)
            {
                NetworkServer.SendToAll(new EmptyMessage(), ServerSendMode);
            }
            else
            {
                NetworkClient.Send(message);
            }
        }

        public void Send(EmptyMessage message, int sendMode)
        {
            if (NetworkServer.active)
            {
                NetworkServer.SendToAll(new EmptyMessage(), sendMode);
            }
            else
            {
                NetworkClient.Send(message);
            }
        }

        public override void RegisterHandler()
        {
            if (NetworkServer.active)
                NetworkServer.RegisterHandler<EmptyMessage>(handleRecieved);
            else
            {
                    NetworkClient.RegisterHandler<EmptyMessage>(handleRecieved);
            }
        }

        public override void UnregisterHandler()
        {
            if (NetworkServer.active)
            {
                NetworkServer.UnregisterHandler<EmptyMessage>();
            }
            else
            {
                NetworkClient.UnregisterHandler<EmptyMessage>();
            }
        }

        private void handleRecieved(NetworkConnection connection, EmptyMessage msg)
        {
            OnRecieved.Invoke(msg);
        }
    }
}
#endif