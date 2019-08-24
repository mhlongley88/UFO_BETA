using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P4_UFOMovement : MonoBehaviour {

	public float speed;
	private float totalSpeed;
	private Rigidbody rb;
	public GameObject beam;
	public List<GameObject> abductedObjects;
	public float thrust;
	public float beamSpeed;

	public Transform playerRotation;

	public string controller;

	public static bool inBoost;

	//private float playerRot;

	void Start()
	{
		beam.SetActive (false);
		rb = GetComponent<Rigidbody> ();
		abductedObjects = new List<GameObject> ();
		totalSpeed = speed;

	}

	void FixedUpdate()
	{
		float hAxis = Input.GetAxis ("P4_Horizontal");
		float vAxis = Input.GetAxis ("P4_Vertical");

		Vector3 playerFwd = new Vector3 (hAxis, 0.0f, vAxis);

		// apply line below when not boosting or moving
		//rb.velocity = rb.velocit	y * 0.125f;

		inBoost = false;

		if (Input.GetButton ("p1_Start")) {
			controller = "p1";
			Debug.Log (controller);
		} else if (Input.GetButton ("p2_Start")) {
			controller = "p2";
			Debug.Log (controller);
		} else if (Input.GetButton ("p3_Start")) {
			controller = "p3";
			Debug.Log (controller);
		} else if (Input.GetButton ("p4_Start")) {
			controller = "p4";
			Debug.Log (controller);
		}

		if (Input.GetKeyDown(KeyCode.Joystick2Button0)) {

			beam.SetActive (true);
			totalSpeed = speed * beamSpeed;

		} else if (Input.GetKeyUp(KeyCode.Joystick2Button0)) {

			beam.SetActive (false);
			totalSpeed = speed;
		}


		Vector3 movement = playerFwd * totalSpeed * Time.deltaTime;

		rb.MovePosition (transform.position + movement);


		//rb.AddForce (playerFwd * 10000.0f, ForceMode.Force);

		if (Input.GetKeyDown(KeyCode.Joystick2Button1)) {

			rb.transform.position += playerFwd * thrust * Time.deltaTime;  
			inBoost = true;
			//yield return new WaitForSeconds (5f);
		}

		if (Input.GetKeyUp (KeyCode.Joystick1Button1)) {

			inBoost = false;

		}
	}

	// TODO: when we collide with another UFO, drop largest abudcted object
	// on Collision Enter


}