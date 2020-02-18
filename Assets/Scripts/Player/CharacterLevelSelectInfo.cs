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
    private Sprite characterHead;
    [SerializeField]
    public Color characterLivesCircleTint;

    public string Name { get { return _name; } }
    public Sprite WeaponType { get { return _weaponType; } }
    public Sprite SpecialWeapon { get { return _specialWeapon; } }
    public float Damage { get { return _damage; } }
    public float RateOfFire { get { return _rateOfFire; } }
    public float Accuracy { get { return _accuracy; } }

    public Sprite CharacterHead => characterHead;
    public Color CharacterLivesCircleTint => characterLivesCircleTint;
}
