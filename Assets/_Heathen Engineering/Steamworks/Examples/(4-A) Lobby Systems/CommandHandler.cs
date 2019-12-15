#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using System;
using HeathenEngineering.Scriptable;
using HeathenEngineering.SteamTools;
using HeathenEngineering.SteamTools.UI;
using UnityEngine;

public class CommandHandler : MonoBehaviour
{
    public SteamSettings steamSettings;
    public SteamworksLobbyChat lobbyChat;
    public GameEvent sayMyNameEvent;
    public StringGameEvent echoThisEvent;

    private void Start()
    {
        sayMyNameEvent.actions.Add(SayMyName);
        echoThisEvent.stringActions.Add(echoThisMessage);
    }

    private void echoThisMessage(string message)
    {
        lobbyChat.SendSystemMessage("Heathen Engineer", "You want me to say \"" + message + "\"\nOkay " + message.ToUpper() + "!!!");
    }

    private void SayMyName()
    {
        lobbyChat.SendSystemMessage("Heathen Engineer", "Your name is " + steamSettings.UserData.DisplayName);
    }
}
#endif
