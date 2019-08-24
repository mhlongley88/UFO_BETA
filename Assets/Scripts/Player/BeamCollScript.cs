using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamCollScript : MonoBehaviour {

	// Use this for initialization

	public Transform abductPoint;

	public float speed;

	void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other){

		if (other.gameObject.tag == "Beam") {

			StartCoroutine ("AbductObject");

		}

	}

	IEnumerator AbductObject(){

		float step = speed * Time.deltaTime;

		this.gameObject.transform.position = Vector3.MoveTowards (transform.position, abductPoint.position, step);

		yield return new WaitForEndOfFrame ();

	}
}
