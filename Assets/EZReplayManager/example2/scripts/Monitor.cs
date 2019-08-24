using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Monitor : MonoBehaviour {
	public GameObject stone;
	public int wallMaxX;
	public int wallMaxY;
	public Transform player;
	//is anyone able to move and play?
	private bool gameActive;
	
	public bool isActive() {
		return gameActive;
	}
	
	public void setActive(bool newValue) {
		gameActive = newValue;
	}
	
	void Awake () {
		//if (player.gameObject.active)
		createWall();
	}

	// Use this for initialization
	void Start () {		
		Application.runInBackground = true;
		gameActive = true;

		StartCoroutine(startRecording());

		
	}
	
	private IEnumerator startRecording() {
		yield return new WaitForSeconds(1f);
		EZReplayManager.get.record();
	}
	
	private void createWall()
	{
		GameObject goStones = new GameObject("Stones");
		for (int y = 0; y < wallMaxY; y++) 
		{
			for (int x = 0; x < wallMaxX; x++) 
			 {
				 if (y == wallMaxY-1 && (x == wallMaxX-1))
					 continue;
				 
				 float addition = 0f;
				 if (y % 2 == 0)
					addition = stone.transform.localScale.x / 2;				 
				 
				GameObject thisStone=(GameObject)Instantiate (stone,new Vector3 ((transform.position.x-x*stone.transform.localScale.x)+addition, y*stone.transform.localScale.y, transform.position.z), Quaternion.identity);
				thisStone.transform.parent = goStones.transform;
			}
		}
	}
	
	bool licenseInfoActive = false;
	IEnumerator showLincensInfo(float seconds) {
		licenseInfoActive = true;
		yield return new WaitForSeconds(seconds);
		licenseInfoActive = false;
	}

    bool webglInfoActive = false;
    IEnumerator showWebGLInfo(float seconds) {
        webglInfoActive = true;
        yield return new WaitForSeconds(seconds);
        webglInfoActive = false;
    }


    void  OnGUI (){
		if (EZReplayManager.get.getCurrentMode() == ViewMode.LIVE) {
			if (GUI.Button ( new Rect(20,250,130,20), "Reload scene")) {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
			
	        GUIStyle style = GUI.skin.GetStyle("box");
	        style.fontStyle = FontStyle.Bold;			
			
			if (GUI.Button ( new Rect(20,280,130,20), "Save replay"))
                //EZReplayManager.get.saveToFile("example1.ezr");
                if (EZR_ReflectionLibrary.functionExist("saveToTextFile")) {
                    if (Application.platform != RuntimePlatform.WebGLPlayer) {
                        EZReplayManager.get.SendMessage("saveToTextFile", "example2.ezr", SendMessageOptions.RequireReceiver);
                    } else {
                        StartCoroutine(showWebGLInfo(5f));
                    }
                } else {
					StartCoroutine(showLincensInfo(5f));
				}
			
			if (licenseInfoActive)
				GUI.Box(new Rect(Screen.width / 2 - (650 / 2), Screen.height / 2 - (40 / 2), 650, 40), "To use this functionality, please purchase the full version of EZReplayManager", style);

            if (webglInfoActive)
                GUI.Box(new Rect(Screen.width / 2 - (650 / 2), Screen.height / 2 - (40 / 2), 650, 40), "Saving and loading does not work in WebGL builds", style);

            if (GUI.Button ( new Rect(20,310,130,20), "Load replay"))
					//EZReplayManager.get.loadFromFile("example1.ezr");
				if (EZR_ReflectionLibrary.functionExist("loadFromTextFile")) {
                    if (Application.platform != RuntimePlatform.WebGLPlayer) {
                        EZReplayManager.get.SendMessage("loadFromTextFile", "example2.ezr", SendMessageOptions.RequireReceiver);
                    } else {
                        StartCoroutine(showWebGLInfo(5f));
                    }

				} else {
					StartCoroutine(showLincensInfo(5f));
				}		
			
		}

        if (GUI.Button(new Rect(20, 340, 130, 20), "Toggle reflection")) {
            ReflectionProbe reflProbe = GameObject.Find("ReflectionProbe").GetComponent<ReflectionProbe>();
            if (PlayerPrefs.GetInt("EnableReflection",1) == 1) {
                PlayerPrefs.SetInt("EnableReflection", 0);
                reflProbe.enabled = false;
            } else {
                PlayerPrefs.SetInt("EnableReflection", 1);
                reflProbe.enabled = true;
            }
        }

    }
	
	
}
