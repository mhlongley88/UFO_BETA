using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Photon.Pun;
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


    private CharacterSelectState selectState = CharacterSelectState.WaitingForPlayer;

    private bool canCycle = true;
    private bool canCycleRight = true;

    public PhotonView pv;

    int _selectedCharacterIndex = 0;
    private int selectedCharacterIndex { get { return _selectedCharacterIndex; }
        set
        {
            if (value < 0 || value >= GameManager.Instance.Characters.Length) return;

            int wonBattles = UnlockSystem.instance.GetMatchesCompleted();
            int characterUnlockedAtWonBattles = GameManager.Instance.Characters[value].matchThreshold;

            if(wonBattles >= characterUnlockedAtWonBattles)
                _selectedCharacterIndex = value;
        }
    }

    public CharacterSelectState GetCurSelectState()
    {
        return selectState;
    }

    // Start is called before the first frame update
    void Start()
    {
        // pressStart.SetActive(true);
        characterModelContainer = this.transform.GetChild(0).GetChild(3);
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            playerNameText.text = playerName;
            currentCharacterModel = Instantiate(GameManager.Instance.Characters[selectedCharacterIndex].characterModel, Vector3.zero, Quaternion.identity, characterModelContainer);
        }
        else
        {
            pv = this.GetComponent<PhotonView>();
            if(pv.IsMine)
                pv.RPC("SyncMulSpawn", RpcTarget.All, selectedCharacterIndex);
            //SpawnMultiplayer();
        }


    }

    [PunRPC]
    void SyncMulSpawn(int index)
    {
        Debug.Log("Spawning");
        playerNameText.text = playerName;
        Debug.Log(selectedCharacterIndex);
        if(characterModelContainer == null)
            characterModelContainer = this.transform.GetChild(0).GetChild(3);
        for (int i=0; i< characterModelContainer.transform.childCount; i++)
        {
            Destroy(characterModelContainer.transform.GetChild(i).gameObject);
        }
        selectedCharacterIndex = Mathf.Abs(index) % GameManager.Instance.CharactersMul.Length;
        currentCharacterModel = Instantiate(GameManager.Instance.Characters[selectedCharacterIndex].characterModel, Vector3.zero, Quaternion.identity, characterModelContainer);
        currentCharacterModel.transform.SetParent(characterModelContainer);
        currentCharacterModel.transform.localPosition = Vector3.zero;
    }

    void SpawnMultiplayer()
    {
        Debug.Log("Multi");
       // if(currentCharacterModel != null)
            Destroy(currentCharacterModel);
       // selectedCharacterIndex = Mathf.Abs(selectedCharacterIndex) % GameManager.Instance.CharactersMul.Length;
        //currentCharacterModel = Photon.Pun.PhotonNetwork.Instantiate(GameManager.Instance.CharactersMul[selectedCharacterIndex].characterModel.name, Vector3.zero, Quaternion.identity);
        //pv.RPC("SyncMulSpawn", RpcTarget.All, selectedCharacterIndex);
       // currentCharacterModel.GetComponent<PlayerController>().enabled = true;
    }

    // Update is called once per frame

    public void UpdateSelectionMul()
    {
        pv.RPC("SyncMulSpawn", RpcTarget.All, selectedCharacterIndex);
        //SpawnMultiplayer();
    }

    private void UpdateSelection()
    {
        if (selectedCharacterIndex < 0) selectedCharacterIndex = GameManager.Instance.Characters.Length - 1;
        else if (selectedCharacterIndex >= GameManager.Instance.Characters.Length) selectedCharacterIndex = 0;

        //selectedCharacterIndex = Mathf.Abs(selectedCharacterIndex) % GameManager.Instance.Characters.Length;

        Destroy(currentCharacterModel);
        currentCharacterModel = Instantiate(GameManager.Instance.Characters[selectedCharacterIndex].characterModel, Vector3.zero, Quaternion.identity, characterModelContainer);
        currentCharacterModel.transform.localPosition = Vector3.zero;
    }

   // [PunRPC]
    void SyncSelection(bool forward)
    {
        if (forward)
        {
            selectedCharacterIndex++;
            canCycle = false;
            UpdateSelectionMul();
        }
        else
        {
            selectedCharacterIndex--;
            canCycle = false;
            UpdateSelectionMul();
        }
    }

    private void ChangeSelectionMul()
    {
        float horizontalInput = InputManager.Instance.GetAxis(AxisEnum.LeftStickHorizontal, player);
        if (canCycle)
        {
            if (horizontalInput < -0.8f)
            {
                SyncSelection(false);
                //pv.RPC("SyncSelection", RpcTarget.All, false);

            }
            else if (horizontalInput > 0.8f)
            {
                SyncSelection(true);
                //pv.RPC("SyncSelection", RpcTarget.All, true);
            }
        }
        else
        {
            if (horizontalInput > -0.8f && horizontalInput < 0.8f)
            {
                canCycle = true;
            }
        }

        float horizontalInputKB = InputManager.Instance.GetAxisKB(AxisEnum.LeftStickHorizontal, player);
        //Debug.Log(horizontalInputKB);
        if (canCycle)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //pv.RPC("SyncSelection", RpcTarget.All, false);
                SyncSelection(false);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                //pv.RPC("SyncSelection", RpcTarget.All, true);
                SyncSelection(true);
            }
        }
        else
        {
            if (horizontalInputKB > -0.8f && horizontalInputKB < 0.8f)
            {
                canCycle = true;
            }
        }

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

        if (LobbyConnectionHandler.instance.IsMultiplayerMode || player == Player.Four)
        {
            float horizontalInputKB = InputManager.Instance.GetAxisKB(AxisEnum.LeftStickHorizontal, player);
            Debug.Log(horizontalInputKB);
            if (canCycle)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    selectedCharacterIndex--;
                    canCycle = false;
                    UpdateSelection();
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    selectedCharacterIndex++;
                    canCycle = false;
                    UpdateSelection();
                }
            }
            else
            {
                if (horizontalInputKB > -0.8f && horizontalInputKB < 0.8f)
                {
                    canCycle = true;
                }
            }
        }

    }

    public void PlayerEnterGame()
    {
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            pressStart.SetActive(false);
            charSelect.SetActive(true);
            GameManager.Instance.AddPlayerToGame(player);
            selectState = CharacterSelectState.SelectingCharacter;
        }
        else
        {
            pv.RPC("PlayerEnterSync", RpcTarget.All);
            pv.RPC("SyncMulSpawn", RpcTarget.All, selectedCharacterIndex);
        }
    }
    public bool isSynced = false;
    [PunRPC]
    void PlayerEnterSync()
    {
        //pressStart.SetActive(false);
        if (!isSynced)
        {
            this.transform.SetParent(MainMenuUIManager.Instance.characterSelect.transform);
            charSelect.SetActive(true);
            GameManager.Instance.AddPlayerToGame(player);
            selectState = CharacterSelectState.SelectingCharacter;
            if (!GameManager.Instance.PlayerObjsMul.Contains(this))
            {
                GameManager.Instance.PlayerObjsMul.Add(this);
            }
            SyncTransforms();
            isSynced = true;
        }
    }

    void SyncTransforms()
    {

        switch (player)
        {
            case Player.One:
                this.transform.SetParent(MainMenuUIManager.Instance.characterSelect.transform);
                this.transform.localPosition = MainMenuUIManager.Instance.characterSelectMenus[0].gameObject.transform.localPosition;
                this.transform.localRotation = MainMenuUIManager.Instance.characterSelectMenus[0].transform.localRotation;
                this.transform.localScale = MainMenuUIManager.Instance.characterSelectMenus[0].transform.localScale;
                break;
            case Player.Two:
                this.transform.SetParent(MainMenuUIManager.Instance.characterSelect.transform);
                this.transform.localPosition = MainMenuUIManager.Instance.characterSelectMenus[1].gameObject.transform.localPosition;
                this.transform.localRotation = MainMenuUIManager.Instance.characterSelectMenus[1].transform.localRotation;
                this.transform.localScale = MainMenuUIManager.Instance.characterSelectMenus[1].transform.localScale;
                break;
            case Player.Three:
                this.transform.SetParent(MainMenuUIManager.Instance.characterSelect.transform);
                this.transform.localPosition = MainMenuUIManager.Instance.characterSelectMenus[2].gameObject.transform.localPosition;
                this.transform.localRotation = MainMenuUIManager.Instance.characterSelectMenus[2].transform.localRotation;
                this.transform.localScale = MainMenuUIManager.Instance.characterSelectMenus[2].transform.localScale;
                break;
            case Player.Four:
                this.transform.SetParent(MainMenuUIManager.Instance.characterSelect.transform);
                this.transform.localPosition = MainMenuUIManager.Instance.characterSelectMenus[3].gameObject.transform.localPosition;
                this.transform.localRotation = MainMenuUIManager.Instance.characterSelectMenus[3].transform.localRotation;
                this.transform.localScale = MainMenuUIManager.Instance.characterSelectMenus[3].transform.localScale;
                break;
            case Player.None:
                break;
        }

        
    }

    [PunRPC]
    void SyncPlayerReady()
    {
        readyObject.SetActive(false);
        myAudioSource.PlayOneShot(playerReadySFX);
        GameManager.Instance.SetPlayerCharacterChoice(player, selectedCharacterIndex);
        selectState = CharacterSelectState.ReadyToStart;
    }

    [PunRPC]
    void SyncBackCharacterSelection()
    {
        readyObject.SetActive(true);
        selectState = CharacterSelectState.SelectingCharacter;
    }

    private void Update()
    {
        if (LobbyConnectionHandler.instance.IsMultiplayerMode && pv.IsMine)
        {
            switch (selectState)
            {
                //case CharacterSelectState.WaitingForPlayer:

                //    if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Submit, player))
                //    {
                //        PlayerEnterGame();
                //    }
                //    break;
                case CharacterSelectState.SelectingCharacter:

                    //ChangeSelection();
                    ChangeSelectionMul();
                    if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Back, player))
                    {
                        //pressStart.SetActive(true);
                        //charSelect.SetActive(false);
                        //selectState = CharacterSelectState.WaitingForPlayer;
                        //GameManager.Instance.RemovePlayerFromGame(player);
                        Photon.Pun.PhotonNetwork.LeaveRoom();
                    }

                    if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Submit, player))
                    {
                        pv.RPC("SyncPlayerReady", RpcTarget.All);
                    }
                    break;
                case CharacterSelectState.ReadyToStart:
                    if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Back, player))
                    {
                        pv.RPC("SyncBackCharacterSelection", RpcTarget.All);
                    }
                    break;
            }
        }
        else if(!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            switch (selectState)
            {
                case CharacterSelectState.WaitingForPlayer:

                    //if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Fire, player))
                    //{
                    //    Debug.Log("FireController");
                    //}
                    //if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Dash, player))
                    //{
                    //    Debug.Log("Dash");
                    //}
                    //if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Beam, player))
                    //{
                    //    Debug.Log("Beam");
                    //}
                    //if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Back, player))
                    //{
                    //    Debug.Log("Back");
                    //}


                    if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Submit, player))
                    {
                        PlayerEnterGame();
                    }
                    break;
                case CharacterSelectState.SelectingCharacter:
                    ChangeSelection();

                    if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Back, player))
                    {
                        pressStart.SetActive(true);
                        charSelect.SetActive(false);
                        selectState = CharacterSelectState.WaitingForPlayer;
                        GameManager.Instance.RemovePlayerFromGame(player);
                    }

                    if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Submit, player))
                    {
                        readyObject.SetActive(false);
                        myAudioSource.PlayOneShot(playerReadySFX);
                        GameManager.Instance.SetPlayerCharacterChoice(player, selectedCharacterIndex);
                        selectState = CharacterSelectState.ReadyToStart;
                    }
                    break;
                case CharacterSelectState.ReadyToStart:
                    if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Back, player))
                    {
                        readyObject.SetActive(true);
                        selectState = CharacterSelectState.SelectingCharacter;
                    }
                    break;
            }
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
            if(LobbyConnectionHandler.instance.IsMultiplayerMode && pv.IsMine)
                UpdateSelectionMul();
            else
                UpdateSelection();
        }
    }
}
