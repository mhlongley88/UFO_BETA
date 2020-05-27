using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableWhenBossHealth : MonoBehaviour
{
    public Boss boss;
    public GameObject objectToEnable;
    public float percentage = 0.5f;
    public UnityEvent OnProcess = new UnityEvent();

    void Update()
    {
        if(boss.health <= boss.maxHealth * percentage)
        {
            objectToEnable.SetActive(true);
            enabled = false;

            OnProcess.Invoke();
        }
    }
}
