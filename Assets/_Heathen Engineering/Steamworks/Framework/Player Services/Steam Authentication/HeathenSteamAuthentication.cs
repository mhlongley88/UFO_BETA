#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using System;
using System.Text;
using UnityEngine;

namespace HeathenEngineering.SteamTools
{
    public class HeathenSteamAuthentication : MonoBehaviour
    {
        public HAuthTicket m_HAuthTicket;
        private byte[] SessionTicket;

        public Callback<GetAuthSessionTicketResponse_t> m_GetAuthSessionTicketResponce;
        public Callback<ValidateAuthTicketResponse_t> m_ValidateAuthSessionTicketResponce;

        public bool IsAuthTicketValid
        {
            get
            {
                if (m_HAuthTicket == default(HAuthTicket) || m_HAuthTicket == HAuthTicket.Invalid)
                    return false;
                else
                    return true;
            }
        }

        public string EncodedAuthTicket
        {
            get
            {
                if (!IsAuthTicketValid)
                    return "";
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in SessionTicket)
                        sb.AppendFormat("{0:X2}", b);

                    return sb.ToString();
                }
            }
        }

        public void GetAuthSessionTicket()
        {
            uint m_pcbTicket;
            SessionTicket = new byte[1024];
            m_HAuthTicket = SteamUser.GetAuthSessionTicket(SessionTicket, 1024, out m_pcbTicket);
            Array.Resize(ref SessionTicket, (int)m_pcbTicket);
        }

        public void CancelAuthTicket()
        {
            SteamUser.CancelAuthTicket(m_HAuthTicket);
        }

        public void BeginAuthSession(byte[] authTicket, CSteamID user)
        {
            SteamUser.BeginAuthSession(authTicket, authTicket.Length, user);
        }

        public void EndAuthSession(CSteamID user)
        {
            SteamUser.EndAuthSession(user);
        }

        private void HandleGetAuthSessionTicketResponce(GetAuthSessionTicketResponse_t pCallback)
        {
            m_HAuthTicket = pCallback.m_hAuthTicket;

            if (m_HAuthTicket != default(HAuthTicket) && m_HAuthTicket != HAuthTicket.Invalid)
                Debug.Log("Session authorization recieved");
        }
    }
}
#endif