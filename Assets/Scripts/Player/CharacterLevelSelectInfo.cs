using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLevelSelectInfo : MonoBehaviour
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private Sprite _weaponType;
    [SerializeField]
    private Sprite _specialWeapon;
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _rateOfFire;
    [SerializeField]
    private float _accuracy;
    [SerializeField]
    private int _projCountAuto;
    [SerializeField]
    private float _fireDelayAuto;
    [SerializeField]
    private Sprite characterHead;
    [SerializeField]
    private Color characterLivesCircleTint;
    [SerializeField]
    private GameObject muzzleFlashVfx;
    public int ufoIndex;
    public bool isUnlocked;
    public string Name { get { return _name; } }
    public Sprite WeaponType { get { return _weaponType; } }
    public Sprite SpecialWeapon { get { return _specialWeapon; } }
    public float Damage { get { return _damage; } }
    public float RateOfFire { get { return _rateOfFire; } }
    public float Accuracy { get { return _accuracy; } }

    public float ProjectileCount { get { return _projCountAuto; } }
    public float FireDelayAuto { get { return _fireDelayAuto; } }

    public Sprite CharacterHead => characterHead;
    public Color CharacterLivesCircleTint => characterLivesCircleTint;

    public GameObject MuzzleFlashVfx => muzzleFlashVfx;

    void Start()
    {
        //UpdateValues(UserPrefs.instance.GetUFOProps().ufoData.Find(item => item.ufoIndex == ufoIndex));
    }

    public void UpdateValues(UFOAttributes values)
    {
        _damage = values.Damage;
        _accuracy = values.Accuracy;
        _rateOfFire = values.RateOfFire;
    }

}
