using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using RotaryHeart.Lib.SerializableDictionary;
using static Weapon;
using System;
using static InputManager;
using Photon.Pun;
public class PlayerController : MonoBehaviour
{
    public static Dictionary<GameObject, PlayerController> playerControllerByGameObject = new Dictionary<GameObject, PlayerController>();

    public Player player;
    public GameObject PlayerSpecialvCam;
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

    [SerializeField]
    private NormalWeapon normalWeapon;

    [SerializeField]
    private SuperWeapon superWeapon;

    private Weapon currentWeapon;

    private bool isAbducting = false;

    private bool superWeaponActive = false;

    bool dead = false;

    private Vector2 rightStickDirection;

    public GameObject specialReady;

    [SerializeField]
    private Transform modelContainer;

    GameObject playerModel;
    PhotonView pv;
    private void Awake()
    {
        pv = this.GetComponent<PhotonView>();
        playerControllerByGameObject.Add(gameObject, this);
    }

    IEnumerator Start()
    {
        currentWeapon = normalWeapon;
        avgScaleOutput = GameObject.FindGameObjectWithTag("AverageScaleOutput").GetComponent<AverageScaleOutput>();
        avgScaleOutput.AddPlayer(this.gameObject.transform);
        healthManager.SetInvincible(true);
        myRigidbody = GetComponent<Rigidbody>();
        beam.SetActive(false);
        originalScale = transform.localScale;
        originalMass = myRigidbody.mass;
        powerMin = 0.0f;

        playerModel = Instantiate(GameManager.Instance.GetPlayerModel(player), modelContainer);
        playerModel.transform.localRotation = Quaternion.identity;
        playerModel.transform.localPosition = Vector3.zero;

        normalWeapon.ChangeWeapon(GameManager.Instance.GetCharacterNormalWeapon(GameManager.Instance.GetPlayerCharacterChoice(player)));
        superWeapon.ChangeWeapon(GameManager.Instance.GetCharacterSuperWeapon(GameManager.Instance.GetPlayerCharacterChoice(player)));
        yield return new WaitForSeconds(invincibleDuration);
        healthManager.SetInvincible(false);
    }

    private void OnDestroy()
    {
        playerControllerByGameObject.Remove(gameObject);
    }

    private void ActivateBeam()
    {
        beam.SetActive(true);
        isAbducting = true;
    }

    private void DeactivateBeam()
    {
        beam.SetActive(false);
        isAbducting = false;
    }

    Vector2 GetInputAxis()
    {
        return GameManager.Instance.paused ? Vector2.zero :
            new Vector2(
                InputManager.Instance.GetAxis(AxisEnum.LeftStickHorizontal, player),
                InputManager.Instance.GetAxis(AxisEnum.LeftStickVertical, player)
            );
    }

