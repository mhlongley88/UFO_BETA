using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instancePostGame : MonoBehaviour
{
    public static instancePostGame instance;
    // Start is called before the first frame update
    void Start()
    {
     
        
    }

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
