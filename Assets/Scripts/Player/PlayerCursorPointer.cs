using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursorPointer : MonoBehaviour
{
    public static PlayerCursorPointer instance;

    Plane plane;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        plane = new Plane();
        
    }

    // Update is called once per frame
    void Update()
    {
        plane.SetNormalAndPosition(Vector3.up, Vector3.zero);

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;
        if (plane.Raycast(ray, out enter))
        {
            var hitPoint = ray.GetPoint(enter);
            var screenPoint = Camera.main.WorldToScreenPoint(hitPoint);
            transform.localPosition = screenPoint;

            transform.localPosition += Vector3.left * Screen.width / 2.0f + Vector3.down * Screen.height / 2.0f;
        }

    }
}
