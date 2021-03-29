using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
public class TouchMenuUI : MonoBehaviour
{
    public LevelSelectionUI levelSelectUI;
    public CharacterLoadOutUI characterLoadOut, characterLoadOutStore;
    public RewardsUI RewardScreen;
    public GameObject selectedCharacter;
    public Transform MainHubCharacterContainer, OpponentCharacterSelectContainerOnline, myCharacterSelectContainer;
    public GameObject MainHub, StartMatchButton, CancelMatchButton;
    public TextMeshProUGUI MatchmakingTimer;
    public GetPrefsUI GemsUI, CoinsUI;
    public GameObject TutorialPrompt;
    public GameObject StoreMenu;

    public FreeRewardBox[] rewardBoxes;

    public Sprite[] UfoAvatar;
    public ChangeNameUI changeNameUI;

    public Image xpFG;
    public TextMeshProUGUI LevelNumber;

    public int matchmakingCounter;
    public bool matchmakingStarted;

    
    // Start is called before the first frame update
    void Start()
    {
        if (!UserPrefs.instance.GetBool("playedBefore"))
        {
            UserPrefs.instance.SetBool("playedBefore", true);

            UserPrefs.instance.SetBool("FreeBox1", true);
            UserPrefs.instance.SetBool("FreeBox2", true);
            UserPrefs.instance.SetBool("FreeBox3", true);

            StartCoroutine(ActivateTutorialPrompt());
        }
        GameManager.Instance.enterTutorial = false;
        GameManager.Instance.ResetTryProps();
    }

    //void Init()
    //{
    //    GameManager.Instance.RemoveAllPlayersFromGame();
    //    SelectingCharacter_WithoutStick();
    //    MainMenuUIManager.Instance.touchMenuUI.SpawnSelectedCharacter(GameManager.Instance.Characters[GameManager.Instance.selectedCharacterIndex].characterModel, GameManager.Instance.Characters[GameManager.Instance.selectedCharacterIndex].characterId);

    //}

    IEnumerator ActivateTutorialPrompt()
    {
        yield return new WaitForSeconds(3f);
        ToggleMainHubCharacter(false);
        TutorialPrompt.SetActive(true);
    }

    public void ToggleMainHubCharacter(bool en)
    {
        MainHubCharacterContainer.gameObject.SetActive(en);
    }

    public void SpawnCharacterMainHub()
    {
        SpawnSelectedCharacter(GameManager.Instance.Characters[GameManager.Instance.selectedCharacterIndex].characterModel, GameManager.Instance.Characters[GameManager.Instance.selectedCharacterIndex].characterId);
    }

    public void SpawnSelectedCharacter(GameObject characterPrefab, int characterId)
    {
        Destroy(selectedCharacter);
        selectedCharacter = Instantiate(characterPrefab, MainHubCharacterContainer);
        selectedCharacter.transform.localPosition = Vector3.zero;
        selectedCharacter.transform.localScale = Vector3.one;
        GameManager.Instance.selectedCharacterIndex = characterId;

        selectedCharacter.GetComponent<CharacterLevelSelectInfo>().currentSkinId = GameManager.Instance.GetSelectedUfoAttribute().currSkinId;
        selectedCharacter.GetComponent<CharacterLevelSelectInfo>().ActivateSkin();

        DisplayUFOPrefsMainHub(GameManager.Instance.GetUfoAttribute(characterId));
        
    }

    public void SpawnCharacterLoadOut_Store(int id, StoreItem item)
    {
        StoreMenu.SetActive(false);
        characterLoadOutStore.gameObject.SetActive(true);

        characterLoadOutStore.EmptyCart();
        UFOAttributes attr = GameManager.Instance.GetUfoAttribute(item.itemId);
        Debug.Log(item.itemId + "--" + attr.isUnlocked);

        if (!attr.isUnlocked)
        {
            characterLoadOutStore.LevelLockedObj.SetActive(true);
            characterLoadOutStore.AddUFOToCart(item.itemId);

        }
        else
        {
            MainMenuUIManager.Instance.touchMenuUI.characterLoadOutStore.LevelLockedObj.SetActive(false);
        }

        characterLoadOutStore.DisplayCharacterInfo_LoadOut_StoreScreen(id);
    }
    

