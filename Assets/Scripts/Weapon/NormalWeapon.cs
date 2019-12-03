using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using Random = UnityEngine.Random;
using Photon.Pun;
public class NormalWeapon : Weapon
{

    [Serializable]
    public enum NormalWeaponTypes
    {
        Shotgun = 0,
        DualPistols = 1,
        SemiAuto = 2,
        MachineGun = 3,
        LaserSpear = 4,
        Canon = 5,
        Launcher = 6
    }



    [Serializable]
    protected class WeaponParamDic : SerializableDictionaryBase<NormalWeaponTypes, ScriptableWeapon> { }


    [SerializeField]
    protected WeaponParamDic weaponDic;

    public Mesh weaponPlaceHolderMesh;




    [SerializeField]
    protected NormalWeaponTypes currentWeapon = NormalWeaponTypes.DualPistols;

    public GameObject PlayerObj;
    Photon.Pun.PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            pv = PlayerObj.GetComponentInParent<Photon.Pun.PhotonView>();
        }
        
        currentAmmo = GetCurrentWeaponSetting().MaxAmmo;

    }

   

    public void ChangeWeapon(NormalWeaponTypes weaponType)
    {
        currentWeapon = weaponType;
        currentAmmo = GetCurrentWeaponSetting().MaxAmmo;
    }



    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (GetCurrentWeaponSetting().WeaponFiringPositionOffsets.Length > 0)
        {
            foreach (Vector3 pos in GetCurrentWeaponSetting().WeaponFiringPositionOffsets)
            {
                if (weaponPlaceHolderMesh != null)
                {
                    Gizmos.DrawWireMesh(weaponPlaceHolderMesh, transform.TransformPoint(pos), transform.rotation);
                    
                    //Gizmos.DrawWireMesh(weaponPlaceHolderMesh, transform.TransformPoint(pos), transform.rotation, transform.localScale);

                    // Gizmos.DrawWireMesh(weaponPlaceHolderMesh, ufoRigidbody.transform.TransformPoint(pos), transform.rotation);
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
        return weaponDic[currentWeapon];
    }

    //public override void Fire()
    //{
    //    base.Fire();
    //    pv.RPC("Fire1", RpcTarget.All);
    //}
    //[PunRPC]

    public override void Fire_OtherInstances(Vector3 fireDirection)
    {
       // if (currentAmmo > 0 && canFire)
        {
            float shootAngle = Random.Range(-GetCurrentWeaponSetting().Spread / 2.0f, GetCurrentWeaponSetting().Spread / 2.0f);
            Bullet b;
            b = Instantiate(GetCurrentWeaponSetting().BulletPrefab, transform.position + GetCurrentWeaponSetting().WeaponFiringPositionOffsets[firePositionIndex], Quaternion.identity).GetComponent<Bullet>();
            b.FireBullet(Quaternion.AngleAxis(shootAngle, Vector3.up) * fireDirection, ufoCollider, GetCurrentWeaponSetting().HealthDamage, GetCurrentWeaponSetting().ScaleDamage, GetCurrentWeaponSetting().BulletVelocity);

        }
    }



    //override PhotonView pv;
    public override void Fire()
    {
     //   Debug.Log(currentAmmo + "-" + canFire);
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

                    b = Instantiate(GetCurrentWeaponSetting().BulletPrefab, transform.position + GetCurrentWeaponSetting().WeaponFiringPositionOffsets[firePositionIndex], Quaternion.identity).GetComponent<Bullet>();
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
            pv.RPC("RPC_Fire_Others", RpcTarget.Others, transform.forward);
        }
    }
}
