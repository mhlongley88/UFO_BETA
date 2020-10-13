using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetConqueredSpriteText : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerManager.Instance.SetConqueredSpriteText(this.GetComponent<SpriteRenderer>());
    }
}
