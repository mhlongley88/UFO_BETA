using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class TouchMenuUI : MonoBehaviour
{
    public GameObject selectedCharacter;
    public Transform MainHubCharacterContainer, OpponentCharacterSelectContainerOnline, myCharacterSelectContainer;
    public GameObject StartMatchButton, CancelMatchButton;
    public TextMeshProUGUI MatchmakingTimer;

    public Sprite[] UfoAvatar;

    public int matchmakingCounter;
    public bool matchmakingStarted;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpawnSelectedCharacter(GameObject characterPrefab)
    {
        Destroy(selectedCharacter);
        selectedCharacter = Instantiate(characterPrefab, MainHubCharacterContainer);
        selectedCharacter.transform.localPosition = Vector3.zero;
        selectedCharacter.transform.localScale = Vector3.one;
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
