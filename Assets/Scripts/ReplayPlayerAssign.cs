using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ReplayPlayerAssign : MonoBehaviour
{
    public CinemachineVirtualCamera vCam3;

    private Transform PlayerTransform;

    // Start is called before the first frame update
    void Awake()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        vCam3.Follow = PlayerTransform;
        vCam3.LookAt = PlayerTransform;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
