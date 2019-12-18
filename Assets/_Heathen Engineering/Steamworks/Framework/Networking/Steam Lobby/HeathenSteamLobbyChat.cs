#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using HeathenEngineering.Tools;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HeathenEngineering.SteamTools
{
    public class HeathenSteamLobbyChat : HeathenUIBehaviour
    {
        public HeathenSteamLobbySettings LobbySettings;
        public UnityEngine.UI.ScrollRect scrollRect;
        public RectTransform collection;
        public UnityEngine.UI.InputField input;
        public int maxMessages;
        public bool showTimeStamp = true;
        public bool allwaysShowTimeStamp = false;
        public bool sendOnKeyCode = false;
        public KeyCode SendCode = KeyCode.Return;
        public string timeStampFormat = "HH:mm:ss";
        public HeathenChatMessage messagePrototype;

        [HideInInspector]
        public List<HeathenChatMessage> messages;

        [Header("Events")]
        public UnityHeathenChatMessageEvent OnMessageRecieved;
        public UnityPersonaEvent OnPersonaLeftClick;
        public UnityPersonaEvent OnPersonaMiddleClick;
        public UnityPersonaEvent OnPersonaRightClick;
        public UnityPersonaEvent OnPersonaLeftDoubleClick;
        public UnityPersonaEvent OnPersonaMiddleDoubleClick;
        public UnityPersonaEvent OnPersonaRightDoubleClick;

        private Callback<LobbyChatMsg_t> m_LobbyChatMsg;

        private void Start()
        {
            m_LobbyChatMsg = Callback<LobbyChatMsg_t>.Create(HandleLobbyChatMessage);
        }

        private void Update()
        {
            if(EventSystem.current.currentSelectedGameObject == input.gameObject && Input.GetKeyDown(SendCode))
            {
                SendChatMessage();
            }
        }

        /// <summary>
        /// Refresh the message entry settings regarding time stamps
        /// This must be called anytime the setting is changed
        /// </summary>
        public void RefreshTimeStampSettings()
        {
            foreach(var m in messages)
            {
                m.ShowStamp = showTimeStamp;
                m.AllwaysShowStamp = allwaysShowTimeStamp;

                if (showTimeStamp && allwaysShowTimeStamp)
                    m.timeRecieved.gameObject.SetActive(true);
                else if (!showTimeStamp)
                    m.timeRecieved.gameObject.SetActive(false);
            }
        }

        private void HandleLobbyChatMessage(LobbyChatMsg_t pCallback)
        {
            CSteamID SteamIDUser;
            byte[] Data = new byte[4096];
            EChatEntryType ChatEntryType;
            int ret = SteamMatchmaking.GetLobbyChatEntry((CSteamID)pCallback.m_ulSteamIDLobby, (int)pCallback.m_iChatID, out SteamIDUser, Data, Data.Length, out ChatEntryType);
            byte[] truncated = new byte[ret];
            Array.Copy(Data, truncated, ret);
            SteamUserData userData = SteamworksFoundationManager._GetUserData(SteamIDUser);
            string message = System.Text.Encoding.UTF8.GetString(truncated);

            var go = Instantiate(messagePrototype.gameObject, collection);
            var msg = go.GetComponent<HeathenChatMessage>();
            msg.PersonaButton.LinkSteamUser(userData);
            msg.Message.text = message;
            msg.timeStamp = DateTime.Now;
            msg.ShowStamp = showTimeStamp;
            msg.AllwaysShowStamp = allwaysShowTimeStamp;
            msg.timeRecieved.text = msg.timeStamp.ToString(timeStampFormat);
            if (!showTimeStamp)
                msg.timeRecieved.gameObject.SetActive(false);
            if (showTimeStamp && allwaysShowTimeStamp)
                msg.timeRecieved.gameObject.SetActive(true);

            msg.PersonaButton.OnLeftClick.AddListener(HandleLeftClick);
            msg.PersonaButton.OnMiddleClick.AddListener(HandleMiddleClick);
            msg.PersonaButton.OnRightClick.AddListener(HandleRightClick);
            msg.PersonaButton.OnLeftDoubleClick.AddListener(HandleLeftDoubleClick);
            msg.PersonaButton.OnMiddleDoubleClick.AddListener(HandleMiddleDoubleClick);
            msg.PersonaButton.OnRightDoubleClick.AddListener(HandleRightDoubleClick);

            messages.Add(msg);

            if(messages.Count > 1 && messages[messages.Count - 2].PersonaButton.UserData.SteamId.m_SteamID == userData.SteamId.m_SteamID)
            {
                //Previous message was from this user ... so we should hide the persona icon
                msg.PersonaButton.gameObject.SetActive(false);
            }

            Canvas.ForceUpdateCanvases();
            if (messages.Count > maxMessages)
            {
                //We have to many messages now so trim the oldest and check the next to see if we need to enable the persona icon
                var firstLine = messages[0];
                messages.Remove(firstLine);
                Destroy(firstLine.gameObject);
                firstLine = messages[0];
                firstLine.PersonaButton.gameObject.SetActive(true);
            }
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;

            OnMessageRecieved.Invoke(msg);
        }

        public void SendChatMessage(string message)
        {
            if (LobbySettings.InLobby)
            {
                LobbySettings.SendChatMessage(message);
                input.ActivateInputField();
            }
        }

        public void SendChatMessage()
        {
            if (!string.IsNullOrEmpty(input.text) && LobbySettings.InLobby)
            {
                SendChatMessage(input.text);
                input.text = string.Empty;
            }
            else
            {
                if (!LobbySettings.InLobby)
                    Debug.LogWarning("Attempted to send a lobby chat message without an established connection");
            }
        }

        private void HandleLeftClick(SteamUserData userData)
        {
            OnPersonaLeftClick.Invoke(userData);
        }

        private void HandleMiddleClick(SteamUserData userData)
        {
            OnPersonaMiddleClick.Invoke(userData);
        }

        private void HandleRightClick(SteamUserData userData)
        {
            OnPersonaRightClick.Invoke(userData);
        }

        private void HandleLeftDoubleClick(SteamUserData userData)
        {
            OnPersonaLeftDoubleClick.Invoke(userData);
        }

        private void HandleMiddleDoubleClick(SteamUserData userData)
        {
            OnPersonaMiddleDoubleClick.Invoke(userData);
        }

        private void HandleRightDoubleClick(SteamUserData userData)
        {
            OnPersonaRightDoubleClick.Invoke(userData);
        }
    }
}
#endif
