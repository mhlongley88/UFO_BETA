using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossAISpawn : MonoBehaviour
{
    public GameObject pawn;
    public float offsetScale = 6.612904f;
    public AIPreset aiPreset;
    public GameObject prefab;
    public Transform spawnPoint;
    public ScriptableHealth scriptableHealth;

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
      //  playerController.GetComponent<Collider>().enabled = false;
        var boss = ai.AddComponent<Boss>();
        playerController.healthManager.HealthSettings = scriptableHealth;
        playerController.OnTakeDamage += boss.OnTakeDamage;

        var bot = ai.AddComponent<PlayerBot>();
        bot.preset = aiPreset;
    }
}
