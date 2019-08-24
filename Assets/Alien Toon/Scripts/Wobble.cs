using UnityEngine;
using System;


public class Wobble:MonoBehaviour
{
    
    public float wobble = 0.01f;
    public float wobbleSpeed = 3.0f;
    
    
    
    
    public void Update() {
    	var tmp_cs1 = transform.localRotation;
        tmp_cs1.x = ((Mathf.Sin((Time.time) * wobbleSpeed) - wobble*.5f) )*wobble;
        tmp_cs1.z = ((Mathf.Cos((Time.time) * wobbleSpeed) - wobble) )*wobble*2;
        transform.localRotation = tmp_cs1;
    }
}