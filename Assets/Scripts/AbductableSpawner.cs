using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AbductableSpawner : MonoBehaviour
{
    public Rigidbody[] abductablePrefabs;
    public Vector2 spawnRate = new Vector2(1.5f, 4.5f);
    public float throwForce = 200.0f;
    public float throwForceVertical = 200.0f;
    public bool stop = false;

    float elapsedRate = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        elapsedRate = Time.time + Random.Range(spawnRate.x, spawnRate.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(!stop && elapsedRate < Time.time)
        {
            Spawn();
            elapsedRate = Time.time + Random.Range(spawnRate.x, spawnRate.y);
        }
    }

    void Spawn()
    {
        Rigidbody obj = Instantiate(abductablePrefabs[Random.Range(0, abductablePrefabs.Length)], transform.position, Quaternion.identity);
   
        DOVirtual.DelayedCall(0.05f, () =>
        {

            obj.isKinematic = false;
            obj.constraints = RigidbodyConstraints.None;
            obj.AddForce(obj.transform.forward * throwForce + Vector3.up * throwForceVertical, ForceMode.Force);
        });

        Vector3 eulerAng = new Vector3(0.0f, Random.Range(0, 360.0f), 0.0f);
        obj.transform.eulerAngles = eulerAng;
    }
}
