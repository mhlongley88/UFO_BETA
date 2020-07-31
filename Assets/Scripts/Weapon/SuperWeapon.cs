
using UnityEngine;
using System;
using RotaryHeart.Lib.SerializableDictionary;
using Random = UnityEngine.Random;
using System.Collections;
using Photon.Pun;
public class SuperWeapon : Weapon
{


    [Serializable]
    public enum SuperWeaponTypes
    {
        AcidLauncher,
        Canon,
        Dual,
        FlameThrower,
        MiniGun,
        DualShotgun,
        Mallet,
        BlobLauncher
    }


    public Mesh weaponPlaceHolderMesh;

    public GameObject specialCanon;
    public GameObject specialAcidThrower;
    public GameObject specialDual;
    public GameObject specialMiniGun;

    public GameObject specialReady;

    [SerializeField]
    private PlayerController myPlayerController;

    private float timer = 0f;

    public SuperWeaponTypes currentWeapon = SuperWeaponTypes.AcidLauncher;

    [Serializable]
    public class SuperWeaponDict : SerializableDictionaryBase<SuperWeaponTypes, WeaponAttributes> { }

    public SuperWeaponDict superWeaponMapping;

    [Serializable]
    public class WeaponAttributes
    {
        public ScriptableWeapon weaponSettings;
        public GameObject weaponModel;
    }

    public GameObject PlayerObj;
    public PhotonView pv;

    void Start()
    {
        if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            pv = PlayerObj.GetComponentInParent<Photon.Pun.PhotonView>();
        }

