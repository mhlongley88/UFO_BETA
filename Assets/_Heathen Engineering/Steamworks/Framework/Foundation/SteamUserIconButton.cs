#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using UnityEngine.EventSystems;

namespace HeathenEngineering.SteamTools
{
    public class SteamUserIconButton : SteamUserFullIcon, IPointerClickHandler
    {
        public UnityPersonaEvent OnLeftClick;
        public UnityPersonaEvent OnMiddleClick;
        public UnityPersonaEvent OnRightClick;
        public UnityPersonaEvent OnLeftDoubleClick;
        public UnityPersonaEvent OnMiddleDoubleClick;
        public UnityPersonaEvent OnRightDoubleClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                if (eventData.clickCount > 1)
                    OnLeftDoubleClick.Invoke(UserData);
                else
                    OnLeftClick.Invoke(UserData);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (eventData.clickCount > 1)
                    OnRightDoubleClick.Invoke(UserData);
                else
                    OnRightClick.Invoke(UserData);
            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
                if (eventData.clickCount > 1)
                    OnMiddleDoubleClick.Invoke(UserData);
                else
                    OnMiddleClick.Invoke(UserData);
            }
        }
    }
}
#endif