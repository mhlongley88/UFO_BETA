using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableWhenBossHealth : MonoBehaviour
{
    public Boss boss;
    public GameObject objectToEnable;
    public GameObject objectToDisable;
    public float percentage = 0.5f;

    void Update()
    {
        if(boss.health <= boss.maxHealth * percentage)
        {
            objectToEnable.SetActive(true);
            enabled = false;
        }
    }
}
