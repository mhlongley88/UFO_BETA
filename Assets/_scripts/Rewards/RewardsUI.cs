using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardsUI : MonoBehaviour
{
    public TextMeshProUGUI GemsText, CoinsText, XPText;
    public GameObject RewardPrefab, RewardPrefabBrawl;
    public Transform RewardContainer;
    //public FreeRewardBox[] rewardBoxes;
    public GameObject currentRewardScreen;
    public bool isBrawlReward;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnRewardScreen(TouchMenuUI.MenuScreens prevScreen)
    {
        if (isBrawlReward)
        {
            SpawnBrawlReward(prevScreen);
        }else
        {
            SpawnNormalReward(prevScreen);
        }
        AssignRewards();
    }

    void SpawnNormalReward(TouchMenuUI.MenuScreens prevScreen)
    {
        Destroy(currentRewardScreen);
        
        currentRewardScreen = Instantiate(RewardPrefab, RewardContainer);
        SetPrevScreen(prevScreen, currentRewardScreen.GetComponent<RewardStats>());
        currentRewardScreen.SetActive(true);
        currentRewardScreen.transform.SetSiblingIndex(currentRewardScreen.transform.parent.childCount - 3);
    }

    void SpawnBrawlReward(TouchMenuUI.MenuScreens prevScreen)
    {
        Destroy(currentRewardScreen);
        
        currentRewardScreen = Instantiate(RewardPrefabBrawl, RewardContainer);
        SetPrevScreen(prevScreen, currentRewardScreen.GetComponent<RewardStats>());
        currentRewardScreen.SetActive(true);
        currentRewardScreen.transform.SetSiblingIndex(currentRewardScreen.transform.parent.childCount - 3);
    }

    void SetPrevScreen(TouchMenuUI.MenuScreens prevScreen, RewardStats stats)
    {
        if(prevScreen == TouchMenuUI.MenuScreens.Store)
        {
            stats.GoBack.AddListener(stats.GoBackToStore);
        }else if(prevScreen == TouchMenuUI.MenuScreens.Events)
        {
            stats.GoBack.AddListener(stats.GoBackToEvents);
        }
    }

    public void SpawnBrawlRewardScreen(TouchMenuUI.MenuScreens prevScreen)
    {
        
    }

    public void AssignRewards()
    {
        RewardStats stats = currentRewardScreen.GetComponent<RewardStats>();
        if (stats)
        {
            GameManager.Instance.GetGems();
            int rewardedGems;
            int rewardedCoins;
            float rewardedXP;

            
            if (stats.isBrawlBox)
            {
                rewardedGems = Random.Range(15, 22);
                rewardedCoins = Random.Range(300, 401);
                rewardedXP = Random.Range(0.18f, 0.34f);
            }
            else
            {
                rewardedGems = Random.Range(10, 16);
                rewardedCoins = Random.Range(175, 301);
                rewardedXP = Random.Range(0.08f, 0.16f);
            }

            UFOAttributes attr = GameManager.Instance.GetSelectedUfoAttribute();

            attr.ufoXP += rewardedXP;

            GameManager.Instance.SetSelectedUfoAttribute(attr);
            GameManager.Instance.AddCoins(rewardedCoins, false);
            GameManager.Instance.AddGems(rewardedGems, false);

            if (stats)
            {
                stats.GemsText.text = rewardedGems.ToString();
                stats.CoinsText.text = rewardedCoins.ToString();
                stats.XPText.text = (Mathf.FloorToInt(rewardedXP * 100)).ToString();
            }
        }
        

    }

    public void UpdateGems(float delay)
    {
        StartCoroutine(UpdateGemsDelayed(delay));
    }

    IEnumerator UpdateGemsDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        MainMenuUIManager.Instance.touchMenuUI.DisplayGemsMainHub();
    }

    public void UpdateCoins(float delay)
    {
        StartCoroutine(UpdateCoinsDelayed(delay));
    }

    IEnumerator UpdateCoinsDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        MainMenuUIManager.Instance.touchMenuUI.DisplayCoinsMainHub();
    }

    public void UpdateGemsCoins(float delay)
    {
        MainMenuUIManager.Instance.touchMenuUI.DisplayGemsCoinsMainHub();
    }

    

    public void GoBackToStore()
    {
        MainMenuUIManager.Instance.touchMenuUI.OpenStore_OnClick();
        currentRewardScreen.SetActive(false);
    }

    public void GoBackToEventsScreen()
    {
        MainMenuUIManager.Instance.touchMenuUI.OpenEvents_OnClick();
        currentRewardScreen.SetActive(false);
    }
}
