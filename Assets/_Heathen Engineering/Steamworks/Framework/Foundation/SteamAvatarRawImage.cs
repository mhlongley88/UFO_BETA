#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using UnityEngine;
using HeathenEngineering.Tools;

namespace HeathenEngineering.SteamTools
{
    [RequireComponent(typeof(UnityEngine.UI.RawImage))]
    public class SteamAvatarRawImage : HeathenUIBehaviour
    {
        private UnityEngine.UI.RawImage image;
        public SteamUserData userData;

        private void Awake()
        {
            image = GetComponent<UnityEngine.UI.RawImage>();

            LinkSteamUser(userData);
        }

        public void LinkSteamUser(SteamUserData newUserData)
        {
            if (userData != null)
                userData.OnAvatarChanged.RemoveListener(handleAvatarChange);

            userData = newUserData;

            if (userData != null)
            {
                if(image == null)
                    image = GetComponent<UnityEngine.UI.RawImage>();

                image.texture = userData.Avatar;
                userData.OnAvatarChanged.AddListener(handleAvatarChange);
            }
        }

        private void handleAvatarChange()
        {
            image.texture = userData.Avatar;
        }

        private void OnDestroy()
        {
            if (userData != null)
                userData.OnAvatarChanged.RemoveListener(handleAvatarChange);
        }
    }
}
#endif