    private void ProcessInput_PC()
    {
        horizontalInput = Input.GetAxis("P1_Horizontal_Axis_Keyboard");
        verticalInput = Input.GetAxis("P1_Vertical_Axis_Keyboard");
        moveInputVector = new Vector3(horizontalInput, 0.0f, verticalInput);
        if (horizontalInput != 0f || verticalInput != 0f)
        {
            //this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, Mathf.Atan2(horizontalInput, verticalInput) * 180 / Mathf.PI, this.transform.localEulerAngles.z);
        }

        if (Input.GetButtonDown("P1_Beam_Keyboard") && energyMeter.fillAmount != 1f)
        {
            Debug.Log("Beam Input");
            
            pv.RPC("RPC_Beam", RpcTarget.All);
        }
        else if (Input.GetButtonUp("P1_Beam_Keyboard"))
        {
            pv.RPC("RPC_Beam_Off", RpcTarget.All);

        }
        if (twinStick)
        {
            rightStickDirection = new Vector2(Input.GetAxis("P1_Rotation_Hor_Keyboard"), Input.GetAxis("P1_Rotation_Ver_Keyboard"));
            if (rightStickDirection.magnitude > 0.1)//rightStickDirection != Vector2.zero)
            {
                //this.transform.localEulerAngles = new Vector3(0f, Mathf.Atan2(rightStickDirection.x, rightStickDirection.y) * 180 / Mathf.PI, 0f);
                Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

                ////Get the Screen position of the mouse
                Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
                //Debug.Log(mouseOnScreen);
                ////Get the angle between the points
                float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

                ////Ta Daaa
                transform.rotation = Quaternion.Euler(new Vector3(0f, -angle , 0f));

                //Vector3 _direction = (transform.position - new Vector3(mouseOnScreen.x, mouseOnScreen.y, this.transform.position.z)).normalized;

                ////create the rotation we need to be in to look at the target
                //Quaternion _lookRotation = Quaternion.LookRotation(_direction);

                ////rotate us over time according to speed until we are in the required rotation
                //transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 1000);

            }
        }
        currentWeapon.UpdateShootDirection(transform.forward);
        if (/*currentWeapon.GetCurrentWeaponSettings().AutoFire && */Input.GetButtonDown("P1_Fire1_Keyboard") /*&& gunReady && !Input.GetButtonDown("P1_Beam_Keyboard")*/)
        {
            
            pv.RPC("RPC_Fire", RpcTarget.All, transform.forward);
        }
        if (Input.GetButtonDown("P1_Dash_Keyboard") && boostReady)
        {
            tryToBoost();
        }

        if (IsSuperWeaponReady() && Input.GetAxis("P1_SuperWeapon1_Keyboard") > 0.5f /*&&*/ || Input.GetAxis("P1_SuperWeapon2_Keyboard") > 0.5f)
        {
            pv.RPC("RPC_ToggleSpecialWeapon", RpcTarget.All);
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
    void RPC_ToggleSpecialWeapon()
    {
        ToggleSuperWeapon(true);
    }

    [PunRPC]
    void RPC_Fire(Vector3 fireDirection)
    {
        
        if (pv.IsMine)
        {
            currentWeapon.Fire();
        }
        else
        {
            currentWeapon.Fire_OtherInstances(fireDirection);
        }
        Debug.Log("Fire1");
    }

    private void ProcessInput()
    {
        Vector2 axis = GetInputAxis();

        horizontalInput = axis.x;
        verticalInput = axis.y;

        moveInputVector = new Vector3(horizontalInput, 0.0f, verticalInput);

        if (GameManager.Instance.paused) return;

        if (horizontalInput != 0f || verticalInput != 0f)
        {
            this.transform.localEulerAngles = new Vector3(0f, Mathf.Atan2(horizontalInput, verticalInput) * 180 / Mathf.PI, 0f);
        }


        if (InputManager.Instance.GetButtonDown(ButtonEnum.Beam, player) && energyMeter.fillAmount != 1f)
        {
            Debug.Log("Beam Input");
            ActivateBeam();
        }
        else if (InputManager.Instance.GetButtonUp(ButtonEnum.Beam, player))
        {
            DeactivateBeam();

           /* GameObject beamInstance = PlayerManager.Instance.beamOffCache.GetInstance();
            beamInstance.transform.position = gameObject.transform.position;
            beamInstance.transform.rotation = gameObject.transform.rotation;
            beamInstance.SetActive(true);*/
        }
        if (twinStick)
        {
            rightStickDirection = new Vector2(InputManager.Instance.GetAxis(AxisEnum.RightStickHorizontal, player), InputManager.Instance.GetAxis(AxisEnum.RightStickVertical, player));
            if (rightStickDirection != Vector2.zero)
            {
                this.transform.localEulerAngles = new Vector3(0f, Mathf.Atan2(rightStickDirection.x, rightStickDirection.y) * 180 / Mathf.PI, 0f);


            }
        }
        currentWeapon.UpdateShootDirection(transform.forward);
        if (((currentWeapon.GetCurrentWeaponSettings().AutoFire && InputManager.Instance.GetButton(ButtonEnum.Fire, player)) || InputManager.Instance.GetButtonDown(ButtonEnum.Fire, player)) && gunReady && !InputManager.Instance.GetButton(ButtonEnum.Beam, player))
        {

            currentWeapon.Fire();
        }
        if (InputManager.Instance.GetButtonDown(ButtonEnum.Dash, player) && boostReady)
        {
            tryToBoost();
        }

        if (IsSuperWeaponReady() && InputManager.Instance.GetAxis(AxisEnum.ActivateSuperWeapon1, player) > 0.5f && InputManager.Instance.GetAxis(AxisEnum.ActivateSuperWeapon2, player) > 0.5f)
        {
            ToggleSuperWeapon(true);
        }

    }

    IEnumerator DeactivateSpecialCamera()
    {
        yield return new WaitForSeconds(2.5f);
        PlayerSpecialvCam.SetActive(false);
    }

    public void ToggleSuperWeapon(bool activate)
    {
        superWeaponActive = activate;
        if (activate)
        {
            //StartCoroutine(CameraController.Instance.ActivateSpecialCamera(1f));
           // if(pv.IsMine)
                PlayerSpecialvCam.SetActive(true);
            normalWeapon.gameObject.SetActive(false);
            superWeapon.gameObject.SetActive(true);
            superWeapon.ActivateWeapon();
            currentWeapon = superWeapon;
            StartCoroutine(DeactivateSpecialCamera());
        }
        else
        {
            PlayerSpecialvCam.SetActive(false);
            normalWeapon.gameObject.SetActive(true);
            superWeapon.gameObject.SetActive(false);
            currentWeapon = normalWeapon;
            energyMeter.fillAmount = 0f;
            scaleDelta = 0f;
        }
    }


    private void tryToBoost()
    {
        if (horizontalInput != 0f || verticalInput != 0f)
        {
            StartCoroutine(BoostCoroutine());
        }
    }

    private IEnumerator BoostCoroutine()
    {
        boostReady = false;
        isBoosting = true;
        Vector3 boostDirection = new Vector3(horizontalInput, 0.0f, verticalInput);
        float timer = 0f;
        myRigidbody.velocity = Vector3.zero;
        while (timer <= boostDuration)
        {
            myRigidbody.MovePosition(boostDirection.normalized * boostSpeed * Time.fixedDeltaTime + transform.position);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity.normalized, GetMaxSpeed());
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

    void Update()
    {
        if (LobbyConnectionHandler.instance.IsMultiplayerMode && pv.IsMine)
        {
            ProcessInput_PC();
            avgScaleOutput.CalculateAndOutput();

            if (Vector3.Distance(transform.localScale, originalScale + Vector3.one * scaleDelta) > 0.01f)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, originalScale + Vector3.one * scaleDelta, Time.deltaTime * scaleSpeed);
            }
        }
        else if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            ProcessInput_PC();
            avgScaleOutput.CalculateAndOutput();

            if (Vector3.Distance(transform.localScale, originalScale + Vector3.one * scaleDelta) > 0.01f)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, originalScale + Vector3.one * scaleDelta, Time.deltaTime * scaleSpeed);
            }
        }

    }

    void FixedUpdate()
    {
        if (!isBoosting)
        {
            moveDirection = Vector2.Lerp(moveDirection, new Vector2(horizontalInput, verticalInput), acceleration * Time.fixedDeltaTime);
            //if (Vector3.Project(myRigidbody.velocity, moveInputVector).magnitude < maxSpeed)
            {
                myRigidbody.MovePosition(new Vector3(moveDirection.x, 0.0f, moveDirection.y) / 2.0f * GetMaxSpeed() * Time.fixedDeltaTime + transform.position);
            }
        }
    }



    public void ChangeScale(float scaleChange)
    {
        if (!healthManager.IsInvincible() || scaleChange > 0f)
        {

            scaleDelta += scaleChange;
            scaleDelta = Mathf.Clamp(scaleDelta, minScaleDelta, maxScaleDelta);
            //myRigidbody.mass = originalMass + scaleDelta * massMultiplier;

        }

    }

    public void Die()
    {
        if (dead) return;

        dead = true;

        avgScaleOutput.RemovePlayer(this.gameObject.transform);
        PlayerManager.Instance.PlayerDied(player, playerModel.transform);

        Debug.LogWarning("Die");
        Destroy(this.gameObject);

       /* GameObject deathPfxInstance = PlayerManager.Instance.deathPfxCache.GetInstance();
        deathPfxInstance.transform.position = gameObject.transform.position;
        deathPfxInstance.transform.rotation = gameObject.transform.rotation;
        deathPfxInstance.SetActive(true);*/

        Instantiate(DeathPFX, gameObject.transform.position, gameObject.transform.rotation);
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
                obj.transform.position = transform.position;
                obj.SetActive(true);
            }
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Bullet"))
        {
            DoDamage(other.gameObject.GetComponent<Bullet>().HealthDamage);

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
        healthManager.ChangeHealth(healthDamage);
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