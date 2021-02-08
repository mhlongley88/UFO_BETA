using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TouchGameUI : MonoBehaviour
{
    public static TouchGameUI instance;
    public GameObject LevelScreenControls, ResultScreenControls;
    public Image AbductButtonImg, AbductButtonContainerImg;
    public Sprite AbductSprite, AbductContainerSprite, SpecialWeaponSprite, SpecialWeaponContainerSprite;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void ToggleSuperWeaponButtonSprite(bool en)
    {
        if (en)
        {
            AbductButtonImg.sprite = SpecialWeaponSprite;
            AbductButtonContainerImg.sprite = SpecialWeaponContainerSprite;
        }
        else
        {
            AbductButtonImg.sprite = AbductSprite;
            AbductButtonContainerImg.sprite = AbductContainerSprite;
        }
    }
}
