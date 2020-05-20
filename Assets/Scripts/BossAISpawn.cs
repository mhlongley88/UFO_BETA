using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAISpawn : MonoBehaviour
{
    public AIPreset aiPreset;
    public GameObject prefab;
    public Transform spawnPoint;

    public void Spawn()
    {
        GameObject ai = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        ai.transform.localScale = spawnPoint.localScale;

        var bot = ai.AddComponent<PlayerBot>();
        bot.preset = aiPreset;
    }
}
