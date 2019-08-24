using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour {

    public float DeathTime;

	// Use this for initialization
	void Start () {


		Destroy (gameObject, DeathTime);
		
	}
	
	// Update is called once per frame
	void Update () {


	}
}
