using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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
        if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            GameObject temp;
            if (PhotonNetwork.IsMasterClient) { 
                temp = PhotonNetwork.Instantiate(enemyPrefab.name + "Mul", spawnPoints[Random.Range(0, spawnPoints.Length)].position, enemyPrefab.transform.rotation);
                temp.GetComponent<AirEnemy>().enabled = true;
            }
        }
        else
        {
            Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, enemyPrefab.transform.rotation);
        }
        //Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, enemyPrefab.transform.rotation);

        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(SpawnEnemies());

        Debug.Log("Spawned another enemy");
    }
}
