#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using UnityEngine;
using HeathenEngineering.Tools;
using HeathenEngineering.Scriptable;

namespace HeathenEngineering.SteamTools
{
    public class SteamUserFullIcon : HeathenUIBehaviour
    {
        public SteamUserData UserData;
        public BoolReference ShowStatusLabel;

        [Header("References")]
        public UnityEngine.UI.RawImage Avatar;
        public UnityEngine.UI.Text PersonaName;
        public UnityEngine.UI.Text StatusLabel;
        public UnityEngine.UI.Image IconBorder;
        public GameObject StatusLabelContainer;
        public bool ColorThePersonaName = true;
        public bool ColorTheStatusLabel = true;

        [Header("Border Colors")]
        public ColorReference OfflineColor;
        public ColorReference OnlineColor;
        public ColorReference AwayColor;
        public ColorReference BuisyColor;
        public ColorReference SnoozeColor;
        public ColorReference WantPlayColor;
        public ColorReference WantTradeColor;
        public ColorReference InGameColor;
        public ColorReference ThisGameColor;

        private void Start()
        {
            if (UserData != null)
                LinkSteamUser(UserData);
        }

        private void Update()
        {
            if (ShowStatusLabel.Value != StatusLabelContainer.activeSelf)
                StatusLabelContainer.SetActive(ShowStatusLabel.Value);
        }

        public void LinkSteamUser(SteamUserData newUserData)
        {
            if (UserData != null)
            {
                if (UserData.OnAvatarChanged != null)
                    UserData.OnAvatarChanged.RemoveListener(handleAvatarChange);
                if (UserData.OnStateChange != null)
                    UserData.OnStateChange.RemoveListener(handleStateChange);
                if (UserData.OnNameChanged != null)
                    UserData.OnNameChanged.RemoveListener(handleNameChanged);
                if (UserData.OnAvatarLoaded != null)
                    UserData.OnAvatarLoaded.RemoveListener(handleAvatarChange);
            }

            UserData = newUserData;
            handleAvatarChange();
            handleNameChanged();
            handleStateChange();

            if (UserData != null)
            {
                if (!UserData.IconLoaded)
                    SteamworksFoundationManager.Instance.Settings.RefreshAvatar(UserData);

                Avatar.texture = UserData.Avatar;
                if (UserData.OnAvatarChanged == null)
                    UserData.OnAvatarChanged = new UnityEngine.Events.UnityEvent();
                UserData.OnAvatarChanged.AddListener(handleAvatarChange);
                if (UserData.OnStateChange == null)
                    UserData.OnStateChange = new UnityEngine.Events.UnityEvent();
                UserData.OnStateChange.AddListener(handleStateChange);
                if (UserData.OnNameChanged == null)
                    UserData.OnNameChanged = new UnityEngine.Events.UnityEvent();
                UserData.OnNameChanged.AddListener(handleNameChanged);
                if (UserData.OnAvatarLoaded == null)
                    UserData.OnAvatarLoaded = new UnityEngine.Events.UnityEvent();
                UserData.OnAvatarLoaded.AddListener(handleAvatarChange);
            }
        }

        private void handleNameChanged()
        {
            PersonaName.text = UserData.DisplayName;
        }

        private void handleAvatarChange()
        {
            Avatar.texture = UserData.Avatar;
        }

        private void handleStateChange()
        {
            switch(UserData.State)
            {
                case Steamworks.EPersonaState.k_EPersonaStateAway:
                    if (UserData.InGame)
                    {
                        if (UserData.GameInfo.m_gameID.AppID().m_AppId == SteamworksFoundationManager.Instance.Settings.ApplicationId.m_AppId)
                        {
                            StatusLabel.text = "Playing";
                            IconBorder.color = ThisGameColor.Value;
                        }
                        else
                        {
                            StatusLabel.text = "In-Game";
                            IconBorder.color = InGameColor.Value;
                        }
                    }
                    else
                    {
                        StatusLabel.text = "Away";
                        IconBorder.color = AwayColor.Value;
                    }
                    break;
                case Steamworks.EPersonaState.k_EPersonaStateBusy:
                    if (UserData.InGame)
                    {
                        if (UserData.GameInfo.m_gameID.AppID().m_AppId == SteamworksFoundationManager.Instance.Settings.ApplicationId.m_AppId)
                        {
                            StatusLabel.text = "Playing";
                            IconBorder.color = ThisGameColor.Value;
                        }
                        else
                        {
                            StatusLabel.text = "In-Game";
                            IconBorder.color = InGameColor.Value;
                        }
                    }
                    else
                    {
                        StatusLabel.text = "Buisy";
                        IconBorder.color = BuisyColor.Value;
                    }
                    break;
                case Steamworks.EPersonaState.k_EPersonaStateLookingToPlay:
                    StatusLabel.text = "Looking to Play";
                    IconBorder.color = WantPlayColor.Value;
                    break;
                case Steamworks.EPersonaState.k_EPersonaStateLookingToTrade:
                    StatusLabel.text = "Looking to Trade";
                    IconBorder.color = WantTradeColor.Value;
                    break;
                case Steamworks.EPersonaState.k_EPersonaStateOffline:
                    StatusLabel.text = "Offline";
                    IconBorder.color = OfflineColor.Value;
                    break;
                case Steamworks.EPersonaState.k_EPersonaStateOnline:
                    if (UserData.InGame)
                    {
                        if (UserData.GameInfo.m_gameID.AppID().m_AppId == SteamworksFoundationManager.Instance.Settings.ApplicationId.m_AppId)
                        {
                            StatusLabel.text = "Playing";
                            IconBorder.color = ThisGameColor.Value;
                        }
                        else
                        {
                            StatusLabel.text = "In-Game";
                            IconBorder.color = InGameColor.Value;
                        }
                    }
                    else
                    {
                        StatusLabel.text = "Online";
                        IconBorder.color = OnlineColor.Value;
                    }
                    break;
                case Steamworks.EPersonaState.k_EPersonaStateSnooze:
                    if (UserData.InGame)
                    {
                        if (UserData.GameInfo.m_gameID.AppID().m_AppId == SteamworksFoundationManager.Instance.Settings.ApplicationId.m_AppId)
                        {
                            StatusLabel.text = "Playing";
                            IconBorder.color = ThisGameColor.Value;
                        }
                        else
                        {
                            StatusLabel.text = "In-Game";
                            IconBorder.color = InGameColor.Value;
                        }
                    }
                    else
                    {
                        StatusLabel.text = "Snooze";
                        IconBorder.color = SnoozeColor.Value;
                    }
                    break;
            }
            if (ColorTheStatusLabel)
                StatusLabel.color = IconBorder.color;
            if (ColorThePersonaName)
                PersonaName.color = IconBorder.color;
        }

        private void OnDestroy()
        {
            if (UserData != null)
                UserData.OnAvatarChanged.RemoveListener(handleAvatarChange);
        }
    }
}
#endif