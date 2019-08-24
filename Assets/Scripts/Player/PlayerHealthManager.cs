﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{


    [SerializeField]
    private ScriptableHealth healthSettings;


    private float refillDuration = 8f;

    /// <summary>
    /// Amount of time after taking damage before health starts to refill
    /// </summary>

    private float refillDelay = 3f;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Slider healthMeter;

    /// <summary>
    /// Time that the player last took damage
    /// </summary>
    private float lastDamageTime;
    private float maxHealth;
    private float minHealth;
    private float currHealth;
    private float startingHealth;

    private bool invincible = false;

    public float CurrHealth
    {
        get
        {
            return currHealth;
        }

        private set
        {
            currHealth = value;
        }
    }

    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }

        private set
        {
            maxHealth = value;
        }
    }

    public float MinHealth
    {
        get
        {
            return minHealth;
        }

        private set
        {
            minHealth = value;
        }
    }

    public float StartingHealth
    {
        get
        {
            return startingHealth;
        }

        private set
        {
            startingHealth = value;
        }
    }


    public void Awake()
    {
        minHealth = healthSettings.minHealth;
        maxHealth = healthSettings.maxHealth;
        startingHealth = healthSettings.startingHealth;
        refillDelay = healthSettings.refillDelay;
        refillDuration = healthSettings.refillDuration;
    }

    // Start is called before the first frame update
    void Start()
    {
        currHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {

        //refill health
        if (CurrHealth < maxHealth && Time.time >= lastDamageTime + refillDelay)
        {
            CurrHealth = Mathf.Lerp(CurrHealth, maxHealth, CurrHealth / maxHealth + Time.deltaTime / refillDuration);
        }
    }

    public void ChangeHealth(float healAmount)
    {
        if (!invincible)
        {
            if (healAmount < 0f)
            {
                lastDamageTime = Time.time;
            }
            CurrHealth = Mathf.Clamp(CurrHealth + healAmount, minHealth, maxHealth);
            healthMeter.value = currHealth / maxHealth;
            if (CurrHealth <= minHealth)
            {
                playerController.Die();
            }
        }
    }

    public void SetInvincible(bool val)
    {
        invincible = val;
    }


    public bool IsInvincible()
    {
        return invincible;
    }
}
