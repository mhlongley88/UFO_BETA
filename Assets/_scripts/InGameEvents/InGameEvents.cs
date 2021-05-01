using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InGameEvents : MonoBehaviour
{
    public int maxMatches;

    public MilestoneReward[] NormalPassRewards, BrawlPassRewards;
    public Scrollbar progressSlider;
    public GameObject EventsScreen;
    public Image BP_Activated_Img, NP_Activated_Img;
    public GameObject BuyBrawlPassScreen;
    public EventItem FinalReward;
    public GameObject FinalRewardConfirmationPanel;
    public ShowLevelTitle RewardLevelSettings;

    public int currentProgressVal;
    // Start is called before the first frame update
    void Start()
    {
        //currentProgressVal = GameManager.Instance.GetMatchesPlayed();
    }

    public void UpdateProgressBar()
    {
        //currentProgressVal = GameManager.Instance.GetMatchesPlayed();
        progressSlider.value = ((float)currentProgressVal / (float)maxMatches);
        RefreshItems();
    }

    public void BuyProgressViaCoins(int coinsAmt)
    {
        int coins = GameManager.Instance.GetCoins();
        if(coins >= coinsAmt)
        {
            GameManager.Instance.AddCoins(-coinsAmt);
            GameManager.Instance.AddMatchesPlayed(1);
            currentProgressVal = GameManager.Instance.GetMatchesPlayed();
            UpdateProgressBar();
        }
    }
    public void BuyBrawlPass(int gemsAmt)
    {
        int coins = GameManager.Instance.GetGems();
        if (coins >= gemsAmt)
        {
            GameManager.Instance.AddGems(-gemsAmt);
            GameManager.Instance.SetBrawlPassPurchased(true);
            BuyBrawlPassScreen.SetActive(false);
            SwitchToBrawlPass();
        }
    }
    public void AddGems(EventItem item)
    {
        if (!UserPrefs.instance.GetBool(item.redeemedString))
        {
            UserPrefs.instance.SetBool(item.redeemedString, true);
            UserPrefs.instance.Save();
            GameManager.Instance.AddGems(item.GemsAmount, true);

            if (item.isBrawlPassItem)
            {
                SwitchToBrawlPass();
            }
            else
            {
                SwitchToNormalPass();
            }
        }
        
    }

    public void UnlockCharacter(EventItem item)
    {
        if (!UserPrefs.instance.GetBool(item.redeemedString))
        {
            UserPrefs.instance.SetBool(item.redeemedString, true);
            UserPrefs.instance.Save();
            UFOAttributes attr = GameManager.Instance.GetUfoAttribute(item.CharacterId);
            if (!attr.isUnlocked)
            {
                attr.isUnlocked = true;
                GameManager.Instance.SetUfoAttribute(item.CharacterId, attr);

            }

            if (item.isBrawlPassItem)
            {
                SwitchToBrawlPass();
            }
            else
            {
                SwitchToNormalPass();
            }
        }
            
    }

    public void UnlockSkin(EventItem item)
    {

    }

    public void UnlockMap(EventItem item)
    {
        if (!UserPrefs.instance.GetBool(item.redeemedString))
        {
            if (item.isFinalReward)
            {
                FinalRewardConfirmationPanel.SetActive(true);
            }
            else
            {
                UserPrefs.instance.SetBool(item.redeemedString, true);
                UserPrefs.instance.Save();
                GameManager.Instance.AddGems(item.GemsAmount, true);

                if (item.isBrawlPassItem)
                {
                    SwitchToBrawlPass();
                }
                else
                {
                    SwitchToNormalPass();
                }
            }
            
        }
    }

    public void UnlockRewardBox(EventItem item)
    {
        if (!UserPrefs.instance.GetBool(item.redeemedString))
        {
            UserPrefs.instance.SetBool(item.redeemedString, true);
            UserPrefs.instance.Save();
            MainMenuUIManager.Instance.touchMenuUI.OpenRewardsScreen_Events_OnClick(item.redeemedString);

            if (item.isBrawlPassItem)
            {
                SwitchToBrawlPass();
            }
            else
            {
                SwitchToNormalPass();
            }
        }
        
    }

    public void LoadFinalRewardLevel()
    {
        MainMenuUIManager.Instance.OfflineButtonListener(0);
        MainMenuUIManager.Instance.SwitchToCharacterSelect_WithoutStick();
        //offlineCharacterSelect.SetCharacter();
        RewardLevelSettings.SetStageSettings(); //
        RewardLevelSettings.EnforceThisStageSettings();
        GameManager.Instance.localPlayer = Player.One;
        MainMenuUIManager.Instance.HoldCharacterChoiceTemporarily(Player.One, GameManager.Instance.selectedCharacterIndex);
        RewardLevelSettings.gameObject.GetComponent<LevelSetBotPreset>().SetBotSettings();

        GameManager.Instance.isEventMatch_FinalReward = true;

        MainMenuUIManager.Instance.LoadScene_LoadingRoon();
    }

    public void EnableEventsScreen()
    {
        EventsScreen.SetActive(true);
        currentProgressVal = GameManager.Instance.GetMatchesPlayed();
        UpdateProgressBar();
        RefreshItems();
    }
    void RefreshItems()
    {
        if (GameManager.Instance.GetBrawlPassPurchased())
        {
            SwitchToBrawlPass();
        }
        else
        {
            SwitchToNormalPass();
        }
    }
    public void DisableEventsScreen()
    {
        EventsScreen.SetActive(false);
    }

    public void DisableEventsScreen(GameObject screen)
    {
        EventsScreen.SetActive(false);
        screen.SetActive(true);
    }

    public void SwitchToNormalPass()
    {
        currentProgressVal = GameManager.Instance.GetMatchesPlayed();
        BP_Activated_Img.enabled = false;
        NP_Activated_Img.enabled = true;
        LockBrawlPassItems();
        UnlockNormalPassItems();
    }

    public void SwitchToBrawlPass()
    {
        currentProgressVal = GameManager.Instance.GetMatchesPlayed();
        if (GameManager.Instance.GetBrawlPassPurchased())
        {
            BP_Activated_Img.enabled = true;
            NP_Activated_Img.enabled = false;
            LockNormalPassItems();
            UnlockBrawlPassItems();
        }
        else
        {
            BuyBrawlPassScreen.SetActive(true);
        }
    }


    void UnlockBrawlPassItems()
    {
        //currentProgressVal = GameManager.Instance.GetMatchesPlayed();
        foreach(MilestoneReward reward in BrawlPassRewards)
        {
            if(reward.unlockThresh <= currentProgressVal)
            {
                reward.relevantItem.MakeInteractable();
                reward.relevantItem.TogglePickUpAnim(!UserPrefs.instance.GetBool(reward.relevantItem.redeemedString));
            }
            
            
        }

        if(currentProgressVal >= maxMatches && !UserPrefs.instance.GetBool(FinalReward.redeemedString))
        {
            FinalReward.MakeInteractable();
            FinalReward.TogglePickUpAnim(true);
        }
        else
        {
            FinalReward.LockIt();
            FinalReward.TogglePickUpAnim(false);
        }

    }
    void LockBrawlPassItems()
    {
        //currentProgressVal = GameManager.Instance.GetMatchesPlayed();
        foreach (MilestoneReward reward in BrawlPassRewards)
        {
            //if (reward.unlockThresh <= currentProgressVal)
            {
                reward.relevantItem.LockIt();
                reward.relevantItem.TogglePickUpAnim(false);
            }


        }
    }
    void UnlockNormalPassItems()
    {
        
        foreach (MilestoneReward reward in NormalPassRewards)
        {
            if (reward.unlockThresh <= currentProgressVal)
            {
                reward.relevantItem.MakeInteractable();
                reward.relevantItem.TogglePickUpAnim(!UserPrefs.instance.GetBool(reward.relevantItem.redeemedString));
            }
        }

        if (currentProgressVal >= maxMatches && !UserPrefs.instance.GetBool(FinalReward.redeemedString))
        {
            FinalReward.MakeInteractable();
            FinalReward.TogglePickUpAnim(true);
        }
        else
        {
            FinalReward.LockIt();
            FinalReward.TogglePickUpAnim(false);
        }
    }
    void LockNormalPassItems()
    {

        foreach (MilestoneReward reward in NormalPassRewards)
        {
            //if (reward.unlockThresh <= currentProgressVal)
            {
                reward.relevantItem.LockIt();
                reward.relevantItem.TogglePickUpAnim(false);
            }
        }
    }
}

[System.Serializable]
public class MilestoneReward
{
    
    public int unlockThresh;
    public EventItem relevantItem;
}