    public void SpawnCharacterLoadOut_Store(int characterId, int skinId, StoreItem item)
    {
        
        StoreMenu.SetActive(false);
        characterLoadOutStore.gameObject.SetActive(true);

        characterLoadOutStore.EmptyCart();
        UFOAttributes attr = GameManager.Instance.GetUfoAttribute(item.skin_characterId);
        if (!attr.isUnlocked)
        {
            characterLoadOutStore.LevelLockedObj.SetActive(true);
            characterLoadOutStore.AddUFOToCart(item.skin_characterId);
        }
        else
        {
            MainMenuUIManager.Instance.touchMenuUI.characterLoadOutStore.LevelLockedObj.SetActive(false);
        }

        if (!attr.Skins[skinId].isUnlocked)
        {
            
            characterLoadOutStore.AddToCart(characterLoadOutStore.ConvertToItemHolder(item));
        }

        characterLoadOutStore.DisplayCharacterInfo_LoadOut_StoreScreen(characterId);
        characterLoadOutStore.EnableSkin(skinId);
    }

    public void DisplayUFOPrefsMainHub(UFOAttributes attr)
    {
       
        xpFG.fillAmount = attr.ufoXP;
        LevelNumber.text = (attr.ufoLevel + 1).ToString();

        if(attr.ufoXP >= 1)
        {
            CharacterLevelUp(attr);
        }
    }
    
    public void DisplayGemsCoinsMainHub()
    {
        GemsUI.UpdateUI();
        CoinsUI.UpdateUI();
    }

    public void DisplayGemsMainHub()
    {
        GemsUI.UpdateUI();
    }

    public void DisplayCoinsMainHub()
    {
        CoinsUI.UpdateUI();
    }

    public void CharacterLevelUp(UFOAttributes attr)
    {
        attr.ufoXP = attr.ufoXP - 1;
        attr.ufoLevel += 1;
        attr.Accuracy = (attr.Accuracy + 0.1f) >= 1 ? 1 : (attr.Accuracy + 0.1f);
        attr.Damage = (attr.Damage + 0.1f) >= 1 ? 1 : (attr.Damage + 0.1f);
        attr.RateOfFire = (attr.RateOfFire + 0.1f) >= 1 ? 1 : (attr.RateOfFire + 0.1f);
        UserPrefs.instance.Save();
        DisplayUFOPrefsMainHub(attr);
    }

    

    public bool localMatchingCancelled = false;
    public void CancelMatchmaking()
    {
        if(PhotonNetwork.CurrentRoom != null)
        {
            
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            
            EnableMatchmakingButton();
        }
        MainMenuUIManager.Instance.SwitchToSplashScreen_WithoutStick();
        RefreshCharacterSelectContainers();
        matchmakingCounter = 0;
        CancelMatchButton.SetActive(false);
    }

    public void EnableMatchmakingButton()
    {
        matchmakingStarted = false;
        localMatchingCancelled = true;
        StartMatchButton.SetActive(true);
        CancelMatchButton.SetActive(false);
        
    }

    public void EnableCancelMatchmakingButton()
    {
        StartMatchButton.SetActive(false);
        CancelMatchButton.SetActive(true);
        StartMatchmakingCounter();
    }

    public void StartMatchmakingCounter()
    {
        matchmakingStarted = true;
        StartCoroutine(IncrementMatchingCounter());
    }

    public void OpenCharacterLoadOut_OnClick()
    {
        MainHub.SetActive(false);
        characterLoadOut.gameObject.SetActive(true);
        characterLoadOut.DisplayCharacterInfo_LoadOutScreen(GameManager.Instance.selectedCharacterIndex);
    }

    public void OpenRewardsScreen_OnClick(int id)
    {
        StoreMenu.SetActive(false);
        RewardScreen.gameObject.SetActive(true);
        RewardScreen.SpawnRewardScreen();
        ConsumeRewardBox(id);
    }

    public void OpenStore_OnClick()
    {
        StoreMenu.SetActive(true);
        CheckRewardBoxesConsumed();
    }

    void ConsumeRewardBox(int id)
    {
        UserPrefs.instance.SetBool(rewardBoxes[id].boxId, false);
    }


    public void CheckRewardBoxesConsumed()
    {
        foreach (FreeRewardBox box in rewardBoxes)
        {
            if (UserPrefs.instance.GetBool(box.boxId))
            {
                box.gameObject.SetActive(true);
            }
            else
            {
                box.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator IncrementMatchingCounter()
    {
        matchmakingCounter++;
        yield return new WaitForSeconds(1f);
        if (matchmakingStarted)
        {
            UpdateMatchmakingCounterUI(matchmakingCounter);
            StartCoroutine(IncrementMatchingCounter());
        }
    }

    void UpdateMatchmakingCounterUI(int timer)
    {
        MatchmakingTimer.text = timer.ToString();
    }

    void RefreshCharacterSelectContainers()
    {
        int count = OpponentCharacterSelectContainerOnline.childCount;
        for(int i = 0; i < count; i++)
        {
            Destroy(OpponentCharacterSelectContainerOnline.GetChild(i).gameObject);
        }
        count = myCharacterSelectContainer.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(myCharacterSelectContainer.GetChild(i).gameObject);
        }
    }
}
