using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1_UFOMovement : MonoBehaviour {

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

	public Transform hitPrefabSM;
	public Transform hitPrefabLG;

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
		float hAxis = Input.GetAxis ("P1_Horizontal");
		float vAxis = Input.GetAxis ("P1_Vertical");

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
		}


		if (Input.GetKeyDown(KeyCode.Joystick1Button0)) {

			beam.SetActive (true);
			totalSpeed = speed * beamSpeed;
		//	Debug.Log ("p1 beam");

		} else if (Input.GetKeyUp(KeyCode.Joystick1Button0)) {

			beam.SetActive (false);
			totalSpeed = speed;
		}


		Vector3 movement = playerFwd * totalSpeed * Time.deltaTime;

		rb.MovePosition (transform.position + movement);


		//rb.AddForce (playerFwd * 10000.0f, ForceMode.Force);

		if (Input.GetKeyDown(KeyCode.Joystick1Button1)) {

			rb.transform.position += playerFwd * thrust * Time.deltaTime;
			inBoost = true;

			//yield return new WaitForSeconds (5f);
		}

		if (Input.GetKeyUp (KeyCode.Joystick1Button1)) {

			inBoost = false;

		}
			
	}

	void OnCollisionEnter(Collision coll){

		Debug.Log (coll.gameObject.name);

		if (coll.gameObject.tag == "Player2"&& inBoost == true) {

			ContactPoint contact = coll.contacts [0];
			Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			Instantiate (hitPrefabSM, pos, rot);

		} else if (coll.gameObject.tag == "Player3" && inBoost == false) {

			ContactPoint contact = coll.contacts [0];
			Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			Instantiate (hitPrefabSM, pos, rot);
	
		} else if (coll.gameObject.tag == "Player4" && inBoost == false) {

			ContactPoint contact = coll.contacts [0];
			Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			Instantiate (hitPrefabSM, pos, rot);
		}

		// TODO: when we collide with another UFO, drop largest abudcted object
		// on Collision Enter

	}
}