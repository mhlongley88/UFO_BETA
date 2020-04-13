using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionInstantiate : MonoBehaviour
{
    public GameObject ExplosionPfX;
    private bool spawnedOnce;
    // Start is called before the first frame update
    void Start()
    {
        spawnedOnce = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (spawnedOnce == true)
        {
            Instantiate(ExplosionPfX, gameObject.transform.position, Quaternion.identity);
            spawnedOnce = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            //yield return new WaitForSeconds(2);
            // Destroy(gameObject);
        }
    }

}