using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStop : MonoBehaviour
{
    public GameObject BGM_object;
    public GameObject MenuObject;
    public GameObject LocalPlayButtonObject;

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
        BGM_object.SetActive(false);
        MenuObject.SetActive(false);
        LocalPlayButtonObject.SetActive(false);

    }

    private void OnDisable()
    {
        BGM_object.SetActive(true);
        MenuObject.SetActive(true);
        LocalPlayButtonObject.SetActive(true);
    }
}
