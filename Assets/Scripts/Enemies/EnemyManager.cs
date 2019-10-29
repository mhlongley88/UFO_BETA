using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 20.0f;
    public Transform[] spawnPoints;

    public void Awake()
    {
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, enemyPrefab.transform.rotation);

        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(SpawnEnemies());

        Debug.Log("Spawned another enemy");
    }
}
