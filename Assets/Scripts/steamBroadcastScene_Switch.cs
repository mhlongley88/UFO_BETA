using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class steamBroadcastScene_Switch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.RightShift))
            if (Input.GetKeyDown(KeyCode.X))
            {
                {
                    Application.LoadLevel(21);
                }
            }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(0);
        }
        
    }
}
