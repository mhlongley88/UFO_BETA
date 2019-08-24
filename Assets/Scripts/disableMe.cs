using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableMe : MonoBehaviour
{

    public float seconds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(LateCall());
    }

    IEnumerator LateCall()
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);

    }
}
