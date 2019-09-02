using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggler : MonoBehaviour
{
    public GameObject UIElements;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UIElements.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIElements.SetActive(true);
        }

    }
}
