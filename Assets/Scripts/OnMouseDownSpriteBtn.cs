using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnMouseDownSpriteBtn : MonoBehaviour
{
    public UnityEvent onClick = new UnityEvent();

    private void OnMouseDown()
    {
        onClick.Invoke();
    }
}
