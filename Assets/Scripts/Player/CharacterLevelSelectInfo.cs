using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLevelSelectInfo : MonoBehaviour
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private string _subname;
    [SerializeField]
    private string _details;
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
    public string SubName { get { return _subname; } }
    public string Details { get { return _details; } }
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

    public CompleteSkin[] Skins;
    public int currentSkinId, priceGems;
    

    void Start()
    {
        if (PlayerManager.Instance && GameManager.Instance.GetTryProps() == null)
        {
            UFOAttributes attr = GameManager.Instance.GetUfoAttribute(ufoIndex);
            currentSkinId = attr.currSkinId;
            ActivateSkin();
        }
    }

    public void ActivateSkin()
    {
        

        foreach(CompleteSkin cSkin in Skins)
        {
            foreach(SkinStats skin in cSkin.skins)
            {
                if (skin.hasAdditionalRequirements)
                {
                    skin.OnDeactivate.Invoke();
                }
                skin.gameObject.SetActive(false);
            }
        }
        if (currentSkinId < 0) return;


        CompleteSkin activeSkin = Skins[currentSkinId];

        foreach(SkinStats skin in activeSkin.skins)
        {
            skin.gameObject.SetActive(true);
            if (skin.hasAdditionalRequirements)
            {
                skin.OnActivate.Invoke();
            }
        }
        
    }


    public void ActivateSkin_LoadOutStore(int id)
    {
        if (currentSkinId < 0) return;

        foreach (CompleteSkin cSkin in Skins)
        {
            foreach (SkinStats skin in cSkin.skins)
            {
                if (skin.hasAdditionalRequirements)
                {
                    skin.OnDeactivate.Invoke();
                }
                skin.gameObject.SetActive(false);
            }
        }
        //Debug.Log(id);
        CompleteSkin activeSkin = Skins[id];

        foreach (SkinStats skin in activeSkin.skins)
        {
            skin.gameObject.SetActive(true);
            if (skin.hasAdditionalRequirements)
            {
                skin.OnActivate.Invoke();
            }
        }

    }

    public void UpdateValues(UFOAttributes values)
    {
        _damage = values.Damage;
        _accuracy = values.Accuracy;
        _rateOfFire = values.RateOfFire;
    }

}
[System.Serializable]
public class CompleteSkin
{
    public SkinStats[] skins;
}