using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMe : MonoBehaviour
{

    public float seconds;
    public GameObject enableObject;

    float elapsed = 0.0f;
    bool processed = false;

    // Start is called before the first frame update
    void Start()
    {
        elapsed = 0.0f;
        processed = false;
        //StartCoroutine(LateCall());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.paused) return;

        elapsed += Time.deltaTime;
        if (elapsed >= seconds && !processed)
        {
            enableObject.SetActive(true);
            processed = true;
        }
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
