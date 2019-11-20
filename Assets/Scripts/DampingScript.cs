using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.DemiLib;
using Photon.Pun;
public class DampingScript : MonoBehaviour {

	public Transform target;
	public float smoothTime = 0.3F;
    public float smooth = 5.0f;
	private Vector3 velocity = Vector3.zero;

    public bool checkRotation;
    public PhotonView pv;
    //private GameObject otherPlayer;

    //public float bumpAmount;

    private void Start()
    {
        
    }

    void Update()
	{
		// Define a target position above and behind the target transform
		Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, 0));

		// Smoothly move the camera towards that target position
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        if(checkRotation == true)
        {
            transform.rotation = target.rotation;
        }

    }


}