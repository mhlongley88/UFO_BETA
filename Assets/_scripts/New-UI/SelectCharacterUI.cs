using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class SelectCharacterUI : MonoBehaviour
{
    public Image img;
    public Sprite LockedSpr, UnlockedSpr;
    public UnityEvent onClick;
    private void Awake()
    {
        //img = this.GetComponent<Image>();
    }

    public void SetSprite(bool locked)
    {
        img.sprite = locked ? LockedSpr : UnlockedSpr;
    }

}
