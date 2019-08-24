using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart;


public class enableSuperWeapon : MonoBehaviour
{

    public GameObject mySuperWeaponObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {

        mySuperWeaponObject.SetActive(true);

    }

    private void OnDisable()
    {
        mySuperWeaponObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
