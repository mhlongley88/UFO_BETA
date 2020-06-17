using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableMe : MonoBehaviour
{

    public float seconds;

    float elapsed = 0.0f;

    bool processed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.paused) return;

        elapsed += Time.deltaTime;
        if (elapsed >= seconds && !processed)
        {
            gameObject.SetActive(false);
            processed = true;
        }
    }

    private void OnEnable()
    {
        elapsed = 0.0f;
        processed = false;
       // StartCoroutine(LateCall());
    }

    IEnumerator LateCall()
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);

    }
}
