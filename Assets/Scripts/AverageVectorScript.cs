using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AverageVectorScript : MonoBehaviour
{
    public CinemachineVirtualCamera gameVCam;

    public GameObject targetScaler;

   // public GameObject[] players;

   /* private float p1Scale;
    private float p2Scale;
    private float p3Scale;
    private float p4Scale;
    private int player1 = 1;

    private float averageScale;*/

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      //  players[1].gameObject.transform.localScale.x = p1Scale;

      //  players[].transform.localScale.x = averageScale;

        gameVCam.m_Lens.OrthographicSize = targetScaler.transform.localScale.x;
    }

}

