using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGameOptionsRetry : MonoBehaviour
{
    public static PostGameOptionsRetry instance;

    public GameObject retryMatchText, nextLevelMatchText;

    private void Awake()
    {
        instance = this;

        //nextLevelMatchText.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        nextLevelMatchText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
