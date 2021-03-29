using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UFOPrefs
{
    public UFOPrefs()
    {
        ufoData = new List<UFOAttributes>();
    }
    public List<UFOAttributes> ufoData;
}
[System.Serializable]
public class UFOAttributes
{
    public UFOAttributes()
    {
        Skins = new List<SkinProps>();
    }
    public List<SkinProps> Skins;

    public int ufoIndex;
    public int ufoLevel;
    public float ufoXP;
    public float Damage;
    public float RateOfFire;
    public float Accuracy;
    public bool isUnlocked;
    public int currSkinId;
    public int priceGems;
}

[System.Serializable]
public class SkinProps
{
    public int id;
    public bool isUnlocked;
    public int priceGems;
}