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



    protected Vector3 shootDirection = Vector3.forward;

    protected int currentAmmo;

    protected bool canFire = true;

    protected int firePositionIndex = 0;

    public abstract ScriptableWeapon GetCurrentWeaponSetting();


    public virtual void Fire()
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

                    b = Instantiate(GetCurrentWeaponSetting().BulletPrefab, transform.TransformPoint(GetCurrentWeaponSetting().WeaponFiringPositionOffsets[firePositionIndex]), Quaternion.identity).GetComponent<Bullet>();
                    firePositionIndex++;
                }
                else
                {
                    b = Instantiate(GetCurrentWeaponSetting().BulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
                }
                b.FireBullet(Quaternion.AngleAxis(shootAngle, Vector3.up) * shootDirection, ufoCollider, GetCurrentWeaponSetting().HealthDamage, GetCurrentWeaponSetting().ScaleDamage, GetCurrentWeaponSetting().BulletVelocity);
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


    public virtual ScriptableWeapon GetCurrentWeaponSettings()
    {
        return GetCurrentWeaponSetting();
    }

}
