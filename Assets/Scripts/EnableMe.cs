using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMe : MonoBehaviour
{

    public float seconds;
    public GameObject enableObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateCall());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
       // StartCoroutine(LateCall());
    }

    IEnumerator LateCall()
    {
        yield return new WaitForSeconds(seconds);
        enableObject.SetActive(true);

    }
}
