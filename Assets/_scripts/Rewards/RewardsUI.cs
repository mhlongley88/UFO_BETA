using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardsUI : MonoBehaviour
{
    public TextMeshProUGUI GemsText, CoinsText, XPText;
    public GameObject RewardPrefab;
    public Transform RewardContainer;
    //public FreeRewardBox[] rewardBoxes;
    public GameObject currentRewardScreen;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnRewardScreen()
    {
        Destroy(currentRewardScreen);
        currentRewardScreen = Instantiate(RewardPrefab, RewardContainer);
        currentRewardScreen.SetActive(true);
        currentRewardScreen.transform.SetSiblingIndex(currentRewardScreen.transform.parent.childCount - 3);
    }


    public void AssignRewards()
    {
        GameManager.Instance.GetGems();
        int rewardedGems = Random.Range(15, 22);
        int rewardedCoins = Random.Range(300, 401);
        float rewardedXP = Random.Range(0.18f, 0.34f);

        UFOAttributes attr = GameManager.Instance.GetSelectedUfoAttribute();

        attr.ufoXP += rewardedXP;

        GameManager.Instance.SetSelectedUfoAttribute(attr);
        GameManager.Instance.AddCoins(rewardedCoins, false);
        GameManager.Instance.AddGems(rewardedGems, false);

        GemsText.text = rewardedGems.ToString();
        CoinsText.text = rewardedCoins.ToString();
        XPText.text = (Mathf.FloorToInt(rewardedXP * 100)).ToString();

    }

    public void UpdateGems(float delay)
    {
        Invoke("UpdateGems", delay);
    }

    public void UpdateGems()
    {
        
        MainMenuUIManager.Instance.touchMenuUI.DisplayGemsMainHub();
    }

    public void UpdateCoins(float delay)
    {
        Invoke("UpdateCoins", delay);
    }

    public void UpdateCoins()
    {
        MainMenuUIManager.Instance.touchMenuUI.DisplayCoinsMainHub();
    }

    public void UpdateGemsCoins()
    {
        MainMenuUIManager.Instance.touchMenuUI.DisplayGemsCoinsMainHub();
    }

    public void GoBackToStore()
    {
        MainMenuUIManager.Instance.touchMenuUI.OpenStore_OnClick();
        currentRewardScreen.SetActive(false);
    }
}
