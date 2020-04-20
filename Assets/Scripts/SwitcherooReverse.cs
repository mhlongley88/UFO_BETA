using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitcherooReverse : MonoBehaviour
{
    public GameObject watchedObject;
    private bool switchIt;

    // Start is called before the first frame update
    void Start()
    {
        switchIt = false;
    }

    // Update is called once per frame
    void Update()
    {
        //this.gameObject.SetActive(true);

        if (switchIt == true)
        {
            this.gameObject.SetActive(false);
        }

        if(watchedObject == true)
        {
            switchIt = true;
        }
    }
}
