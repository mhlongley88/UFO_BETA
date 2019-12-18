using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcheroo : MonoBehaviour

{
    public GameObject watchedObject;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.SetActive(true);

        if (watchedObject == true)
        {
            this.gameObject.SetActive(false);
        }

        if(watchedObject==false)
        {
            this.gameObject.SetActive(true);
        }



    }
}
