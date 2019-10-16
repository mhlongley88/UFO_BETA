using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Boss : MonoBehaviour
{
    public int maxHealth = 40;
    public int health = 40;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        
    }

    public void OnTakeDamage()
    {
        Debug.Log("Boss took damage");

        health -= 2;
        if(health <= 0)
        {
            Debug.Log("Boss is Dead");
        }
    }
}
