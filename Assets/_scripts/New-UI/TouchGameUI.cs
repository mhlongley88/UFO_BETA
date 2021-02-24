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
    public bool isTutorialScene;
    public List<Image> AbductButtonImgs, AbductButtonContainerImgs;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Debug.Log(this.name);
    }

    public void ToggleSuperWeaponButtonSprite(bool en)
    {
        if (en)
        {
            if (isTutorialScene)
            {
                SetSpritesTutorialCase(SpecialWeaponSprite, SpecialWeaponContainerSprite);
            }
            else
            {
                AbductButtonImg.sprite = SpecialWeaponSprite;
                AbductButtonContainerImg.sprite = SpecialWeaponContainerSprite;
            }
        }
        else
        {
            if (isTutorialScene)
            {
                SetSpritesTutorialCase(AbductSprite, AbductContainerSprite);
            }
            else
            {
                AbductButtonImg.sprite = AbductSprite;
                AbductButtonContainerImg.sprite = AbductContainerSprite;
            }
        }
    }

    void SetSpritesTutorialCase(Sprite spr, Sprite container)
    {
        foreach(Image img in AbductButtonImgs)
        {
            //if(img.gameObject.activeSelf)
                img.sprite = spr;
        }
        foreach (Image img in AbductButtonContainerImgs)
        {
            //if (img.gameObject.activeSelf)
                img.sprite = container;
        }
    }

}
