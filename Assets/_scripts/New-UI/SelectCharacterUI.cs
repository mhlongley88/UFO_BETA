using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectCharacterUI : MonoBehaviour
{
    public Image img;
    public Sprite LockedSpr, UnlockedSpr;

    private void Awake()
    {
        //img = this.GetComponent<Image>();
    }

    public void SetSprite(bool locked)
    {
        img.sprite = locked ? LockedSpr : UnlockedSpr;
    }
}
