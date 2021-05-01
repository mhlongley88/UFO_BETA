using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class RewardStats : MonoBehaviour
{
    public TextMeshProUGUI GemsText, CoinsText, XPText;
    public bool isBrawlBox;

    public UnityEvent GoBack;

    public void Awake()
    {
        
    }
    public void GoToPrevScreen()
    {
        if (gameObject.activeSelf)
        {
            MainMenuUIManager.Instance.touchMenuUI.RewardScreen.UpdateCoins(0);
            MainMenuUIManager.Instance.touchMenuUI.RewardScreen.UpdateGems(0);
            GoBack.Invoke();
        }
            
    }
    public void GoBackToStore()
    {
        MainMenuUIManager.Instance.touchMenuUI.RewardScreen.GoBackToStore();
    }
    public void GoBackToEvents()
    {
        MainMenuUIManager.Instance.touchMenuUI.RewardScreen.GoBackToEventsScreen();
    }

    public void UpdateGems(float delay)
    {
        MainMenuUIManager.Instance.touchMenuUI.RewardScreen.UpdateGems(delay);
    }

    public void UpdateCoins(float delay)
    {
        MainMenuUIManager.Instance.touchMenuUI.RewardScreen.UpdateCoins(delay);
    }

}
