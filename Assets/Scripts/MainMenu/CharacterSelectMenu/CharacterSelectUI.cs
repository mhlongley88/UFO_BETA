﻿using System.Collections;
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
    public TextMeshProUGUI playerNameText;

    public Transform characterModelContainer;
    private GameObject currentCharacterModel;

    public TextMeshPro characterLabel;
    public Image weaponTypeImage;
    public Image specialWeaponImage;
    public Slider damageSlider;
    public Slider rateOfFireSlider;
    public Slider accuracySlider;

    public GameObject newVfx;

    public Image DamageFG, RateOfFireFG, AccuracyFG;
    public bool UseWithoutStick = false;

    public GameObject oldUI, newUI;
    public Image ufoPortrait;
    public int playerLevel;
    public float playerStars;

    int _selectedCharacterIndex = 0;
    private int selectedCharacterIndex;
    //{
    //    get { return _selectedCharacterIndex; } 
    //    set 
    //    {
    //        if (!LobbyConnectionHandler.instance.IsMultiplayerMode || (pv && pv.IsMine))
    //        {
    //            int wonMatches = UnlockSystem.instance.GetMatchesCompleted();

    //            if (value >= GameManager.Instance.Characters.Length)
    //            {
    //                _selectedCharacterIndex = 0;
    //                return;
    //            }

    //            if (value < 0)
    //            {
    //                for (int e = GameManager.Instance.Characters.Length - 1; e >= 0; e--)
    //                {
    //                    if (wonMatches >= GameManager.Instance.Characters[e].matchThreshold || CharacterUnlockFromProgression.IsUnlocked(e))
    //                    {
    //                        _selectedCharacterIndex = e;
    //                        break;
    //                    }
    //                }

    //                return;
    //            }


    //            int characterUnlockedAtWonMatches = GameManager.Instance.Characters[value].matchThreshold;

    //            if (wonMatches >= characterUnlockedAtWonMatches || CharacterUnlockFromProgression.IsUnlocked(value))
    //                _selectedCharacterIndex = value;
    //            else
    //            {
    //                if (value > _selectedCharacterIndex)
    //                {
    //                    for (int e = value; e < GameManager.Instance.Characters.Length; e++)
    //                    {
    //                        characterUnlockedAtWonMatches = GameManager.Instance.Characters[e].matchThreshold;
    //                        if (wonMatches >= characterUnlockedAtWonMatches || CharacterUnlockFromProgression.IsUnlocked(e))
    //                        {
    //                            _selectedCharacterIndex = e;
    //                            break;
    //                        }
    //                        else
    //                        {
    //                            if (e == GameManager.Instance.Characters.Length - 1)
    //                            {
    //                                e = -1;
    //                            }
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    for (int e = value; e >= 0; e--)
    //                    {
    //                        characterUnlockedAtWonMatches = GameManager.Instance.Characters[e].matchThreshold;
    //                        if (wonMatches >= characterUnlockedAtWonMatches || CharacterUnlockFromProgression.IsUnlocked(e))
    //                        {
    //                            _selectedCharacterIndex = e;
    //                            break;
    //                        }
    //                        else
    //                        {
    //                            if (e == GameManager.Instance.Characters.Length - 1)
    //                            {
    //                                e = -1;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            _selectedCharacterIndex = value;
    //        }
    //    }
    //}

    public CharacterSelectState selectState = CharacterSelectState.WaitingForPlayer;

    private bool canCycle = true;
    private bool canCycleRight = true;

    public PhotonView pv;

    public CharacterSelectState GetCurSelectState()
    {
        return selectState;
    }

    private void Awake()
    {
        if (this.GetComponent<PhotonView>())
        {
            pv = this.GetComponent<PhotonView>();
        }
    }

    int rewirePlayerId = 0;
    Rewired.Player rewirePlayer;

    // Start is called before the first frame update
    void Start()
    {
        // pressStart.SetActive(true);
        //if(!UseWithoutStick)
            //characterModelContainer = this.transform.GetChild(0).GetChild(3);
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            if(playerNameText)
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
            

            int spawnIndex = 0;
            int counter = 0;
            foreach (Photon.Realtime.Player p in Photon.Pun.PhotonNetwork.PlayerList)
            {
                if (p.UserId == Photon.Pun.PhotonNetwork.LocalPlayer.UserId)
                {
                    spawnIndex = counter;
                    break;
                }
                counter++;
            }
            //Debug.Log(spawnIndex + "?");
            //rewirePlayer = ReInput.players.GetPlayer(spawnIndex);
            //rewirePlayer.controllers.maps.SetAllMapsEnabled(true);

            if (pv && pv.IsMine)
                pv.RPC("SyncMulSpawn", RpcTarget.AllBuffered, selectedCharacterIndex);

            //foreach()
            //rewirePlayer.
            //Assign Controllers for Online Match. Keyboard and all joysticks control current player.
            foreach (Rewired.Player player in ReInput.players.AllPlayers)
            {
                player.controllers.ClearControllersOfType(ControllerType.Joystick);
            }

            //rewirePlayer = ReInput.players.GetPlayer(3);// Steam version. Commented for mobile
            rewirePlayer = ReInput.players.GetPlayer(0);
            //rewirePlayer.controllers.hasKeyboard = true;
            //rewirePlayer.controllers.hasMouse = true;
            foreach (Rewired.Joystick joystick in ReInput.controllers.Joysticks)
            {
                rewirePlayer.controllers.AddController(joystick, true);
            }
            //rewirePlayer.controllers.maps.SetAllMapsEnabled(true);
            //SpawnMultiplayer();
        }
        if (UseWithoutStick)
        {
            GameManager.Instance.RemoveAllPlayersFromGame();
            SelectingCharacter_WithoutStick();
            MainMenuUIManager.Instance.touchMenuUI.SpawnSelectedCharacter(GameManager.Instance.Characters[GameManager.Instance.selectedCharacterIndex].characterModel, GameManager.Instance.Characters[GameManager.Instance.selectedCharacterIndex].characterId);
        }
    }

    private void OnDestroy()
    {
        // (NOTE: Elvis) I commented this because after getting back from the online menu, I couldnt move around the menu anymore with the keyboard
        // because disabling are controllers maps removes the controllers from the player's input, nothing works anymore.
        // If there is another reason to use this command let me know so I can work around this, but entering online mode and going back without going to 
        // level selection will disable all controls for the player, including keyboard.
        if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            //rewirePlayer.controllers.maps.SetAllMapsEnabled(false);
        }
    }

    [PunRPC]
    void SyncMulSpawn(int index)
    {
        //Debug.Log("Spawning");
        playerNameText.text = playerName;
        //Debug.Log(selectedCharacterIndex);
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

        ufoPortrait.sprite = MainMenuUIManager.Instance.touchMenuUI.UfoAvatar[index];

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

        if((GameManager.Instance.Characters[selectedCharacterIndex].matchThreshold <= UnlockSystem.instance.MatchesCompleted && UnlockSystem.instance.recentlyUnlockedCharacters.Count > 0))
        {
            if(UnlockSystem.instance.recentlyUnlockedCharacters.Contains(selectedCharacterIndex))
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

    public void SetCharacter(int characterId)
    {
        selectedCharacterIndex = characterId;
        if (selectedCharacterIndex < 0) selectedCharacterIndex = GameManager.Instance.Characters.Length - 1;
        else if (selectedCharacterIndex >= GameManager.Instance.Characters.Length) selectedCharacterIndex = 0;

        //selectedCharacterIndex = Mathf.Abs(selectedCharacterIndex) % GameManager.Instance.Characters.Length;
        Debug.Log(selectedCharacterIndex);
        Destroy(currentCharacterModel);
        currentCharacterModel = Instantiate(GameManager.Instance.Characters[selectedCharacterIndex].characterModel, Vector3.zero, Quaternion.identity, characterModelContainer);
        currentCharacterModel.transform.localPosition = Vector3.zero;
        currentCharacterModel.transform.localScale = Vector3.one;
        currentCharacterModel.transform.localEulerAngles = Vector3.zero;
        
        if ((GameManager.Instance.Characters[selectedCharacterIndex].matchThreshold <= UnlockSystem.instance.MatchesCompleted && UnlockSystem.instance.recentlyUnlockedCharacters.Count > 0))
        {
            if (UnlockSystem.instance.recentlyUnlockedCharacters.Contains(selectedCharacterIndex))
                newVfx.SetActive(true);
            else
                newVfx.SetActive(false);
            //UnlockSystem.instance.recentlyUnlockedCharacters.RemoveAt(0);
        }

        var info = currentCharacterModel.GetComponent<CharacterLevelSelectInfo>();

        //characterLabel.text = info.Name;
        //weaponTypeImage.sprite = info.WeaponType;
        //specialWeaponImage.sprite = info.SpecialWeapon;
        //damageSlider.value = info.Damage;
        //rateOfFireSlider.value = info.RateOfFire;
        //accuracySlider.value = info.Accuracy;
        DamageFG.fillAmount = info.Damage;
        RateOfFireFG.fillAmount = info.RateOfFire;
        AccuracyFG.fillAmount = info.Accuracy;
        MainMenuUIManager.Instance.HoldCharacterChoiceTemporarily(player, selectedCharacterIndex);
    }

    public void DisplayCharacterInfo_LoadOutScreen()
    {
        //selectedCharacterIndex = characterId;
        if (selectedCharacterIndex < 0) selectedCharacterIndex = GameManager.Instance.Characters.Length - 1;
        else if (selectedCharacterIndex >= GameManager.Instance.Characters.Length) selectedCharacterIndex = 0;

        //selectedCharacterIndex = Mathf.Abs(selectedCharacterIndex) % GameManager.Instance.Characters.Length;

        Destroy(currentCharacterModel);
        currentCharacterModel = Instantiate(GameManager.Instance.Characters[selectedCharacterIndex].characterModel, Vector3.zero, Quaternion.identity, characterModelContainer);
        currentCharacterModel.transform.localPosition = Vector3.zero;
        currentCharacterModel.transform.localScale = Vector3.one;
        currentCharacterModel.transform.localEulerAngles = Vector3.zero;

        if ((GameManager.Instance.Characters[selectedCharacterIndex].matchThreshold <= UnlockSystem.instance.MatchesCompleted && UnlockSystem.instance.recentlyUnlockedCharacters.Count > 0))
        {
            if (UnlockSystem.instance.recentlyUnlockedCharacters.Contains(selectedCharacterIndex))
                newVfx.SetActive(true);
            else
                newVfx.SetActive(false);
            //UnlockSystem.instance.recentlyUnlockedCharacters.RemoveAt(0);
        }

        var info = currentCharacterModel.GetComponent<CharacterLevelSelectInfo>();

        //characterLabel.text = info.Name;
        //weaponTypeImage.sprite = info.WeaponType;
        //specialWeaponImage.sprite = info.SpecialWeapon;
        //damageSlider.value = info.Damage;
        //rateOfFireSlider.value = info.RateOfFire;
        //accuracySlider.value = info.Accuracy;
        DamageFG.fillAmount = info.Damage;
        RateOfFireFG.fillAmount = info.RateOfFire;
        AccuracyFG.fillAmount = info.Accuracy;
        //MainMenuUIManager.Instance.HoldCharacterChoiceTemporarily(player, selectedCharacterIndex);
    }

    public void SetCharacter()
    {
        
        if (selectedCharacterIndex < 0) selectedCharacterIndex = GameManager.Instance.Characters.Length - 1;
        else if (selectedCharacterIndex >= GameManager.Instance.Characters.Length) selectedCharacterIndex = 0;

        //selectedCharacterIndex = Mathf.Abs(selectedCharacterIndex) % GameManager.Instance.Characters.Length;

        //Destroy(currentCharacterModel);
        //currentCharacterModel = Instantiate(GameManager.Instance.Characters[selectedCharacterIndex].characterModel, Vector3.zero, Quaternion.identity, characterModelContainer);
        //currentCharacterModel.transform.localPosition = Vector3.zero;
        //currentCharacterModel.transform.localScale = Vector3.one;
        //currentCharacterModel.transform.localEulerAngles = Vector3.zero;

        //if ((GameManager.Instance.Characters[selectedCharacterIndex].matchThreshold <= UnlockSystem.instance.MatchesCompleted && UnlockSystem.instance.recentlyUnlockedCharacters.Count > 0))
        //{
        //    if (UnlockSystem.instance.recentlyUnlockedCharacters.Contains(selectedCharacterIndex))
        //        newVfx.SetActive(true);
        //    else
        //        newVfx.SetActive(false);
        //    //UnlockSystem.instance.recentlyUnlockedCharacters.RemoveAt(0);
        //}

        var info = currentCharacterModel.GetComponent<CharacterLevelSelectInfo>();

        //characterLabel.text = info.Name;
        //weaponTypeImage.sprite = info.WeaponType;
        //specialWeaponImage.sprite = info.SpecialWeapon;
        //damageSlider.value = info.Damage;
        //rateOfFireSlider.value = info.RateOfFire;
        //accuracySlider.value = info.Accuracy;
        //DamageFG.fillAmount = info.Damage;
        //RateOfFireFG.fillAmount = info.RateOfFire;
        //AccuracyFG.fillAmount = info.Accuracy;
        MainMenuUIManager.Instance.HoldCharacterChoiceTemporarily(player, selectedCharacterIndex);
    }

    public void SpawnCharacterMainHub()
    {
        MainMenuUIManager.Instance.touchMenuUI.SpawnSelectedCharacter(GameManager.Instance.Characters[selectedCharacterIndex].characterModel, GameManager.Instance.Characters[GameManager.Instance.selectedCharacterIndex].characterId);
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

    public void SelectingCharacter_WithoutStick()
    {
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            //pressStart.SetActive(false);
            //charSelect.SetActive(true);
            GameManager.Instance.AddPlayerToGame(player);
            selectState = CharacterSelectState.SelectingCharacter;
        }
        else if(!UseWithoutStick)
        {
            int ready = 0;
            if (CharacterSelectState.ReadyToStart == selectState)
            {
                ready = 1;
            }
            else
            {
                ready = 0;
            }
            pv.RPC("PlayerEnterSync", RpcTarget.AllBuffered, LobbyConnectionHandler.instance.myDisplayName, ready);//hunz
            pv.RPC("SyncMulSpawn", RpcTarget.AllBuffered, selectedCharacterIndex);
        }
    }

    public void PlayerEnterGame(bool isBot = false)
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
            if (isBot)
            {
                isControlledByBot = isBot;
                selectedCharacterIndex = GameManager.Instance.GetPlayerCharacterChoice(player);
            }
            else
            {
                selectedCharacterIndex = GameManager.Instance.selectedCharacterIndex;
                GameManager.Instance.localPlayer = player;
            }
            
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
            pv.RPC("SyncPlayerReady", RpcTarget.AllBuffered);
        }
    }
    public bool isSynced = false, isControlledByBot = false;
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
            oldUI.SetActive(false);
            if (pv.IsMine && !isControlledByBot)
            {
                newUI.SetActive(false);
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

        if (!pv.IsMine || isControlledByBot)
        {
            this.transform.SetParent(MainMenuUIManager.Instance.touchMenuUI.OpponentCharacterSelectContainerOnline);
            this.transform.localScale = Vector3.one;
            this.transform.localPosition = new Vector3(this.transform.position.x, this.transform.position.y, 0);

            //switch (player)
            //{
            //case Player.One:
            //    this.transform.SetParent(MainMenuUIManager.Instance.characterSelect.transform);
            //    this.transform.localPosition = MainMenuUIManager.Instance.characterSelectMenus[0].gameObject.transform.localPosition;
            //    this.transform.localRotation = MainMenuUIManager.Instance.characterSelectMenus[0].transform.localRotation;
            //    this.transform.localScale = MainMenuUIManager.Instance.characterSelectMenus[0].transform.localScale;
            //    break;
            //case Player.Two:
            //    this.transform.SetParent(MainMenuUIManager.Instance.characterSelect.transform);
            //    this.transform.localPosition = MainMenuUIManager.Instance.characterSelectMenus[1].gameObject.transform.localPosition;
            //    this.transform.localRotation = MainMenuUIManager.Instance.characterSelectMenus[1].transform.localRotation;
            //    this.transform.localScale = MainMenuUIManager.Instance.characterSelectMenus[1].transform.localScale;
            //    break;
            //case Player.Three:
            //    this.transform.SetParent(MainMenuUIManager.Instance.characterSelect.transform);
            //    this.transform.localPosition = MainMenuUIManager.Instance.characterSelectMenus[2].gameObject.transform.localPosition;
            //    this.transform.localRotation = MainMenuUIManager.Instance.characterSelectMenus[2].transform.localRotation;
            //    this.transform.localScale = MainMenuUIManager.Instance.characterSelectMenus[2].transform.localScale;
            //    break;
            //case Player.Four:
            //    this.transform.SetParent(MainMenuUIManager.Instance.characterSelect.transform);
            //    this.transform.localPosition = MainMenuUIManager.Instance.characterSelectMenus[3].gameObject.transform.localPosition;
            //    this.transform.localRotation = MainMenuUIManager.Instance.characterSelectMenus[3].transform.localRotation;
            //    this.transform.localScale = MainMenuUIManager.Instance.characterSelectMenus[3].transform.localScale;
            //    break;
            //case Player.None:
            //    break;
            //}
        }
        else
        {
            //this.GetComponent<RectTransform>().localScale = Vector3.one;
            //this.GetComponent<RectTransform>().localPosition = new Vector3(this.transform.position.x, this.transform.position.y, 0);
            this.transform.SetParent(MainMenuUIManager.Instance.touchMenuUI.myCharacterSelectContainer);
            
            //this.transform.localScale = Vector3.one;
            //this.transform.localPosition = new Vector3(this.transform.position.x, this.transform.position.y, 0);
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
    public void EnableCharacterSelectSplashScreen()
    {
        pressStart.SetActive(true);
        charSelect.SetActive(false);

        selectState = CharacterSelectState.WaitingForPlayer;
        GameManager.Instance.RemovePlayerFromGame(player);
        MainMenuUIManager.Instance.selectingCharacters = false;
    }
    private void Update()
    {
        //pStatsInfo.SetActive(charSelect.activeInHierarchy);
        if (GameManager.Instance.paused)
            return;
        if (LobbyConnectionHandler.instance.IsMultiplayerMode && pv && pv.IsMine)
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

    public void SwitchCharacterMul(float side)
    {
        if (!pv.IsMine) return;

        float horizontalInput = side;
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
    }
}
