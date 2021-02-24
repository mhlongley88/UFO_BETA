using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
public class TouchMenuUI : MonoBehaviour
{
    public CharacterLoadOutUI characterLoadOut;
    public GameObject selectedCharacter;
    public Transform MainHubCharacterContainer, OpponentCharacterSelectContainerOnline, myCharacterSelectContainer;
    public GameObject StartMatchButton, CancelMatchButton;
    public TextMeshProUGUI MatchmakingTimer;
    public GetPrefsUI GemsUI, CoinsUI;
    public GameObject TutorialPrompt;

    public Sprite[] UfoAvatar;
    public ChangeNameUI changeNameUI;

    public Image xpFG;
    public TextMeshProUGUI LevelNumber;

    public int matchmakingCounter;
    public bool matchmakingStarted;

    public int levelUpPrice = 1000;
    // Start is called before the first frame update
    void Start()
    {
        if (!UserPrefs.instance.GetBool("playedBefore"))
        {
            UserPrefs.instance.SetBool("playedBefore", true);
            StartCoroutine(ActivateTutorialPrompt());
        }
    }

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

    public void SpawnSelectedCharacter(GameObject characterPrefab, int characterId)
    {
        Destroy(selectedCharacter);
        selectedCharacter = Instantiate(characterPrefab, MainHubCharacterContainer);
        selectedCharacter.transform.localPosition = Vector3.zero;
        selectedCharacter.transform.localScale = Vector3.one;
        GameManager.Instance.LocalPlayerId = characterId;
        DisplayUFOPrefsMainHub(GameManager.Instance.GetUfoAttribute(characterId));
        
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

    void CharacterLevelUp(UFOAttributes attr)
    {
        attr.ufoXP = attr.ufoXP - 1;
        attr.ufoLevel += 1;
        attr.Accuracy = (attr.Accuracy + 0.1f) >= 1 ? 1 : (attr.Accuracy + 0.1f);
        attr.Damage = (attr.Damage + 0.1f) >= 1 ? 1 : (attr.Damage + 0.1f);
        attr.RateOfFire = (attr.RateOfFire + 0.1f) >= 1 ? 1 : (attr.RateOfFire + 0.1f);
        UserPrefs.instance.Save();
        DisplayUFOPrefsMainHub(attr);
    }

    public void CharacterLevelUpViaCoins()
    {
        if (GameManager.Instance.GetCoins() < levelUpPrice || MaxLevelReached()) return;
        UFOAttributes attr = GameManager.Instance.GetUfoAttribute(GameManager.Instance.LocalPlayerId);
        attr.ufoXP += 1f;// UnityEngine.Random.Range((float)(lowerBound/ upperBound), 1);
        
        GameManager.Instance.SetCoins((GameManager.Instance.GetCoins() - levelUpPrice));

        GameManager.Instance.SetUfoAttribute(GameManager.Instance.LocalPlayerId, attr);
        CharacterLevelUp(attr);
        //SpawnSelectedCharacter(GameManager.Instance.Characters[GameManager.Instance.LocalPlayerId].characterModel, GameManager.Instance.LocalPlayerId);
        characterLoadOut.DisplayCharacterInfo_LoadOutScreen();
    }

    bool MaxLevelReached()
    {
        UFOAttributes attr = GameManager.Instance.GetUfoAttribute(GameManager.Instance.LocalPlayerId);
        if (attr.Accuracy < 1 || attr.Damage < 1 || attr.RateOfFire < 1)
            return false;
        else
            return true;
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
