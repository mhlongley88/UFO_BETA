using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionForceScript : MonoBehaviour {

	public float radius = 5.0f;
	public float power = 10.0f;

	// Use this for initialization
	void Start () {

		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere (explosionPos, radius);

		foreach (Collider hit in colliders) {

			Rigidbody rb = hit.GetComponent<Rigidbody> ();

			if (rb != null)
				rb.AddExplosionForce (power, explosionPos, radius, 3.0f);

		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
