using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamesCompletedTally : MonoBehaviour
{
    public static bool gameWasCompleted;
    public static int gamesCompleted;

    public int Threshold1;
    public int Threshold2;
    public int Threshold3;
    public int Threshold4;
    public int Threshold5;
    public int Threshold6;
    public int Threshold7;
    public int Threshold8;
    public int Threshold9;
    public int Threshold10;
    public int Threshold11;
    public int Threshold12;
    public int Threshold13;
    public int Threshold14;
    public int Threshold15;

    public GameObject LevelUnlock1;
    public GameObject LevelUnlock2;
    public GameObject LevelUnlock3;
    public GameObject LevelUnlock4;
    public GameObject LevelUnlock5;
    public GameObject LevelUnlock6;
    public GameObject LevelUnlock7;
    public GameObject LevelUnlock8;
    public GameObject LevelUnlock9;
    public GameObject LevelUnlock10;
    public GameObject LevelUnlock11;
    public GameObject LevelUnlock12;
    public GameObject LevelUnlock13;
    public GameObject LevelUnlock14;
    public GameObject LevelUnlock15;

    public GameObject[] Locked1;
    public GameObject[] Locked2;
    public GameObject[] Locked3;
    public GameObject[] Locked4;
    public GameObject[] Locked5;
    public GameObject[] Locked6;
    public GameObject[] Locked7;
    public GameObject[] Locked8;
    public GameObject[] Locked9;
    public GameObject[] Locked10;
    public GameObject[] Locked11;
    public GameObject[] Locked12;
    public GameObject[] Locked13;
    public GameObject[] Locked14;
    public GameObject[] Locked15;

    private int arrayCount = 9;

    // Start is called before the first frame update
    void Start()
    {
        gameWasCompleted = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(gamesCompleted);

        // gameWasCompleted = false;

        if (Input.GetKeyDown(KeyCode.P))
        {
            gamesCompleted = gamesCompleted + 1;
            Debug.Log(gamesCompleted);
        }

        if(gameWasCompleted == true)
        {
          //  gamesCompleted=gamesCompleted+1;
           // gameWasCompleted = false;
        }

        if(gamesCompleted >= Threshold1)
        {
            LevelUnlock1.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked1[i].SetActive(false);
            }
        }
        if (gamesCompleted >= Threshold2)
        {
            LevelUnlock2.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked2[i].SetActive(false);
            }
        }
        if (gamesCompleted >= Threshold3)
        {
            LevelUnlock3.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked3[i].SetActive(false);
            }
        }
        if (gamesCompleted >= Threshold4)
        {
            LevelUnlock4.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked4[i].SetActive(false);
            }
        }
        if (gamesCompleted >= Threshold5)
        {
            LevelUnlock5.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked5[i].SetActive(false);
            }
        }
        if (gamesCompleted >= Threshold6)
        {
            LevelUnlock6.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked6[i].SetActive(false);
            }
        }
        if (gamesCompleted >= Threshold7)
        {
            LevelUnlock7.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked7[i].SetActive(false);
            }
        }
        if (gamesCompleted >= Threshold8)
        {
            LevelUnlock8.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked8[i].SetActive(false);
            }
        }
        if (gamesCompleted >= Threshold9)
        {
            LevelUnlock9.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked9[i].SetActive(false);
            }
        }
        if (gamesCompleted >= Threshold10)
        {
            LevelUnlock10.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked10[i].SetActive(false);
            }
        }
        if (gamesCompleted >= Threshold11)
        {
            LevelUnlock11.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked11[i].SetActive(false);
            }
        }
        if (gamesCompleted >= Threshold12)
        {
            LevelUnlock12.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked12[i].SetActive(false);
            }
        }
        if (gamesCompleted >= Threshold13)
        {
            LevelUnlock13.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked13[i].SetActive(false);
            }
        }
        if (gamesCompleted >= Threshold14)
        {
            LevelUnlock14.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked14[i].SetActive(false);
            }
        }
        if (gamesCompleted >= Threshold15)
        {
            LevelUnlock15.SetActive(true);
            for (int i = 0; i < arrayCount; ++i)
            {
                Locked15[i].SetActive(false);
            }
        }

        /*  if (Input.GetKeyDown(KeyCode.I))
          {
              Debug.Log(gamesCompleted);
          }*/
    }
}
