using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instanceUI : MonoBehaviour
{
    public static instanceUI instance;
    // Start is called before the first frame update
    void Start()
    {


    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

}