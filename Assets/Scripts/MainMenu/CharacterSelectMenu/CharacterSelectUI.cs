using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using static InputManager;

public class CharacterSelectUI : MonoBehaviour
{
    public enum CharacterSelectState
    {
        WaitingForPlayer,
        SelectingCharacter,
        ReadyToStart
    }

    public Player player;
    public GameObject charSelect;


    public GameObject pressStart;


    public GameObject readyObject;



    public AudioClip playerReadySFX;
    public AudioSource myAudioSource;



    public string playerName;
    public TextMeshPro playerNameText;

    public Transform characterModelContainer;
    private GameObject currentCharacterModel;

    private int selectedCharacterIndex = 0;

    private CharacterSelectState selectState = CharacterSelectState.WaitingForPlayer;

    private bool canCycle = true;
    private bool canCycleRight = true;

    public CharacterSelectState GetCurSelectState()
    {
        return selectState;
    }
    // Start is called before the first frame update
    void Start()
    {
        pressStart.SetActive(true);

        playerNameText.text = playerName;
        currentCharacterModel = Instantiate(GameManager.Instance.Characters[selectedCharacterIndex].characterModel, Vector3.zero, Quaternion.identity, characterModelContainer);


    }

    // Update is called once per frame

    private void UpdateSelection()
    {
        if (selectedCharacterIndex < 0) selectedCharacterIndex = GameManager.Instance.Characters.Length - 1;
        else if (selectedCharacterIndex >= GameManager.Instance.Characters.Length) selectedCharacterIndex = 0;

        //selectedCharacterIndex = Mathf.Abs(selectedCharacterIndex) % GameManager.Instance.Characters.Length;

        Destroy(currentCharacterModel);
        currentCharacterModel = Instantiate(GameManager.Instance.Characters[selectedCharacterIndex].characterModel, Vector3.zero, Quaternion.identity, characterModelContainer);
        currentCharacterModel.transform.localPosition = Vector3.zero;
    }

    private void ChangeSelection()
    {
        float horizontalInput = InputManager.Instance.GetAxis(AxisEnum.LeftStickHorizontal, player);
        if (canCycle)
        {
            if (horizontalInput < -0.8f)
            {
                selectedCharacterIndex--;
                canCycle = false;
                UpdateSelection();
            }
            else if (horizontalInput > 0.8f)
            {
                selectedCharacterIndex++;
                canCycle = false;
                UpdateSelection();
            }
        }
        else
        {
            if (horizontalInput > -0.8f && horizontalInput < 0.8f)
            {
                canCycle = true;
            }
        }
    }

    private void PlayerEnterGame()
    {
        pressStart.SetActive(false);
        charSelect.SetActive(true);
        GameManager.Instance.AddPlayerToGame(player);
        selectState = CharacterSelectState.SelectingCharacter;
    }

    private void Update()
    {
        switch (selectState)
        {
            case CharacterSelectState.WaitingForPlayer:

                if (InputManager.Instance.GetButtonDown(ButtonEnum.Submit, player))
                {
                    PlayerEnterGame();
                }
                break;
            case CharacterSelectState.SelectingCharacter:
                ChangeSelection();

                if (InputManager.Instance.GetButtonDown(ButtonEnum.Back, player))
                {
                    pressStart.SetActive(true);
                    charSelect.SetActive(false);
                    selectState = CharacterSelectState.WaitingForPlayer;
                    GameManager.Instance.RemovePlayerFromGame(player);
                }

                if (InputManager.Instance.GetButtonDown(ButtonEnum.Submit, player))
                {
                    readyObject.SetActive(false);
                    myAudioSource.PlayOneShot(playerReadySFX);
                    GameManager.Instance.SetPlayerCharacterChoice(player, selectedCharacterIndex);
                    selectState = CharacterSelectState.ReadyToStart;
                }
                break;
            case CharacterSelectState.ReadyToStart:
                if (InputManager.Instance.GetButtonDown(ButtonEnum.Back, player))
                {
                    readyObject.SetActive(true);
                    selectState = CharacterSelectState.SelectingCharacter;
                }
                break;
        }
    }

    public void ReturnFromLevelSelect()
    {
        if(GameManager.Instance.IsPlayerInGame(player))
        {
            readyObject.SetActive(true);
            charSelect.SetActive(true);
            pressStart.SetActive(false);
            selectState = CharacterSelectState.SelectingCharacter;
            UpdateSelection();
        }
    }
}
