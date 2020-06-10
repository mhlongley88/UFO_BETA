using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{

    public int maxRandomRange;

    private int randNum;

    public GameObject[] backgrounds;
    public GameObject[] earths;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        randNum = Random.Range(0, maxRandomRange);
        backgrounds[randNum].SetActive(true);
        earths[randNum].SetActive(true);
    }

    private void OnDisable()
    {
        randNum = Random.Range(0, maxRandomRange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
