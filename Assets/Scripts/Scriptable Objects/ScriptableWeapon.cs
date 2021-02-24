using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSettings", menuName = "UFO/WeaponSettings", order = 0)]
public class ScriptableWeapon : ScriptableObject
{
    [SerializeField]
    private int maxAmmo;

    [SerializeField]
    private float reloadTime;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float fireRate;

    [SerializeField]
    private float healthDamage;

    [SerializeField]
    private float scaleDamage;

    [Tooltip("Can the player hold down the fire button to fire continuously")]
    [SerializeField]
    private bool continuousFire = false;

    [SerializeField]
    private float recoilForce;

    [SerializeField]
    private float bulletVelocityMultiplier = 10f;


    [SerializeField]
    private float spread = 30f;

    [SerializeField]
    private int shotsPerVolley = 1;

    [SerializeField]
    private Vector3[] weaponFiringPositionOffsets;

    [SerializeField]
    private bool enableSpeedModifier = false;

    [Tooltip("if EnableSpeedModifier is true, player's max speed will be set to this value")]
    [SerializeField]
    private float speedModifier = 1;

    [Header("Special Weapon Settings")]
    [SerializeField]
    private bool reloadable = false;



    [Tooltip("If true the super weapon will be deactivated after the number of seconds defined by weaponDuration")]
    [SerializeField]
    private bool timed = false;



    [Tooltip("Should the weapon fire without input from the player")]
    [SerializeField]
    private bool autoFire = false;

    [SerializeField]
    private bool infiniteAmmo = false;


    [SerializeField]
    private float weaponDuration = 10f;


    [SerializeField]
    private int _projCountAuto = 1;
    [SerializeField]
    private float _fireDelayAuto = 0.3f;

    public int ProjectileCount { get { return _projCountAuto; } }
    public float FireDelayAuto { get { return _fireDelayAuto; } }

    public bool Reloadable
    {
        get
        {
            return reloadable;
        }
    }

    public bool Timed
    {
        get
        {
            return timed;
        }
    }

    public bool AutoFire1
    {
        get
        {
            return autoFire;
        }
    }

    public bool InfiniteAmmo
    {
        get
        {
            return infiniteAmmo;
        }
    }

    public float WeaponDuration
    {
        get
        {
            return weaponDuration;
        }
    }
    public float ReloadTime
    {
        get
        {
            return reloadTime;
        }

    }

    public int MaxAmmo
    {
        get
        {
            return maxAmmo;
        }
    }

    public GameObject BulletPrefab
    {
        get
        {
            return bulletPrefab;
        }
    }

    public float FireRate
    {
        get
        {
            return fireRate;
        }

    }

    public float HealthDamage
    {
        get
        {
            return healthDamage;
        }

    }

    public bool AutoFire
    {
        get
        {
            return autoFire;
        }

    }

    public float RecoilForce
    {
        get
        {
            return recoilForce;
        }

    }

    public float BulletVelocity
    {
        get
        {
            return bulletVelocityMultiplier;
        }
    }

    public float ScaleDamage
    {
        get
        {
            return scaleDamage;
        }
    }

    public float Spread
    {
        get
        {
            return spread;
        }

    }

    public int ShotsPerVolley
    {
        get
        {
            return shotsPerVolley;
        }
    }

    public Vector3[] WeaponFiringPositionOffsets
    {
        get
        {
            return weaponFiringPositionOffsets;
        }
    }

    public bool EnableSpeedModifier
    {
        get
        {
            return enableSpeedModifier;
        }

    }

    public float SpeedModifier
    {
        get
        {
            return speedModifier;
        }
    }
}
