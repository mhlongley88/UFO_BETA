using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Steamworks;
using Photon.Pun;
public class SteamGameInvite : MonoBehaviour
{
 /*   public static SteamGameInvite instance;
    protected Callback<GameRichPresenceJoinRequested_t> m_GameRichPresenceJoinRequested;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //SteamFriends.ActivateGameOverlay("friends");
        m_GameRichPresenceJoinRequested = Callback<GameRichPresenceJoinRequested_t>.Create(OnGameRichPresenceJoinRequested);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGameRichPresenceJoinRequested(GameRichPresenceJoinRequested_t pCallback)
    {
        Debug.Log("OnGameRichPresenceJoinRequested");
        //JoinGame(pCallback.m_rgchConnect);
        string s = GetInvitedRoomId(pCallback.m_steamIDFriend);
        JoinGame(s);
    }

    public void SyncRoomIdAccrossSteam(string roomId)
    {
        SteamFriends.SetRichPresence("InvitationRoomId", roomId);
    }

    public string GetInvitedRoomId(CSteamID inviteFromId)
    {
        return SteamFriends.GetFriendRichPresence(inviteFromId, "InvitationRoomId");
    }


    void JoinGame(string connStr)
    {
        PhotonNetwork.JoinRoom(connStr);
    }*/
}
