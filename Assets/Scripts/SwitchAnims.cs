using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnims : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject characterINTRO;
    public GameObject characterMID;

    void OnEnable()
    {
        characterINTRO.SetActive(false);
        characterMID.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
