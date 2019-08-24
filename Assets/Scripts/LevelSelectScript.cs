using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectScript : MonoBehaviour
{

    public static int levelStaticInt;

    private int levelInt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        levelStaticInt = levelInt;

        Debug.Log(levelStaticInt);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            levelInt = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            levelInt = 2;

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            levelInt = 3;

        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            levelInt = 4;

        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            levelInt = 5;

        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            levelInt = 6;

        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            levelInt = 7;

        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            levelInt = 8;

        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            levelInt = 9;

        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            levelInt = 10;

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            levelInt = 11;

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            levelInt = 12;

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            levelInt = 13;

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            levelInt = 14;

        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            levelInt = 15;

        }



    }
    }
