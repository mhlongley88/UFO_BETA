using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AssignFollowTarget : MonoBehaviour
{
    private Transform avgVctr;

    public CinemachineVirtualCamera specialVCam;

    // Start is called before the first frame update
    void Start()
    {
        avgVctr = GameObject.Find("CAMERA/AverageVector").transform;
        specialVCam.Follow = avgVctr;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
