using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCamEnable : MonoBehaviour
{
    public float seconds;
    private GameObject enableObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateCall());
        enableObject = GameObject.Find("/LevelObjects/CAMERA/CM vcam1");
        enableObject.SetActive(false);
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