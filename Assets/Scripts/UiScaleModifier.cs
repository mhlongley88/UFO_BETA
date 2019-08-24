using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiScaleModifier : MonoBehaviour
{
    public Transform AverageVector;
    public AverageScaleOutput avgScaleOutput;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.localScale = AverageVector.localScale - new Vector3(-8.5f, 8.5f, -8.5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.localScale = AverageVector.localScale/5.5f;
    
    }
}
