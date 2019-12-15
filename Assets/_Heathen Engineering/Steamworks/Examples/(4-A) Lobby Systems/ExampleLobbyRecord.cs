#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using HeathenEngineering.SteamTools;
using Steamworks;
using UnityEngine;

public class ExampleLobbyRecord : LobbyRecordBehvaiour
{
    public HeathenSteamLobbySettings LobbySettings;
    public UnityEngine.UI.Text lobbyId;
    public UnityEngine.UI.Text lobbySize;
    public UnityEngine.UI.Button connectButton;
    public UnityEngine.UI.Text buttonLabel;

    [Header("List Record")]
    public LobbyHunterLobbyRecord record;

    public override void SetLobby(LobbyHunterLobbyRecord record, HeathenSteamLobbySettings lobbySettings)
    {
        LobbySettings = lobbySettings;
        this.record = record;
        lobbyId.text = string.IsNullOrEmpty(record.name) ? "<unknown>" : record.name;
        lobbySize.text = record.maxSlots.ToString();
    }

    public void Selected()
    {
        OnSelected.Invoke(record.lobbyId);
    }

    private void Update()
    {
        if(record.lobbyId != CSteamID.Nil
            && LobbySettings.lobbyId.m_SteamID == record.lobbyId.m_SteamID)
        {
            connectButton.interactable = false;
            buttonLabel.text = "You are here!";
        }
        else
        {
            connectButton.interactable = true;
            buttonLabel.text = "Join lobby!";
        }
    }
}
#endif