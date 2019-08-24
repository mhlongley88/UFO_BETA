using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AddToTargetGroup : MonoBehaviour
{

    private CinemachineTargetGroup MyTargetGroup;
    private Transform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = this.transform;
        MyTargetGroup = GameObject.Find("CAMERA/TargetGroup1").GetComponent<CinemachineTargetGroup>();
        MyTargetGroup.AddMember(myTransform, 1.0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
