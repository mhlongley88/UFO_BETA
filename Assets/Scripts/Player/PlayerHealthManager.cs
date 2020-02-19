using System.Collections;
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

    public Sprite healthCountSprite;
    public Image[] healthCounterImages;

    private LifeManager lifeManager;
    public LifeManager LifeManager => lifeManager;

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

        //for (int i = 0; i < healthCounterImages.Length; i++)
        //{
        //    var img = healthCounterImages[i];
        //    img.color = Color.white;
        //    img.sprite = healthCountSprite;
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        currHealth = startingHealth;
        lifeManager = LevelUIManager.Instance.GetLifeManager(playerController.player);

        if(TutorialManager.instance != null)
        {
            for (int i = 0; i < healthCounterImages.Length; i++)
            {
                var img = healthCounterImages[i];
                img.gameObject.SetActive(false);
            }
        }
    }

    public void ApplyTintOnCircles(Color tint)
    {
        for (int i = 0; i < lifeManager.lifeIndicators.Length; i++)
        {
            lifeManager.lifeIndicators[i].color = tint;
        }

        for (int i = 0; i < healthCounterImages.Length; i++)
        {
            healthCounterImages[i].color = tint;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //refill health
        if (CurrHealth < maxHealth && Time.time >= lastDamageTime + refillDelay)
        {
            CurrHealth = Mathf.Lerp(CurrHealth, maxHealth, CurrHealth / maxHealth + Time.deltaTime / refillDuration);
        }

        var lives = PlayerManager.Instance.players[playerController.player].lives;
        for (int i = 0; i < healthCounterImages.Length; i++)
        {
            if(i >= lives)
                healthCounterImages[i].color = Color.clear;      
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
