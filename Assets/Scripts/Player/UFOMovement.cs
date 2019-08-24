using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOMovement : MonoBehaviour {

	public float speed;
	private float totalSpeed;
	private Rigidbody rb;
	public GameObject beam;
	public List<GameObject> abductedObjects;
	public float thrust;
	public float beamSpeed;

	public Transform playerRotation;

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
		float hAxis = Input.GetAxis ("Horizontal");
		float vAxis = Input.GetAxis ("Vertical");

		Vector3 playerFwd = new Vector3 (hAxis, 0.0f, vAxis);

		// apply line below when not boosting or moving
		//rb.velocity = rb.velocit	y * 0.125f;

		if (Input.GetButtonDown("Fire1")) {

			beam.SetActive (true);
			totalSpeed = speed * beamSpeed;

		} else if (Input.GetButtonUp("Fire1")) {

			beam.SetActive (false);
			totalSpeed = speed;
		}


		Vector3 movement = playerFwd * totalSpeed * Time.deltaTime;

		rb.MovePosition (transform.position + movement);


		//rb.AddForce (playerFwd * 10000.0f, ForceMode.Force);

		if (Input.GetButtonDown("Fire2")) {

			rb.transform.position += playerFwd * thrust * Time.deltaTime;  

			//yield return new WaitForSeconds (5f);
		}
	}

	// TODO: when we collide with another UFO, drop largest abudcted object
	// on Collision Enter


}