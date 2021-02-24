using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using Random = UnityEngine.Random;
public abstract class Weapon : MonoBehaviour
{



    [SerializeField]
    protected Collider ufoCollider;

    [SerializeField]
    protected Rigidbody ufoRigidbody;

    // For bots
    [HideInInspector]
    public float healthDamageOffset;

    protected Vector3 shootDirection = Vector3.forward;

    public int currentAmmo;
    public BulletSpawnPoint[] bulletSpawnPoints;
    public bool canFire = true;
    public float muzzleFlashLifetime = 2.0f;
    protected int firePositionIndex = 0;

    public abstract ScriptableWeapon GetCurrentWeaponSetting();

    GameObject muzzleFlash = null;

    public virtual void Fire_OtherInstances(Vector3 fireDirection)
    {

    }

    public void SetMuzzleFlash(GameObject muzzlePrefab)
    {
        muzzleFlash = muzzlePrefab;
    }


    protected void SpawnMuzzleFlash(Vector3 point)
    {
        if (muzzleFlash != null)
        {
            var muzzleInstance = Instantiate(muzzleFlash, point + transform.forward, muzzleFlash.transform.rotation);
        }
    }

    //Photon.Pun.PhotonView pv;
    public virtual void Fire(bool viaPress = true)
    {
        Debug.Log(currentAmmo + "-" + canFire);
        if (currentAmmo > 0 && canFire)
        {
            Bullet b;
            float shootAngle;
            for (int i = 0; i < GetCurrentWeaponSetting().ShotsPerVolley; i++)
            {
                shootAngle = Random.Range(-GetCurrentWeaponSetting().Spread / 2.0f, GetCurrentWeaponSetting().Spread / 2.0f);
                //if (GetCurrentWeaponSetting().WeaponFiringPositionOffsets.Length > 0)
                //{
                //    if (firePositionIndex >= GetCurrentWeaponSetting().WeaponFiringPositionOffsets.Length)
                //    {
                //        firePositionIndex = 0;
                //    }

                //    var bulletSpawnPoint = transform.TransformPoint(GetCurrentWeaponSetting().WeaponFiringPositionOffsets[firePositionIndex]);
                //    SpawnMuzzleFlash(bulletSpawnPoint);
                //    b = Instantiate(GetCurrentWeaponSetting().BulletPrefab, bulletSpawnPoint, Quaternion.identity).GetComponent<Bullet>();
                //    firePositionIndex++;
                //}
                //else
                //{
                //    var bulletSpawnPoint = transform.position;

                //    b = Instantiate(GetCurrentWeaponSetting().BulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
                //}

                if (bulletSpawnPoints.Length > 1)
                {
                    if (firePositionIndex >= bulletSpawnPoints.Length)
                    {
                        firePositionIndex = 0;
                    }

                    var bulletSpawnPoint = bulletSpawnPoints[firePositionIndex].transform.position;//transform.TransformPoint(GetCurrentWeaponSetting().WeaponFiringPositionOffsets[firePositionIndex]);
                    SpawnMuzzleFlash(bulletSpawnPoint);
                    b = Instantiate(GetCurrentWeaponSetting().BulletPrefab, bulletSpawnPoint, Quaternion.identity).GetComponent<Bullet>();
                    //b.transform.position = bulletSpawnPoint;
                    firePositionIndex++;
                }
                else
                {
                    var bulletSpawnPoint = bulletSpawnPoints[0].transform.position;

                    b = Instantiate(GetCurrentWeaponSetting().BulletPrefab, bulletSpawnPoint, Quaternion.identity).GetComponent<Bullet>();
                   // b.transform.position = bulletSpawnPoint;
                }

               // b.FireBullet(Quaternion.AngleAxis(shootAngle, Vector3.up) * shootDirection, ufoCollider, GetCurrentWeaponSetting().HealthDamage + healthDamageOffset, GetCurrentWeaponSetting().ScaleDamage, GetCurrentWeaponSetting().BulletVelocity);
            }
            currentAmmo--;
            StartCoroutine(AmmoCooldownCoroutine());
            StartCoroutine(WeaponCooldownCoroutine());
            ufoRigidbody.AddExplosionForce(GetCurrentWeaponSetting().RecoilForce, shootDirection.normalized * 1.0f + transform.position, 1f, 0f, ForceMode.Impulse);
        }
    }

    protected virtual IEnumerator WeaponCooldownCoroutine()
    {
        canFire = false;
        yield return new WaitForSeconds(GetCurrentWeaponSetting().FireRate);
        canFire = true;
    }


    protected virtual IEnumerator AmmoCooldownCoroutine()
    {
        yield return new WaitForSeconds(GetCurrentWeaponSetting().ReloadTime);
        currentAmmo = Mathf.Min(GetCurrentWeaponSetting().MaxAmmo, currentAmmo + 1);
    }


    public void UpdateShootDirection(Vector3 newDirection)
    {
        shootDirection = newDirection;
    }

    public void ReloadWeapon()
    {
        currentAmmo = GetCurrentWeaponSetting().MaxAmmo;
    }

    public virtual ScriptableWeapon GetCurrentWeaponSettings()
    {
        return GetCurrentWeaponSetting();
    }

}
