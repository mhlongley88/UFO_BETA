using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Photon.Pun;
using static InputManager;
using UnityEngine.UI;
using Rewired;

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

    public TextMeshPro characterLabel;
    public Image weaponTypeImage;
    public Image specialWeaponImage;
    public Slider damageSlider;
    public Slider rateOfFireSlider;
    public Slider accuracySlider;

    public GameObject newVfx;

    int _selectedCharacterIndex = 0;
    private int selectedCharacterIndex {get { return _selectedCharacterIndex; } 
        set 
        {
            int wonMatches = UnlockSystem.instance.GetMatchesCompleted();

            if(value >= GameManager.Instance.Characters.Length)
            {
                _selectedCharacterIndex = 0;
                return;
            }

            if(value < 0)
            {
                for(int e = GameManager.Instance.Characters.Length - 1; e >= 0; e--)
                {
                    if(wonMatches >= GameManager.Instance.Characters[e].matchThreshold || CharacterUnlockFromProgression.IsUnlocked(e))
                    {
                        _selectedCharacterIndex = e;
                        break;
                    }
                }

                return;
            }

            int characterUnlockedAtWonMatches = GameManager.Instance.Characters[value].matchThreshold;

            if(wonMatches >= characterUnlockedAtWonMatches || CharacterUnlockFromProgression.IsUnlocked(value))
                _selectedCharacterIndex = value;
            else 
            {
                if(value > _selectedCharacterIndex)
                {
                    for(int e = value; e < GameManager.Instance.Characters.Length; e++)
                    {
                        characterUnlockedAtWonMatches = GameManager.Instance.Characters[e].matchThreshold;
                        if(wonMatches >= characterUnlockedAtWonMatches || CharacterUnlockFromProgression.IsUnlocked(e))
                        {
                            _selectedCharacterIndex = e;
                            break;
                        }
                        else 
                        {
                            if(e == GameManager.Instance.Characters.Length - 1)
                            {
                                e = -1;
                            }
                        }
                    }
                }
                else
                {
                    for (int e = value; e >= 0; e--)
                    {
                        characterUnlockedAtWonMatches = GameManager.Instance.Characters[e].matchThreshold;
                        if (wonMatches >= characterUnlockedAtWonMatches || CharacterUnlockFromProgression.IsUnlocked(e))
                        {
                            _selectedCharacterIndex = e;
                            break;
                        }
                        else
                        {
                            if (e == GameManager.Instance.Characters.Length - 1)
                            {
                                e = -1;
                            }
                        }
                    }
                }
            }
        }
    }

    private CharacterSelectState selectState = CharacterSelectState.WaitingForPlayer;

    private bool canCycle = true;
    private bool canCycleRight = true;

    public PhotonView pv;

    public CharacterSelectState GetCurSelectState()
    {
        return selectState;
    }

    int rewirePlayerId = 0;
    Rewired.Player rewirePlayer;

    // Start is called before the first frame update
    void Start()
    {
        // pressStart.SetActive(true);
        characterModelContainer = this.transform.GetChild(0).GetChild(3);
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            playerNameText.text = playerName;
            if(currentCharacterModel == null)
                currentCharacterModel = Instantiate(GameManager.Instance.Characters[selectedCharacterIndex].characterModel, Vector3.zero, Quaternion.identity, characterModelContainer);

            var info = currentCharacterModel.GetComponent<CharacterLevelSelectInfo>();

            characterLabel.text = info.Name;
            weaponTypeImage.sprite = info.WeaponType;
            specialWeaponImage.sprite = info.SpecialWeapon;
            damageSlider.value = info.Damage;
            rateOfFireSlider.value = info.RateOfFire;
            accuracySlider.value = info.Accuracy;

            switch (player)
            {
                case Player.One: rewirePlayerId = 0; break;
                case Player.Two: rewirePlayerId = 1; break;
                case Player.Three: rewirePlayerId = 2; break;
                case Player.Four: rewirePlayerId = 3; break;
            }

            rewirePlayer = ReInput.players.GetPlayer(rewirePlayerId);
        }
        else
        {
            pv = this.GetComponent<PhotonView>();
            if(pv.IsMine)
                pv.RPC("SyncMulSpawn", RpcTarget.AllBuffered, selectedCharacterIndex);

            rewirePlayer = ReInput.players.GetPlayer(0);
            rewirePlayer.controllers.maps.SetAllMapsEnabled(true);
            //SpawnMultiplayer();
        }
    }

    private void OnDestroy()
    {
        if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            rewirePlayer.controllers.maps.SetAllMapsEnabled(false);
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

        var info = currentCharacterModel.GetComponent<CharacterLevelSelectInfo>();

        characterLabel.text = info.Name;
        weaponTypeImage.sprite = info.WeaponType;
        specialWeaponImage.sprite = info.SpecialWeapon;
        damageSlider.value = info.Damage;
        rateOfFireSlider.value = info.RateOfFire;
        accuracySlider.value = info.Accuracy;
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
        pv.RPC("SyncMulSpawn", RpcTarget.AllBuffered, selectedCharacterIndex);
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

        if((GameManager.Instance.Characters[selectedCharacterIndex].matchThreshold <= UnlockSystem.instance.MatchesCompleted && UnlockSystem.instance.recentlyUnlockedCharacters.Count > 0) ||
            CharacterUnlockFromProgression.IsUnlocked(selectedCharacterIndex))
        {
            if(UnlockSystem.instance.recentlyUnlockedCharacters.Contains(GameManager.Instance.Characters[selectedCharacterIndex].matchThreshold))
                newVfx.SetActive(true);
            else
                newVfx.SetActive(false);
            //UnlockSystem.instance.recentlyUnlockedCharacters.RemoveAt(0);
        }

        var info = currentCharacterModel.GetComponent<CharacterLevelSelectInfo>();

        characterLabel.text = info.Name;
        weaponTypeImage.sprite = info.WeaponType;
        specialWeaponImage.sprite = info.SpecialWeapon;
        damageSlider.value = info.Damage;
        rateOfFireSlider.value = info.RateOfFire;
        accuracySlider.value = info.Accuracy;
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
        //float horizontalInput = InputManager.Instance.GetAxis(AxisEnum.LeftStickHorizontal, player);
        float horizontalInput = rewirePlayer.GetAxis("Horizontal");
        float keyThreshold = 0.8f;

        if (player == Player.Four || LobbyConnectionHandler.instance.IsMultiplayerMode)
            keyThreshold = 0.1f;

        if (canCycle)
        {
            if (horizontalInput < -keyThreshold)
            {
                SyncSelection(false);
                //pv.RPC("SyncSelection", RpcTarget.All, false);

            }
            else if (horizontalInput > keyThreshold)
            {
                SyncSelection(true);
                //pv.RPC("SyncSelection", RpcTarget.All, true);
            }
        }
        else
        {
            if (horizontalInput > -keyThreshold && horizontalInput < keyThreshold)
            {
                canCycle = true;
            }
        }

        //float horizontalInputKB = rewirePlayer.GetAxis("Horizontal");
        ////Debug.Log(horizontalInputKB);
        //if (canCycle)
        //{
        //    if (rewirePlayer.GetButtonDown("Submit"))
        //    {
        //        //pv.RPC("SyncSelection", RpcTarget.All, false);
        //        SyncSelection(false);
        //    }
        //    else if (rewirePlayer.GetButtonDown("DKey"))
        //    {
        //        //pv.RPC("SyncSelection", RpcTarget.All, true);
        //        SyncSelection(true);
        //    }
        //}
        //else
        //{
        //    if (horizontalInputKB > -0.8f && horizontalInputKB < 0.8f)
        //    {
        //        canCycle = true;
        //    }
        //}

    }

    private void ChangeSelection()
    {
        //float horizontalInput = InputManager.Instance.GetAxis(AxisEnum.LeftStickHorizontal, player);
        float horizontalInput = rewirePlayer.GetAxis("Horizontal");
        float keyThreshold = 0.8f;

        if (player == Player.Four)
            keyThreshold = 0.1f;

        if (canCycle)
        {
            if (horizontalInput < -keyThreshold)
            {
                selectedCharacterIndex--;
                canCycle = false;
                UpdateSelection();
            }
            else if (horizontalInput > keyThreshold)
            {
                selectedCharacterIndex++;
                canCycle = false;
                UpdateSelection();
            }
        }
        else
        {
            if (horizontalInput > -keyThreshold && horizontalInput < keyThreshold)
            {
                canCycle = true;
            }
        }

        //if (LobbyConnectionHandler.instance.IsMultiplayerMode || player == Player.Four)
        //{
        //    float horizontalInputKB = rewirePlayer.GetAxis("Horizontal");
        //    //Debug.Log(horizontalInputKB);
        //    if (canCycle)
        //    {
        //        if (rewirePlayer.GetButtonDown("Submit"))
        //        {
        //            selectedCharacterIndex--;
        //            canCycle = false;
        //            UpdateSelection();
        //        }
        //        else if (rewirePlayer.GetButtonDown("DKey"))
        //        {
        //            selectedCharacterIndex++;
        //            canCycle = false;
        //            UpdateSelection();
        //        }
        //    }
        //    else
        //    {
        //        if (horizontalInputKB > -0.8f && horizontalInputKB < 0.8f)
        //        {
        //            canCycle = true;
        //        }
        //    }
        //}

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
            int ready = 0;
            if(CharacterSelectState.ReadyToStart == selectState)
            {
                ready = 1;
            }
            else
            {
                ready = 0;
            }
            pv.RPC("PlayerEnterSync", RpcTarget.AllBuffered, LobbyConnectionHandler.instance.myDisplayName,ready);//hunz
            pv.RPC("SyncMulSpawn", RpcTarget.AllBuffered, selectedCharacterIndex);
        }
    }
    public bool isSynced = false;
    [PunRPC]
    void PlayerEnterSync(string displayName, int isReady)
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
            playerName = playerNameText.text = displayName;
            if(isReady == 1)
            {
                selectState = CharacterSelectState.ReadyToStart;
            }
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
        //pStatsInfo.SetActive(charSelect.activeInHierarchy);

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
     
                    //if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Back, player))
                    if (rewirePlayer.GetButtonDown("Back"))
                    {
                        //pressStart.SetActive(true);
                        //charSelect.SetActive(false);
                        //selectState = CharacterSelectState.WaitingForPlayer;
                        //GameManager.Instance.RemovePlayerFromGame(player);
                        Photon.Pun.PhotonNetwork.LeaveRoom();
                    }

                    //if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Submit, player))
                    if (rewirePlayer.GetButtonDown("Submit"))
                    {
                        pv.RPC("SyncPlayerReady", RpcTarget.AllBuffered);
                    }
                    break;
                case CharacterSelectState.ReadyToStart:
                    //if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Back, player))
                    if (rewirePlayer.GetButtonDown("Back"))
                    {
                        pv.RPC("SyncBackCharacterSelection", RpcTarget.AllBuffered);
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

                    //if (MainMenuUIManager.Instance.selectingCharacters)
                    //{
                    //    MainMenuUIManager.Instance.selectingCharacters = false;
                    //}

                    //if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Submit, player))
                    if (rewirePlayer.GetButtonDown("Submit"))
                    {
                        PlayerEnterGame();
                        MainMenuUIManager.Instance.selectingCharacters = true;
                    }
                    break;
                case CharacterSelectState.SelectingCharacter:
                    ChangeSelection();

                    //if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Back, player))
                    if (rewirePlayer.GetButtonDown("Back"))
                    {
                        pressStart.SetActive(true);
                        charSelect.SetActive(false);

                        selectState = CharacterSelectState.WaitingForPlayer;
                        GameManager.Instance.RemovePlayerFromGame(player);
                        MainMenuUIManager.Instance.selectingCharacters = false;
                    }

                    //if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Submit, player))
                    if (rewirePlayer.GetButtonDown("Submit"))
                    {
                        readyObject.SetActive(false);
                        myAudioSource.PlayOneShot(playerReadySFX);
                        GameManager.Instance.SetPlayerCharacterChoice(player, selectedCharacterIndex);
                        selectState = CharacterSelectState.ReadyToStart;
                    }
                    break;
                case CharacterSelectState.ReadyToStart:

                    //if (InputManager.Instance.GetButtonDownCharacterSelection(ButtonEnum.Back, player))
                    if (rewirePlayer.GetButtonDown("Back"))
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
            if(pressStart)
                pressStart.SetActive(false);
            selectState = CharacterSelectState.SelectingCharacter;
            MainMenuUIManager.Instance.selectingCharacters = true;

            if (LobbyConnectionHandler.instance.IsMultiplayerMode && pv && pv.IsMine)
                UpdateSelectionMul();
            //else if (LobbyConnectionHandler.instance.IsMultiplayerMode && !pv)
            //    Destroy(this.gameObject);
            else if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
                UpdateSelection();
        }
    }
}
