using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour {

	public GameObject Player1_UI;
	public GameObject Player2_UI;
	public GameObject Wins_UI;
	public GameObject Player1;
	public GameObject Player2;

	private bool gameEnd;

	// Use this for initialization
	void Start () {

		Player1_UI.SetActive (false);
		Player2_UI.SetActive (false);
		Wins_UI.SetActive (false);
		gameEnd = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider coll){

		if (coll.gameObject.tag == "Player1" && gameEnd == false) {

			Player1.SetActive (false);
			Player2_UI.SetActive (true);
			Wins_UI.SetActive (true);
			gameEnd = true;

		} else if (coll.gameObject.tag == "Player1" && gameEnd == true) {

			Destroy (Player1);

		}

		if (coll.gameObject.tag == "Player2" && gameEnd == false) {

			Player2.SetActive (false);
			Player1_UI.SetActive (true);
			Wins_UI.SetActive (true);
			gameEnd = true;

		} else if (coll.gameObject.tag == "Player2" && gameEnd == true) {

			Destroy(Player2);

		}
	}
}
