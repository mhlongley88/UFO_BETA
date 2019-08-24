using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private GameObject myCam;

    // Start is called before the first frame update
    void Start()
    {
        myCam = GameObject.Find("CAMERA/Main Camera");
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(myCam.transform);
        
    }
}
