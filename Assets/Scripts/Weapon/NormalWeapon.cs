using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using Random = UnityEngine.Random;
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


    // Start is called before the first frame update
    void Start()
    {
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
}
