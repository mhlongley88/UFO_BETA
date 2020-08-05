using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;

public class PhotonChatClient : MonoBehaviour, IChatClientListener
{
    ChatClient chatClient;
    string myUserId;
    public static PhotonChatClient instance;
    void Start()
    {
        instance = this;
        chatClient = new ChatClient(this);
        //// Set your favourite region. "EU", "US", and "ASIA" are currently supported.
        //chatClient.ChatRegion = "ASIA";
        //chatClient.Connect(chatAppId, chatAppVersion, new AuthenticationValues(userID));
    }
    void Update()
    {
        chatClient.Service();
    }
    public void Connect(string chatAppId, string chatAppVersion, string userID)
    {
        //chatClient = new ChatClient(this);
        myUserId = userID;
        // Set your favourite region. "EU", "US", and "ASIA" are currently supported.
        chatClient.ChatRegion = "ASIA";
        //chatClient.Connect(chatAppId, chatAppVersion, new AuthenticationValues(userID));
        Debug.Log(chatClient.Connect(chatAppId, chatAppVersion, new AuthenticationValues(userID)));
    }
    public void SendInvitation(string friendId, string roomId)
    {
        chatClient.SendPrivateMessage(friendId, roomId);
    }

    #region IChatClientListener implementation

    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        Debug.Log(message);
        //throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {

        //throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        Debug.Log("Connected");
       // SendInvitation("76561199002318893", "111");
        //throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log(state.ToString());
       // throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("Private Message M: " + message.ToString());
        Debug.Log("Private Message S: " + sender);
        if (sender != myUserId)
        {
            
            SteamScript.instance.InviteRecieved(sender, message.ToString());
        }
        //throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
