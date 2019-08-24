using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.DemiLib;

public class CameraController : MonoBehaviour
{

    public GameObject gameCam;
    public GameObject specialCamera;
    public Camera cam;

    //public Transform player1;
    //public Transform player2;
    //	public Transform player3;
    //	public Transform player4;

    public GameObject boundaryL;
    public GameObject boundaryR;
    public GameObject boundaryTop;
    public GameObject boundaryBottom;

    public Transform[] boundaries;
    Vector3[] oldBoundariesXZ;

    //private float gameCamStartSize;

    //public AverageScaleOutput avgScaleOutput;
    public float boundariesBaseScale = 5;
    private static CameraController instance;
    public static CameraController Instance
    {
        get
        {
            return instance;
        }
    }

    // Use this for initialization
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        //gameCam.orthographicSize = gameCamStartSize;

        oldBoundariesXZ = new Vector3[boundaries.Length];
        for (int i = 0; i < boundaries.Length; i++)
        {
            oldBoundariesXZ[i] = boundaries[i].position;
        }

    }

    public IEnumerator ActivateSpecialCamera(float duration)
    {
        specialCamera.SetActive(true);
        yield return new WaitForSeconds(duration);
        specialCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < oldBoundariesXZ.Length; i++)
        {
            var oldBoundaryXZ = oldBoundariesXZ[i];

            var mappedX = (oldBoundaryXZ.x * cam.orthographicSize) / boundariesBaseScale;
            var mappedZ = (oldBoundaryXZ.z * cam.orthographicSize) / boundariesBaseScale;

            boundaries[i].position = new Vector3(mappedX, oldBoundaryXZ.y, mappedZ);
        }


        if (Input.GetKeyDown(KeyCode.G))
        {

            //gameCam.orthographicSize = 80.0f;
            gameCam.GetComponent<DOTweenAnimation>().DOPlayById("CameraScaleUp1");

            boundaryBottom.GetComponent<DOTweenAnimation>().DOPlayAllById("CameraBoundaryScaleUp");
            boundaryL.GetComponent<DOTweenAnimation>().DOPlayAllById("CameraBoundaryScaleUp");
            boundaryR.GetComponent<DOTweenAnimation>().DOPlayAllById("CameraBoundaryScaleUp");
            boundaryTop.GetComponent<DOTweenAnimation>().DOPlayAllById("CameraBoundaryScaleUp");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {

            gameCam.GetComponent<DOTweenAnimation>().DOPlayById("CameraScaleDown");

            boundaryBottom.GetComponent<DOTweenAnimation>().DOPlayAllById("CameraBoundaryScaleDown");
            boundaryL.GetComponent<DOTweenAnimation>().DOPlayAllById("CameraBoundaryScaleUpDown");
            boundaryR.GetComponent<DOTweenAnimation>().DOPlayAllById("CameraBoundaryScaleUpDown");
            boundaryTop.GetComponent<DOTweenAnimation>().DOPlayAllById("CameraBoundaryScaleUpDown");

        }


    }


}
