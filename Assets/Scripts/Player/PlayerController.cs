using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using RotaryHeart.Lib.SerializableDictionary;
using static Weapon;
using System;
using static InputManager;
using Photon.Pun;
using Rewired;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public static Dictionary<GameObject, PlayerController> playerControllerByGameObject = new Dictionary<GameObject, PlayerController>();

    public bool pawn = false;
    [HideInInspector]
    public GameObject pawnModel;

    public bool canGrowWhenUsingSpecial = true;
   // [HideInInspector]
    public Vector3 offsetScale;

    public Player player;
   // public GameObject PlayerSpecialvCam;
    public bool twinStick = false;
    private float timeStamp;
    private Rigidbody myRigidbody;
    public float radius;
    public float power;
    public float boostHitMultiplier;

    public float massMultiplier = 1f;

    public float abductMaxSpeed;
    public float maxSpeed;
    public float acceleration;

    public float scaleSpeed = 1.0f;

    public float boostSpeed = 10f;
    public float boostDuration = 0.5f;

    public float boostCooldown;
    public float shootCooldown;

    public float deadZone = 0f;

    public GameObject beam;
    public GameObject beamOff;
    public float beamForce;

    private float totalSpeed;

    private bool inBoost;
    private bool inBeam;

    private float stopUFO = 0.0f;
    public float hitSubtract;
    public float bulletHitSubtract;

    private GameObject ufoSaucer;

    public static bool hit_p1;

    public GameObject dropItem;
    public Transform dropPoint;

    public static float point_p1;

    // public string input_movementHorizontal;
    // public string input_movementVertical;

    // public string input_rightStickHorizontal;
    // public string input_rightStickVertical;
    // public string input_beam;
    // public string input_dash;
    // public string input_shoot;
    // public string input_heal;
    // public string input_superWeapon;
    // public string input_superWeapon2;

    public GameObject DeathPFX;

    private float horizontalInput;
    private float verticalInput;
    private bool beamInput;
    private bool dashInput;
    public GameObject aimFlagObject;
    public GameObject DashPFX;

    public float invincibleDuration = 3.0f;

    private Vector3 moveInputVector;
    private float scaleDelta = 0f;
    private Vector3 originalScale;
    private bool boostReady = true;
    private bool gunReady = true;
    private float originalMass = 1f;
    private Stack<GameObject> abductedObjects = new Stack<GameObject>();
    // private bool invincible = false;

    public Image energyMeter;
    public float energyMax;
    public float powerMin;
    public float superWeaponEnergy;

    public PlayerHealthManager healthManager;

    public float defaultHealthDamage = -1f;
    public float defaultScaleDamage = -0.4f;

    // amount of health & scale gained when player heal's himself
    public float healHealthAmount;
    public float healScaleAmount;


    public float maxScaleDelta;
    public float minScaleDelta;
    /* 
    public float scaleMultiplier = 3.0f;
    public Vector3 scaleAdd;
*/
    public int minusCount;

    private AverageScaleOutput avgScaleOutput;
    private AveragePosition avgPos;
    private bool isBoosting = false;
    private Vector2 moveDirection = Vector2.zero;
    float boostCounter = 0.0f;

    [SerializeField]
    private NormalWeapon normalWeapon;

    [SerializeField]
    private SuperWeapon superWeapon;

    private Weapon currentWeapon;
    public Weapon CurrentWeapon { get { return currentWeapon; } }

    private bool isAbducting = false;

    public bool superWeaponActive = false;

    bool dead = false;

    private Vector2 rightStickDirection;

    public GameObject specialReady;
    public Image playerIndicator;

    [SerializeField]
    private Transform modelContainer;

    [HideInInspector]
    public GameObject playerModel;
    PhotonView pv;

    [HideInInspector]
    public bool allowLocalProcessInput = true;

    IgnoreThisColliderWithParentPlayer[] collidersToIgnore;

    int rewirePlayerId;
    Rewired.Player rewirePlayer;

    [HideInInspector]
    public delegate void TakeDamage(float d);
    public TakeDamage OnTakeDamage;

    public float mouseRotationDamp = 16.0f;
    // For Bots
    //[HideInInspector]
    //public bool inputAbduction;
    //bool oldInputAbduction;

    Plane castPlane;
    Vector3 rayShootPoint;

    CharacterLevelSelectInfo characterInfo;

    private void Awake()
    {
        if (this.GetComponent<PhotonView>())
        {
            pv = this.GetComponent<PhotonView>();
        }
        playerControllerByGameObject.Add(gameObject, this);

        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
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
            rewirePlayer = ReInput.players.GetPlayer(0);
            rewirePlayer.controllers.maps.SetAllMapsEnabled(true);
        }

        castPlane = new Plane(Vector3.up, transform.position);
    }

    IEnumerator Start()
    {
        //if(TutorialManager.instance != null && player == Player.One && GameManager.Instance.GetActivePlayers().Count == 1)
        //    rewirePlayer.controllers.maps.SetAllMapsEnabled(true);

        currentWeapon = normalWeapon;
        avgScaleOutput = GameObject.FindGameObjectWithTag("AverageScaleOutput").GetComponent<AverageScaleOutput>();
        avgScaleOutput.AddPlayer(this.gameObject.transform);
        healthManager.SetInvincible(true);
        myRigidbody = GetComponent<Rigidbody>();
        beam.SetActive(false);
        originalScale = transform.localScale;

        transform.localScale = offsetScale + originalScale;

        originalMass = myRigidbody.mass;
        powerMin = 0.0f;

        {
            playerModel = Instantiate(pawn ? pawnModel : GameManager.Instance.GetPlayerModel(player), modelContainer);
            playerModel.transform.localRotation = Quaternion.identity;
            playerModel.transform.localPosition = Vector3.zero;

            // Mark that you played with this model
            if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
            {
                GameManager.Instance.SetPlayerPlayedWithThisModel(player);
            }
            else
            {
                if(pv.IsMine)
                    GameManager.Instance.SetPlayerPlayedWithThisModel(player);
            }
        }

        characterInfo = playerModel.GetComponent<CharacterLevelSelectInfo>();

       // if (!pawn)
        {
            // Wait a few frames to LifeManager be assigned in the Health Manager, this should be quick (Elvis)
            while (healthManager.LifeManager == null) yield return null;

            if(healthManager.LifeManager != null)
                healthManager.LifeManager.characterHead.sprite = characterInfo.CharacterHead;

            healthManager.ApplyTintOnCircles(characterInfo.CharacterLivesCircleTint);
        }
        normalWeapon.ChangeWeapon(GameManager.Instance.GetCharacterNormalWeapon(GameManager.Instance.GetPlayerCharacterChoice(player)));
        superWeapon.ChangeWeapon(GameManager.Instance.GetCharacterSuperWeapon(GameManager.Instance.GetPlayerCharacterChoice(player)));

        normalWeapon.SetMuzzleFlash(characterInfo.MuzzleFlashVfx);
        superWeapon.SetMuzzleFlash(characterInfo.MuzzleFlashVfx);

        playerIndicator.color = characterInfo.CharacterLivesCircleTint;

        if(PlayerBot.active && !PlayerBot.chosenPlayer.Contains(player))
        {
            playerIndicator.gameObject.SetActive(true);
        }

        if (!pawn) yield return new WaitForSeconds(invincibleDuration);
        
        healthManager.SetInvincible(false);

        //if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        //{
        //    Debug.Log("PC");
        //    isConsole = false;
        //   // isPC = true;
        //}
        //else if (Application.platform == RuntimePlatform.PS4 || Application.platform == RuntimePlatform.XboxOne)
        //{
        //    Debug.Log("Console");
        //    isPC = false;
        //    isConsole = true;
        //}

        collidersToIgnore = GetComponentsInChildren<IgnoreThisColliderWithParentPlayer>();
    }

    private void OnDestroy()
    {
        playerControllerByGameObject.Remove(gameObject);
    }

    public void ActivateBeam()
    {
        beam.SetActive(true);
        isAbducting = true;
    }

    public void DeactivateBeam()
    {
        beam.SetActive(false);
        isAbducting = false;
    }

    //Vector2 GetInputAxis()
    //{
    //    return GameManager.Instance.paused ? Vector2.zero :
    //        new Vector2(
    //            InputManager.Instance.GetAxis(AxisEnum.LeftStickHorizontal, player),
    //            InputManager.Instance.GetAxis(AxisEnum.LeftStickVertical, player)
    //        );
    //}

    //Vector2 GetInputAxisKB()
    //{
    //    return GameManager.Instance.paused ? Vector2.zero :
    //        new Vector2(
    //            InputManager.Instance.GetAxisKB(AxisEnum.LeftStickHorizontal, player),
    //            InputManager.Instance.GetAxisKB(AxisEnum.LeftStickVertical, player)
    //        );
    //}

    Vector2 GetInputAxis()
    {
        return (GameManager.Instance.paused || GameManager.Instance.HasCutsceneObjectsActive) ? Vector2.zero : rewirePlayer.GetAxis2D("Horizontal", "Vertical");
    }

    bool isControllerDecided = false;
    private void ProcessInput_PC()
    {
        //horizontalInput = Input.GetAxis("P1_Horizontal_Axis_Keyboard");
        //verticalInput = Input.GetAxis("P1_Vertical_Axis_Keyboard");
        //moveInputVector = new Vector3(horizontalInput, 0.0f, verticalInput);
        //Vector2 axis = GetInputAxis();

        //if (!isControllerDecided)
        //{
        //    if (GetInputAxis() != Vector2.zero)
        //    {
        //        isConsole = true;
        //        isPC = false;
        //        isControllerDecided = true;
                
        //    }
        //    if (GetInputAxisKB() != Vector2.zero)
        //    {
        //        isPC = true;
        //        isConsole = false;
        //        isControllerDecided = true;

        //    }
            
        //}
        //else
        {
            //if (isPC)
            //{
            //    Vector2 axisKB = GetInputAxisKB();
            //    horizontalInputKB = axisKB.x;
            //    verticalInputKB = axisKB.y;
            //    moveInputVector = new Vector3(horizontalInputKB, 0.0f, verticalInputKB);
            //}
            //else if (isConsole)
            //{
            //    Vector2 axis = GetInputAxis();
            //    horizontalInput = axis.x;
            //    verticalInput = axis.y;
            //    moveInputVector = new Vector3(horizontalInput, 0.0f, verticalInput);
            //}
            //else
            //{
            //    isControllerDecided = false;
            //}
        }

        Vector2 axis = GetInputAxis();
        horizontalInput = axis.x;
        verticalInput = axis.y;
        moveInputVector = new Vector3(horizontalInput, 0.0f, verticalInput);

        if (GameManager.Instance.paused || GameManager.Instance.HasCutsceneObjectsActive)
        {
            if (isAbducting)
                pv.RPC("RPC_Beam_Off", RpcTarget.AllBuffered);

            return;
        }
        //if (Input.GetButtonDown("P1_Beam_Keyboard") && energyMeter.fillAmount != 1f)
        if ((rewirePlayer.GetButtonDown("Abduct") ) && energyMeter.fillAmount != 1f)
        {
            Debug.Log("Beam Input");
            
            pv.RPC("RPC_Beam", RpcTarget.AllBuffered);
        }
        else if ((rewirePlayer.GetButtonUp("Abduct")))
        {
            pv.RPC("RPC_Beam_Off", RpcTarget.AllBuffered);

        }

        if (twinStick)
        {
            
            //rightStickDirection = new Vector2(InputManager.Instance.GetAxis(AxisEnum.RightStickHorizontal, player), InputManager.Instance.GetAxis(AxisEnum.RightStickVertical, player));
            rightStickDirection = rewirePlayer.GetAxis2D("AimHorizontal", "AimVertical");

            //if (player != Player.Four)
            {
                if (rightStickDirection != Vector2.zero)
                {

                    this.transform.localEulerAngles = new Vector3(0f, Mathf.Atan2(rightStickDirection.x, rightStickDirection.y) * 180 / Mathf.PI, 0f);
                }
                //Debug.Log("console");
            }
            
            if((player == Player.Four || LobbyConnectionHandler.instance.IsMultiplayerMode) && rightStickDirection == Vector2.zero)
            {
                // if (rightStickDirection != Vector2.zero)// > 0.1)//rightStickDirection != Vector2.zero)
                castPlane.SetNormalAndPosition(Vector3.up, transform.position);
                float dist;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (castPlane.Raycast(ray, out dist))
                {
                    rayShootPoint = ray.GetPoint(dist);

                    Quaternion targetRotation = Quaternion.LookRotation(rayShootPoint - transform.position, Vector3.up);
                    targetRotation.x = 0;
                    targetRotation.z = 0;
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.smoothDeltaTime * mouseRotationDamp);
                }

                //this.transform.localEulerAngles = new Vector3(0f, Mathf.Atan2(rightStickDirection.x, rightStickDirection.y) * 180 / Mathf.PI, 0f);

                //Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

                //Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

                //float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

                //transform.rotation = Quaternion.Euler(new Vector3(0f, -angle, 0f));
            }
        }

        currentWeapon.UpdateShootDirection(transform.forward);
        //if (InputManager.Instance.GetButtonDown(ButtonEnum.Fire, player) && gunReady && !InputManager.Instance.GetButton(ButtonEnum.Beam, player))
        if (rewirePlayer.GetButtonDown("Shoot") && gunReady && !rewirePlayer.GetButton("Abduct"))
        {

            //pv.RPC("RPC_Fire_Others", RpcTarget.All, transform.forward);
            RPC_Fire(transform.forward);
        }

        //if (InputManager.Instance.GetButtonDown(ButtonEnum.Dash, player) && boostReady)
        if (rewirePlayer.GetButtonDown("Dash") && boostReady)
        {
            tryToBoost();
        }

        //if (isConsole)
        //{
        //    //if (IsSuperWeaponReady() && InputManager.Instance.GetAxis(AxisEnum.ActivateSuperWeapon1, player) > 0.8f && InputManager.Instance.GetAxis(AxisEnum.ActivateSuperWeapon2, player) > 0.8f)
        //    if (IsSuperWeaponReady() && rewirePlayer.GetButton("ActivateSuperWeapon1") && rewirePlayer.GetButton("ActivateSuperWeapon2"))
        //    {
        //        // Debug.Log(InputManager.Instance.GetAxis(AxisEnum.ActivateSuperWeapon2, player));
        //        pv.RPC("RPC_ToggleSpecialWeapon", RpcTarget.AllBuffered);
        //    }
        //}
        //else
        {
            //Debug.Log(InputManager.Instance.GetAxisKB(AxisEnum.ActivateSuperWeapon1, player) + "1");
            //Debug.Log(InputManager.Instance.GetAxisKB(AxisEnum.ActivateSuperWeapon2, player) + "2");
            if (IsSuperWeaponReady() && rewirePlayer.GetButton("ActivateSuperWeapon1") && rewirePlayer.GetButton("ActivateSuperWeapon2"))
            {
                // Debug.Log(InputManager.Instance.GetAxis(AxisEnum.ActivateSuperWeapon2, player));
                pv.RPC("RPC_ToggleSpecialWeapon", RpcTarget.AllBuffered);
            }
        }

    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.x - b.x, b.y - a.y) * Mathf.Rad2Deg;
    }

    [PunRPC]
    void RPC_Beam()
    {
        if (pv.IsMine)
        {
            ActivateBeam();
        }
        else
        {
            ActivateBeam();
        }
        
    }

    [PunRPC]
    void RPC_Beam_Off()
    {
        if (pv.IsMine)
        {
            DeactivateBeam();
            Instantiate(beamOff, gameObject.transform.position, gameObject.transform.rotation);
        }
        else
        {
            DeactivateBeam();
            Instantiate(beamOff, gameObject.transform.position, gameObject.transform.rotation);
        }

    }

    [PunRPC]
    void Death_RPC(int livesLeft)
    {
        dead = true;

        avgScaleOutput.RemovePlayer(this.gameObject.transform);

        PlayerManager.Instance.players[player].lives = livesLeft;
        PlayerManager.Instance.PlayerDied(player, playerModel.transform);

        if (pv.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }

        Instantiate(DeathPFX, gameObject.transform.position, gameObject.transform.rotation);
        
    }

    [PunRPC]
    void RPC_ToggleSpecialWeapon()
    {
        ToggleSuperWeapon(true);
    }

    //[PunRPC]
    void RPC_Fire(Vector3 fireDirection)
    {
        
        if (pv.IsMine)
        {
            Debug.Log(currentWeapon.name);

            currentWeapon.Fire();
        }
        //else if(currentWeapon.canFire && currentWeapon.currentAmmo > 0)
        //{
        //    currentWeapon.Fire_OtherInstances(fireDirection);
        //}
        // Debug.Log("Fire1");
    }

    [PunRPC]
    void RPC_Fire_Others(Vector3 fireDirection)
    {
        currentWeapon.Fire_OtherInstances(fireDirection);
    }


    //float horizontalInputKB, verticalInputKB;
    //Vector3 moveInputVectorKB;

    private void ProcessInput()
    {
        //if (!isControllerDecided)
        //{
        //    if (GetInputAxis() != Vector2.zero)
        //    {
        //        isConsole = true;
        //        isPC = false;
        //        isControllerDecided = true;

        //    }
        //    if (GetInputAxisKB() != Vector2.zero)
        //    {
        //       // Debug.Log("aksjckajc" + player + "---" + GetInputAxisKB());
        //        isPC = true;
        //        isConsole = false;
        //        isControllerDecided = true;

        //    }

        //}
        //else
        //{
        //    if (isPC)
        //    {
        //        Vector2 axisKB = GetInputAxisKB();
        //        horizontalInputKB = axisKB.x;
        //        verticalInputKB = axisKB.y;
        //        moveInputVector = new Vector3(horizontalInputKB, 0.0f, verticalInputKB);
        //    }
        //    else if (isConsole)
        //    {
        //        Vector2 axis = GetInputAxis();
        //        horizontalInput = axis.x;
        //        verticalInput = axis.y;
        //        moveInputVector = new Vector3(horizontalInput, 0.0f, verticalInput);
        //    }
        //    else
        //    {
        //        isControllerDecided = false;
        //    }
        //}

        Vector2 axis = GetInputAxis();
        horizontalInput = axis.x;
        verticalInput = axis.y;
        moveInputVector = new Vector3(horizontalInput, 0.0f, verticalInput);


        if (GameManager.Instance.paused || GameManager.Instance.HasCutsceneObjectsActive)
        {
            if (isAbducting)
                DeactivateBeam();

            return;
        }

        //if (horizontalInput != 0f || verticalInput != 0f)
        //{
        //    this.transform.localEulerAngles = new Vector3(0f, Mathf.Atan2(horizontalInput, verticalInput) * 180 / Mathf.PI, 0f);
        //}   

        //if (InputManager.Instance.GetButtonDown(ButtonEnum.Beam, player) && energyMeter.fillAmount != 1f)
        if ((rewirePlayer.GetButtonDown("Abduct")) && energyMeter.fillAmount != 1f)
        {
            Debug.Log("Beam Input");
            ActivateBeam();
        }
        //else if (InputManager.Instance.GetButtonUp(ButtonEnum.Beam, player))
        else if (rewirePlayer.GetButtonUp("Abduct"))
        {
            DeactivateBeam();

           /* GameObject beamInstance = PlayerManager.Instance.beamOffCache.GetInstance();
            beamInstance.transform.position = gameObject.transform.position;
            beamInstance.transform.rotation = gameObject.transform.rotation;
            beamInstance.SetActive(true);*/
        }
        if (twinStick)
        {
            //rightStickDirection = new Vector2(InputManager.Instance.GetAxis(AxisEnum.RightStickHorizontal, player), InputManager.Instance.GetAxis(AxisEnum.RightStickVertical, player));
            rightStickDirection = rewirePlayer.GetAxis2D("AimHorizontal", "AimVertical");

            //if (player != Player.Four)
            {
                if (rightStickDirection != Vector2.zero)
                {
                    this.transform.localEulerAngles = new Vector3(0f, Mathf.Atan2(rightStickDirection.x, rightStickDirection.y) * 180 / Mathf.PI, 0f);
                }
                //Debug.Log("console");
            }

            if ((player == Player.Four || LobbyConnectionHandler.instance.IsMultiplayerMode) && rightStickDirection == Vector2.zero)
            {
                // if (rightStickDirection != Vector2.zero)// > 0.1)//rightStickDirection != Vector2.zero)
                {
                    castPlane.SetNormalAndPosition(Vector3.up, transform.position);
                    float dist;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (castPlane.Raycast(ray, out dist))
                    {
                        rayShootPoint = ray.GetPoint(dist);

                        Quaternion targetRotation = Quaternion.LookRotation(rayShootPoint - transform.position, Vector3.up);
                        targetRotation.x = 0;
                        targetRotation.z = 0;
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.smoothDeltaTime * mouseRotationDamp);
                    }

                    //Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

                    //Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

                    //float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

                    //transform.rotation = Quaternion.Euler(new Vector3(0f, -angle, 0f));
                }
            }
        }
        currentWeapon.UpdateShootDirection(transform.forward);
        //if (((currentWeapon.GetCurrentWeaponSettings().AutoFire && InputManager.Instance.GetButton(ButtonEnum.Fire, player)) || InputManager.Instance.GetButtonDown(ButtonEnum.Fire, player)) && gunReady && !InputManager.Instance.GetButton(ButtonEnum.Beam, player))
        if (((currentWeapon.GetCurrentWeaponSettings().AutoFire && rewirePlayer.GetButtonDown("Shoot")) || rewirePlayer.GetButtonDown("Shoot")) && gunReady && !rewirePlayer.GetButton("Abduct"))
        {
            currentWeapon.Fire();
        }
        //if (InputManager.Instance.GetButtonDown(ButtonEnum.Dash, player) && boostReady)
        if (rewirePlayer.GetButtonDown("Dash") && boostReady)
        {
            tryToBoost();
        }

        //if (isConsole)
        //{
        //    //if (IsSuperWeaponReady() && InputManager.Instance.GetAxis(AxisEnum.ActivateSuperWeapon1, player) > 0.8f && InputManager.Instance.GetAxis(AxisEnum.ActivateSuperWeapon2, player) > 0.8f)
        //    if (IsSuperWeaponReady() && rewirePlayer.GetButton("ActivateSuperWeapon1") && rewirePlayer.GetButton("ActivateSuperWeapon2"))
        //    {
        //        ToggleSuperWeapon(true);
        //    }
        //}
        //else
        {
            if (IsSuperWeaponReady() && rewirePlayer.GetButton("ActivateSuperWeapon1") && rewirePlayer.GetButton("ActivateSuperWeapon2"))
            {
                ToggleSuperWeapon(true);
            }
        }

    }

    IEnumerator DeactivateSpecialCamera()
    {
        yield return new WaitForSeconds(2.5f);
       // PlayerSpecialvCam.SetActive(false);
    }

    public void ToggleSuperWeapon(bool activate)
    {
        superWeaponActive = activate;
        if (activate)
        {
           // superWeaponActive = activate;
            //StartCoroutine(CameraController.Instance.ActivateSpecialCamera(1f));
            // if(pv.IsMine)
           // PlayerSpecialvCam.SetActive(true);
            normalWeapon.gameObject.SetActive(false);
            superWeapon.gameObject.SetActive(true);
            superWeapon.ActivateWeapon();
            currentWeapon = superWeapon;
            Debug.Log(currentWeapon.name);
            StartCoroutine(DeactivateSpecialCamera());
        }
        else
        {
            Debug.Log("Deactivating");
           // PlayerSpecialvCam.SetActive(false);
            normalWeapon.gameObject.SetActive(true);
            superWeapon.gameObject.SetActive(false);
            //superWeaponActive = false;
            currentWeapon = normalWeapon;
            normalWeapon.ReloadWeapon();

            currentWeapon.canFire = true;
            //superWeapon.DeactivateWeapon();
            Debug.Log(currentWeapon.name);
            //normalWeapon.ChangeWeapon(GameManager.Instance.GetCharacterNormalWeapon(GameManager.Instance.GetPlayerCharacterChoice(player)));
            energyMeter.fillAmount = 0f;
            scaleDelta = 0f;
        }
    }


    public void tryToBoost()
    {
        //if (isConsole)
        {
            if (horizontalInput != 0f || verticalInput != 0f)
            {
                StartCoroutine(BoostCoroutine());
            }
        }
        //else if(isPC)
        //{
        //    if (horizontalInputKB != 0f || verticalInputKB != 0f)
        //    {
        //        StartCoroutine(BoostCoroutine());
        //    }
        //}
    }

    private IEnumerator BoostCoroutine()
    {
        boostReady = false;
        isBoosting = true;
        Vector3 boostDirection;// = new Vector3(horizontalInput, 0.0f, verticalInput);
        //if (isConsole)
        {
            boostDirection = new Vector3(horizontalInput, 0.0f, verticalInput);
        }
        //else
        //{
        //    boostDirection = new Vector3(horizontalInputKB, 0.0f, verticalInputKB);
        //}

        float timer = 0f;
        myRigidbody.velocity = Vector3.zero;
        while (timer <= boostDuration)
        {
            //myRigidbody.MovePosition(boostDirection.normalized * boostSpeed * Time.fixedDeltaTime + transform.position);
            myRigidbody.velocity = (transform.position + boostDirection.normalized * boostSpeed) - transform.position;
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity.normalized, GetMaxSpeed());

        boostCounter = 0.0f;
        isBoosting = false;

       /* GameObject dashPfxInstance = PlayerManager.Instance.dashCache.GetInstance();
        dashPfxInstance.transform.position = gameObject.transform.position;
        dashPfxInstance.transform.rotation = gameObject.transform.rotation;
        dashPfxInstance.SetActive(true);*/

        Instantiate(DashPFX, gameObject.transform.position, gameObject.transform.rotation);

        yield return new WaitForSeconds(boostCooldown);
        boostReady = true;
    }

    public float GetMaxSpeed()
    {
        if (currentWeapon.GetCurrentWeaponSettings().EnableSpeedModifier)
        {
            if (isAbducting)
            {
                return Mathf.Min(currentWeapon.GetCurrentWeaponSettings().SpeedModifier, abductMaxSpeed);
            }
            else
            {
                return currentWeapon.GetCurrentWeaponSettings().SpeedModifier;
            }
        }
        else if (isAbducting)
        {
            return abductMaxSpeed;
        }
        else
        {
            return maxSpeed;
        }
    }
    public bool isConsole = false, isPC = false;
    void Update()
    {
        //if (isPC)
        {
            if (LobbyConnectionHandler.instance.IsMultiplayerMode && pv.IsMine)
            {
                ProcessInput_PC();

                avgScaleOutput.CalculateAndOutput();

                if (Vector3.Distance(transform.localScale, (originalScale + offsetScale) + Vector3.one * scaleDelta) > 0.01f)
                {
                    transform.localScale = Vector3.MoveTowards(transform.localScale, (originalScale + offsetScale) + Vector3.one * scaleDelta, Time.deltaTime * scaleSpeed);
                }
            }
            else if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
            {
                if(allowLocalProcessInput) ProcessInput();

                avgScaleOutput.CalculateAndOutput();

                if (Vector3.Distance(transform.localScale, (originalScale + offsetScale) + Vector3.one * scaleDelta) > 0.01f)
                {
                    transform.localScale = Vector3.MoveTowards(transform.localScale, (originalScale + offsetScale) + Vector3.one * scaleDelta, Time.deltaTime * scaleSpeed);
                }
            }
        }
        //else if(isConsole)
        //{
        //    if (LobbyConnectionHandler.instance.IsMultiplayerMode && pv.IsMine)
        //    {
        //        ProcessInput();
        //        avgScaleOutput.CalculateAndOutput();

        //        if (Vector3.Distance(transform.localScale, originalScale + Vector3.one * scaleDelta) > 0.01f)
        //        {
        //            transform.localScale = Vector3.MoveTowards(transform.localScale, originalScale + Vector3.one * scaleDelta, Time.deltaTime * scaleSpeed);
        //        }
        //    }
        //    else if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        //    {
        //        ProcessInput();
        //        avgScaleOutput.CalculateAndOutput();

        //        if (Vector3.Distance(transform.localScale, originalScale + Vector3.one * scaleDelta) > 0.01f)
        //        {
        //            transform.localScale = Vector3.MoveTowards(transform.localScale, originalScale + Vector3.one * scaleDelta, Time.deltaTime * scaleSpeed);
        //        }
        //    }
        //}

        if(!isBoosting && !boostReady)
        {
            boostCounter += Time.deltaTime;
            boostCounter = Mathf.Clamp(boostCounter, 0.0f, boostCooldown);
            LevelUIManager.Instance.ChangeDashMeter(player, (boostCounter * 100.0f) / boostCooldown);
        }

        if(boostReady)
            LevelUIManager.Instance.ChangeDashMeter(player, 100.0f);
    }

    bool wasMoving;
    void FixedUpdate()
    {
#if UNITY_EDITOR
        // TESTING ONLY
        if (PlayerBot.active && !pawn)
        {
            if (PlayerBot.chosenPlayer.Contains(player))
            {
                if (Input.GetKeyDown(KeyCode.L))
                {
                    dead = true;

                    avgScaleOutput.RemovePlayer(this.gameObject.transform);

                    PlayerManager.Instance.PlayerDied(player, playerModel.transform);

                    Instantiate(DeathPFX, gameObject.transform.position, gameObject.transform.rotation);
                    Destroy(this.gameObject);
                }
            }
        }
#endif

        if (!isBoosting)
        {
            //if (isConsole)
            //{
            //    moveDirection = Vector2.Lerp(moveDirection, new Vector2(horizontalInput, verticalInput), acceleration * Time.fixedDeltaTime);
            //}
            //else if(isPC)
            //{
            //    moveDirection = Vector2.Lerp(moveDirection, new Vector2(horizontalInputKB, verticalInputKB), acceleration * Time.fixedDeltaTime);
            //}

            moveDirection = Vector2.Lerp(moveDirection, new Vector2(horizontalInput, verticalInput), acceleration * Time.fixedDeltaTime);

            //if (Vector3.Project(myRigidbody.velocity, moveInputVector).magnitude < maxSpeed)
            {
                //if ((isPC && (horizontalInputKB != 0 || verticalInputKB != 0)) || (isConsole && (horizontalInput != 0 || verticalInput != 0)))
                if (horizontalInput != 0 || verticalInput != 0)
                    myRigidbody.velocity = (transform.position + new Vector3(moveDirection.x, 0.0f, moveDirection.y) / 2.0f * GetMaxSpeed()) - transform.position;
                else
                    myRigidbody.velocity *= 0.989f;
            }
        }
    }

    public void ApplyExternalInput(Vector3 axis, Quaternion lookAt)
    {
        isConsole = true;
        isPC = false;
        twinStick = false;

        horizontalInput = axis.x;
        verticalInput = axis.z;

        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, Time.smoothDeltaTime * 6.0f);
    }
    public void ChangeScale(float scaleChange)
    {
        if (!canGrowWhenUsingSpecial) return;

        bool isntInvincible = healthManager != null ? !healthManager.IsInvincible() : true;

        if (isntInvincible || scaleChange > 0f)
        {

            scaleDelta += scaleChange;
            scaleDelta = Mathf.Clamp(scaleDelta, minScaleDelta, maxScaleDelta);
            //myRigidbody.mass = originalMass + scaleDelta * massMultiplier;

        }

    }

    public void Die()
    {
        if (dead || pawn) return;

        dead = true;

        //avgScaleOutput.RemovePlayer(this.gameObject.transform);
        //PlayerManager.Instance.PlayerDied(player, playerModel.transform);

        Debug.LogWarning("Die");
        if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            pv.RPC("Death_RPC", RpcTarget.AllBuffered, PlayerManager.Instance.players[player].lives);
        }

        else
        {
            dead = true;

            avgScaleOutput.RemovePlayer(this.gameObject.transform);
            if(!pawn) PlayerManager.Instance.PlayerDied(player, playerModel.transform);
            Instantiate(DeathPFX, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(this.gameObject);
        }
            

       /* GameObject deathPfxInstance = PlayerManager.Instance.deathPfxCache.GetInstance();
        deathPfxInstance.transform.position = gameObject.transform.position;
        deathPfxInstance.transform.rotation = gameObject.transform.rotation;
        deathPfxInstance.SetActive(true);*/

        
    }

    public bool IsSuperWeaponReady()
    {
        return !superWeaponActive && energyMeter.fillAmount == 1.0f;
    }

    public void AddAbductedObject(GameObject abductedObject, float scaleAdd,
                                  float energyAdd)
    {
        // remove abducted object from game
        abductedObject.transform.position = Vector2.one * 1000f;
        abductedObject.SetActive(false);
        abductedObjects.Push(abductedObject);
        ChangeScale(scaleAdd);

        if (energyMeter.fillAmount < 1.0f)
        {
            energyMeter.fillAmount += energyAdd;
        }
        if (energyMeter.fillAmount == 1.0f)
        {
            specialReady.SetActive(true);
        }
    }

    public void DropAbductedObject(int count)
    {
        GameObject obj;
        for (int i = 0; i < count; i++)
        {
            if (abductedObjects.Count > 0)
            {
                obj = abductedObjects.Pop();

                Rigidbody rb = obj.GetComponent<Rigidbody>();
                Collider col = obj.GetComponent<Collider>();
                if (col != null)
                    col.isTrigger = false;

                if (rb != null) rb.isKinematic = false;

                obj.transform.position = transform.position;
                obj.SetActive(true);
            }
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (GameManager.Instance.paused || GameManager.Instance.HasCutsceneObjectsActive) return;

        if (collidersToIgnore != null)
        {
            if (Array.FindIndex(collidersToIgnore, it => it.theCollider == other.collider) >= 0)
            {
                return;
            }
        }

        //Debug.Log(gameObject.name + ": The gameobject I Hit was " + other.gameObject.name + " - " + other.collider.tag);
        if (other.collider.CompareTag("Bullet"))
        {
            float healthDamage = 0.0f;
            Bullet b;
            if (other.gameObject.TryGetComponent(out b))
                healthDamage = b.HealthDamage;

            if(b == null)
            {
                MeleeBullet mb;
                if (other.gameObject.TryGetComponent(out mb))
                    healthDamage = mb.HealthDamage;
            }
			
			if(healthDamage != 0.0f)
				DoDamage(healthDamage);

            //ChangeHealth (other.gameObject.GetComponent<Bullet>().healthDamage);
            //ChangeScale(other.gameObject.GetComponent<Bullet>().scaleDamage);

            //ChangeScale(defaultScaleDamage);
            // if (scaleCount > 0)
            //     scaleToMinusInterval = 1;
        }

        if (other.collider.CompareTag("BulletFist"))
        {
            //ChangeHealth (other.gameObject.GetComponent<Bullet>().healthDamage);
            //ChangeScale(other.gameObject.GetComponent<Bullet>().scaleDamage);

           // myRigidbody.AddExplosionForce(140.0f, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), 100.0f);

           // healthManager.ChangeHealth(other.gameObject.GetComponent<Bullet>().HealthDamage);
            //ChangeScale(defaultScaleDamage);
            DropAbductedObject(1);
            // if (scaleCount > 0)
            //     scaleToMinusInterval = 1;
        }

    }

    public void DoDamage(float healthDamage = -3.0f)
    {
        if(!pawn) healthManager.ChangeHealth(healthDamage);

        OnTakeDamage?.Invoke(healthDamage);

        DropAbductedObject(1);
    }

    public void ApplyForce(Vector3 point)
    {
        myRigidbody.AddForce((point + transform.position).normalized * 60.0f, ForceMode.Impulse);
    }

    public float GetScaleDelta()
    {
        return scaleDelta;
    }

}