using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class BossAISpawn : MonoBehaviour
{
    public GameObject pawn;
    public int bossInitialHealth = 200;
    public Color healthBarColor;
    public float offsetScale = 6.612904f;
    public AIPreset aiPreset;
    public GameObject prefab;
    public Transform spawnPoint;
    public ScriptableHealth scriptableHealth;
    public EnableWhenBossHealth enableWhen;
    public EnableWhenBossHealth enableWhen2;

    GameObject instance;

    Boss bossInstance;
    PlayerHealthManager healthManager;

    public void Spawn()
    {
        GameObject ai = Instantiate(pawn, spawnPoint.position, spawnPoint.rotation);
        ai.transform.localScale = spawnPoint.localScale;

        var playerController = ai.GetComponent<PlayerController>();
        playerController.pawnModel = prefab;
        playerController.pawn = true;
        playerController.offsetScale = new Vector3(offsetScale, offsetScale, offsetScale);
        playerController.aimFlagObject.SetActive(false);
        playerController.canGrowWhenUsingSpecial = false;

        playerController.healthManager.ApplyTintOnCircles(Color.clear);
        if(playerController.healthManager.HealthMeter.image != null)
            playerController.healthManager.HealthMeter.image.color = healthBarColor;

       var boss = ai.AddComponent<Boss>();
        boss.maxHealth = bossInitialHealth;
        boss.health = bossInitialHealth;

        scriptableHealth.startingHealth = bossInitialHealth;
        scriptableHealth.maxHealth = bossInitialHealth;
        scriptableHealth.minHealth = 0;

        playerController.healthManager.HealthSettings = scriptableHealth;
        playerController.OnTakeDamage += boss.OnTakeDamage;

        healthManager = playerController.healthManager;

        var bot = ai.AddComponent<PlayerBot>();
        bot.preset = aiPreset;

        DOVirtual.DelayedCall(0.23f, () =>
        {
            playerController.healthManager.SetInvincible(false);

            healthManager.HealthMeter.minValue = 0;
            healthManager.HealthMeter.maxValue = 1;
            healthManager.HealthMeter.value = 1;
        });

        if (enableWhen != null)
        {
            enableWhen.boss = boss;
            enableWhen.enabled = true;

            enableWhen2.boss = boss;
            enableWhen2.enabled = true;
        }

        bossInstance = boss;
        instance = ai;
    }

    private void Update()
    {
        if (healthManager != null && bossInstance != null)
        {

            healthManager.HealthMeter.value = (float)bossInstance.health / (float)bossInstance.maxHealth;
        }

    }

    public void EnableAI()
    {
        instance.SetActive(true);
    }

    public void DisableAI()
    {
        instance.SetActive(false);
    }
}