      //  currentAmmo = GetCurrentWeaponSetting().MaxAmmo;

    }

    public void ActivateWeapon()
    {
        
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            canFire = true;
            currentAmmo = GetCurrentWeaponSetting().MaxAmmo;
            if (GetCurrentWeaponSetting().Timed)
            {
                StartCoroutine(WeaponDurationTimer());
            }
            if (GetCurrentWeaponSetting().AutoFire)
            {
                StartCoroutine(AutoFire());
            }
            superWeaponMapping[currentWeapon].weaponModel.SetActive(true);
        }
        else
        {
            ActivateWeaponMul();
        }
    }

    public void ActivateWeaponMul()
    {
      //  pv = PlayerObj.GetComponent<Photon.Pun.PhotonView>();
        if (!pv.IsMine)
        {
            //pv = PlayerObj.GetComponent<Photon.Pun.PhotonView>();
            canFire = true;
            currentAmmo = GetCurrentWeaponSetting().MaxAmmo;
            if (GetCurrentWeaponSetting().Timed)
            {
                StartCoroutine(WeaponDurationTimer());
            }
            //if (GetCurrentWeaponSetting().AutoFire)
            //{
            //    StartCoroutine(AutoFire());
            //}
            superWeaponMapping[currentWeapon].weaponModel.SetActive(true);
        }
        else
        {
           // pv = PlayerObj.GetComponent<Photon.Pun.PhotonView>();
            canFire = true;
            currentAmmo = GetCurrentWeaponSetting().MaxAmmo;
            if (GetCurrentWeaponSetting().Timed)
            {
                StartCoroutine(WeaponDurationTimer());
            }
            if (GetCurrentWeaponSetting().AutoFire)
            {
                StartCoroutine(AutoFire());
            }
            superWeaponMapping[currentWeapon].weaponModel.SetActive(true);
        }
    }

    public void ChangeWeapon(SuperWeaponTypes weaponType)
    {
        currentWeapon = weaponType;
        currentAmmo = GetCurrentWeaponSetting().MaxAmmo;
    }

    private IEnumerator WeaponDurationTimer()
    {
        yield return new WaitForSeconds(GetCurrentWeaponSetting().WeaponDuration);
        // timer = 0f;
        // while (timer < GetCurrentWeaponSetting().WeaponDuration)
        // {
        //     timer += Time.unscaledDeltaTime;
        //     yield return new WaitForEndOfFrame();
        // }
        DeactivateWeapon();
    }

    public void DeactivateWeapon()
    {
        if(myPlayerController != null) myPlayerController.ToggleSuperWeapon(false);
        
        superWeaponMapping[currentWeapon].weaponModel.SetActive(false);

        specialReady.SetActive(false);
        
    }

    //public void RPC_DeactivateWeapon()
    //{
    //    myPlayerController.ToggleSuperWeapon(false);
    //    superWeaponMapping[currentWeapon].weaponModel.SetActive(false);

    //    specialReady.SetActive(false);

    //}

    public IEnumerator AutoFire()
    {
        canFire = false;
        while ((GetCurrentWeaponSetting().InfiniteAmmo || currentAmmo > 0))
        {
            Bullet b;
            float shootAngle;
            for (int i = 0; i < GetCurrentWeaponSetting().ShotsPerVolley; i++)
            {
                shootAngle = Random.Range(-GetCurrentWeaponSetting().Spread / 2.0f, GetCurrentWeaponSetting().Spread / 2.0f);
                if (GetCurrentWeaponSetting().WeaponFiringPositionOffsets.Length > 0)
                {
                    if (firePositionIndex >= GetCurrentWeaponSetting().WeaponFiringPositionOffsets.Length)
                    {
                        firePositionIndex = 0;
                    }

                    var bulletSpawnPoint = transform.TransformPoint(GetCurrentWeaponSetting().WeaponFiringPositionOffsets[firePositionIndex]);
                    SpawnMuzzleFlash(bulletSpawnPoint);
                    b = Instantiate(GetCurrentWeaponSetting().BulletPrefab, bulletSpawnPoint, Quaternion.identity).GetComponent<Bullet>();
                    firePositionIndex++;
                }
                else
                {
                    var bulletSpawnPoint = transform.position;
                    SpawnMuzzleFlash(bulletSpawnPoint);
                    b = Instantiate(GetCurrentWeaponSetting().BulletPrefab, bulletSpawnPoint, Quaternion.identity).GetComponent<Bullet>();
                }
                b.FireBullet(Quaternion.AngleAxis(shootAngle, Vector3.up) * shootDirection, ufoCollider, GetCurrentWeaponSetting().HealthDamage, GetCurrentWeaponSetting().ScaleDamage, GetCurrentWeaponSetting().BulletVelocity);
            }
            currentAmmo--;
            //StartCoroutine(AmmoCooldownCoroutine());
            ufoRigidbody.AddForce(-shootDirection.normalized * GetCurrentWeaponSetting().RecoilForce, ForceMode.Impulse);
            if(pv != null)
                pv.RPC("RPC_Fire_Others", RpcTarget.Others, transform.forward);
            yield return new WaitForSeconds(GetCurrentWeaponSetting().FireRate);

        }
        DeactivateWeapon();
        canFire = true;
    }
    //public override void Fire()
    //{
    //    if (LobbyConnectionHandler.instance.IsMultiplayerMode)
    //        pv.RPC("Fire1", RpcTarget.All);
    //    else
    //        Fire1();
    //}
    //[PunRPC]

    public override void Fire_OtherInstances(Vector3 fireDirection)
    {
        if (currentAmmo > 0 && canFire)
        {
            float shootAngle = Random.Range(-GetCurrentWeaponSetting().Spread / 2.0f, GetCurrentWeaponSetting().Spread / 2.0f);
            Bullet b;
            b = Instantiate(GetCurrentWeaponSetting().BulletPrefab, transform.position + GetCurrentWeaponSetting().WeaponFiringPositionOffsets[firePositionIndex], Quaternion.identity).GetComponent<Bullet>();
            b.FireBullet(Quaternion.AngleAxis(shootAngle, Vector3.up) * fireDirection, ufoCollider, GetCurrentWeaponSetting().HealthDamage + healthDamageOffset, GetCurrentWeaponSetting().ScaleDamage, GetCurrentWeaponSetting().BulletVelocity);

        }
    }
    //PhotonView pv;
    public override void Fire()
    {
        if (currentAmmo > 0 && canFire)
        {
            Bullet b;
            float shootAngle;
            for (int i = 0; i < GetCurrentWeaponSetting().ShotsPerVolley; i++)
            {
                shootAngle = Random.Range(-GetCurrentWeaponSetting().Spread / 2.0f, GetCurrentWeaponSetting().Spread / 2.0f);
                if (GetCurrentWeaponSetting().WeaponFiringPositionOffsets.Length > 0)
                {
                    if (firePositionIndex >= GetCurrentWeaponSetting().WeaponFiringPositionOffsets.Length)
                    {
                        firePositionIndex = 0;
                    }

                    var bulletSpawnPoint = transform.TransformPoint(GetCurrentWeaponSetting().WeaponFiringPositionOffsets[firePositionIndex]);
                    SpawnMuzzleFlash(bulletSpawnPoint);
                    b = Instantiate(GetCurrentWeaponSetting().BulletPrefab, bulletSpawnPoint, Quaternion.identity).GetComponent<Bullet>();
                    firePositionIndex++;
                }
                else
                {
                    var bulletSpawnPoint = transform.position;
                    SpawnMuzzleFlash(bulletSpawnPoint);
                    b = Instantiate(GetCurrentWeaponSetting().BulletPrefab, bulletSpawnPoint, Quaternion.identity).GetComponent<Bullet>();
                }
                b.FireBullet(Quaternion.AngleAxis(shootAngle, Vector3.up) * shootDirection, ufoCollider, GetCurrentWeaponSetting().HealthDamage + healthDamageOffset, GetCurrentWeaponSetting().ScaleDamage, GetCurrentWeaponSetting().BulletVelocity);
            }
            currentAmmo--;
            if (GetCurrentWeaponSetting().InfiniteAmmo)
            {
                StartCoroutine(AmmoCooldownCoroutine());
            }
            StartCoroutine(WeaponCooldownCoroutine());
            ufoRigidbody.AddForce(-shootDirection.normalized * GetCurrentWeaponSetting().RecoilForce, ForceMode.Impulse);
            if (!GetCurrentWeaponSetting().InfiniteAmmo && !GetCurrentWeaponSetting().Reloadable && currentAmmo <= 0)
            {
                DeactivateWeapon();
            }

            if(pv != null)
                pv.RPC("RPC_Fire_Others", RpcTarget.Others, transform.forward);
        }
    }



    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (GetCurrentWeaponSetting().WeaponFiringPositionOffsets.Length > 0)
        {
            foreach (Vector3 pos in GetCurrentWeaponSetting().WeaponFiringPositionOffsets)
            {
                if (weaponPlaceHolderMesh != null)
                {
                    Gizmos.DrawWireMesh(weaponPlaceHolderMesh, transform.TransformPoint(pos), transform.rotation);
                }
            }
        }
        else
        {
            Gizmos.DrawWireMesh(weaponPlaceHolderMesh, transform.position, transform.rotation);

        }
    }

    public override ScriptableWeapon GetCurrentWeaponSetting()
    {
        return superWeaponMapping[currentWeapon].weaponSettings;
    }
}