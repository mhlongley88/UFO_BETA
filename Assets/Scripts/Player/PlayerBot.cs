using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerBotSlot
{
    One = 1,
    Two = 2,
    Three = 3,
}

public class PlayerBot : MonoBehaviour
{
    PlayerController playerController;
    public static List<Player> chosenPlayer = new List<Player>();
    public static List<AIPreset> aiPresets = new List<AIPreset>();
    public static List<PlayerBotSlot> aiSlots = new List<PlayerBotSlot>();

    public static Transform adversaryTransform;
    public static bool active;
    public PlayerBotSlot slot;
    Vector3 destination;

    bool moving = true;

    float movingRate = 0.0f;
    float shootingRate = 0.0f;

    bool followingPlayer;
    
    [HideInInspector]
    public AIPreset preset;

    float abductRateElapsed = 0.0f;
    bool abductOn;
    bool usingSpecial;
    float useDashElaspsed;
    float increaseHealthRateElapsed;
    float increaseDamageRateElapsed;

    Transform targetAbduct;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();

        destination = transform.position;
        playerController.allowLocalProcessInput = false;

        // preset = BotConfigurator.instance.currentPreset;

        //    playerController.inputAbduction = false;
        abductRateElapsed = Time.time + Random.Range(6.0f, 8.0f);
    }

    private void OnDestroy()
    {
        //chosenPlayer.Remove(playerController.player);
        //GameManager.Instance.RemovePlayerFromGame(chosenPlayer);
    }

    void Update()
    {
        Quaternion lookDir = Quaternion.LookRotation(destination - transform.position, Vector3.up);

        if (GameManager.Instance.paused)
        {
            playerController.ApplyExternalInput(Vector3.zero, lookDir);
            return;
        }

        if(movingRate < Time.time)
        {
            moving = !moving;

            movingRate = Time.time + (moving ? Random.Range(2.7f, 6) : Random.Range(1.5f, 3.0f));

            if(moving)
                followingPlayer = Random.value > 0.45f;
        }

        var adversaryPlayer = GameManager.Instance.GetActivePlayers().Find(it => !chosenPlayer.Contains(it));
        var adversaryObject = PlayerManager.Instance.players[adversaryPlayer].instance;

        if (moving)
        {
            if ((destination - transform.position).magnitude < 1)
                FindNewPoint();
        }

        if (adversaryObject)
        {
            lookDir = Quaternion.LookRotation(adversaryObject.transform.position - transform.position, Vector3.up);
            var ea = lookDir.eulerAngles;
            ea.x = ea.z = 0.0f;
            lookDir.eulerAngles = ea;

            if (followingPlayer)
                destination = adversaryObject.transform.position;
        }
        
        if(preset.abduct)
        {
            if(abductOn)
            {
                destination = targetAbduct.position;
            }
        }


        playerController.ApplyExternalInput(moving ? (destination - transform.position).normalized : Vector3.zero, lookDir);

        // Shooting
        if(adversaryObject && shootingRate < Time.time)
        {
            playerController.CurrentWeapon.UpdateShootDirection(transform.forward);
            playerController.CurrentWeapon.Fire();

            shootingRate = Time.time + Random.Range(preset.shootRateMinMax.x, preset.shootRateMinMax.y);

            // A little variation
            if (Random.value > 0.75f)
                shootingRate = Time.time + Random.Range(preset.shootRateMinMax.x - 0.1f, preset.shootRateMinMax.y - 0.1f);
        }

        // Abduction
        if (preset.abduct)
        {
            if (!abductOn)
            {
                if (abductRateElapsed < Time.time)
                {
                    if (ObjectAbduct.AbductableObjects.Count > 0)
                    {
                        for (int i = 0; i < ObjectAbduct.AbductableObjects.Count; i++)
                        {
                            var abductable = ObjectAbduct.AbductableObjects[i];

                            var p = abductable.transform.position;
                            p.y = transform.position.y;

                            if ((p - transform.position).magnitude < 6.0f)
                            {
                                abductOn = true;
                                targetAbduct = abductable.transform;
                                playerController.ActivateBeam();

                                break;
                            }
                        }
                    }

                    abductRateElapsed = Time.time + Random.Range(1.0f, 4.0f);
                }
            }
            else
            {
                if (abductRateElapsed < Time.time)
                {
                    abductOn = false;
                    targetAbduct = null;
                    playerController.DeactivateBeam();

                    abductRateElapsed = Time.time + Random.Range(6.0f, 8.0f);
                }
            }
            //playerController.inputAbduction = abductOn;
        }

        //Specials
        if(preset.useSpecials && !usingSpecial)
        {
            if (playerController.IsSuperWeaponReady())
            {
                usingSpecial = true;
                StartCoroutine(UseSpecial());
            }
        }

        if(preset.useDash)
        {
            if((adversaryObject.transform.position - transform.position).magnitude < 4)
            {
                if(useDashElaspsed < Time.time)
                {
                    playerController.tryToBoost();
                    useDashElaspsed = Time.time + Random.Range(0.8f, 2.0f);
                }
            }
        }

        if(preset.increaseHealth)
        {
            if (increaseHealthRateElapsed < Time.time)
            {
                playerController.healthManager.ChangeHealth(preset.amountHealthToIncrease);
    
                increaseHealthRateElapsed = Time.time + preset.increaseHealthRate;
            }
        }


        if (preset.increaseDamage)
        {
            if (increaseDamageRateElapsed < Time.time)
            {
                playerController.CurrentWeapon.healthDamageOffset += preset.amountDamageToIncrease;

                increaseDamageRateElapsed = Time.time + preset.increaseDamageRate;
            }
        }
    }

    IEnumerator UseSpecial()
    {
        yield return new WaitForSeconds(1.0f);

        playerController.ToggleSuperWeapon(true);
        usingSpecial = false;
    }

    Vector3[] dirs = new Vector3[4] { Vector3.right, Vector3.left, Vector3.forward, Vector3.back };
    void FindNewPoint()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, dirs[Random.Range(0, 4)], out hit, 10000.0f, GameManager.Instance.boundaryMask))
        {
            destination = (hit.point + hit.normal) * Random.Range(0.6f, 1.0f);
        }
    }

}
