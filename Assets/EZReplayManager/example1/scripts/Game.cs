using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
	private bool activated=true;
	
	void Awake() {
		
		
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(startRecording());
		
	}
	
	private IEnumerator startRecording() {
		yield return new WaitForSeconds(1f);
		EZReplayManager.get.record();
	}	
	
	// Update is called once per frame
	void Update () {
		
		GameObject middleHeli = GameObject.Find("Heli3");
		
		if (middleHeli != null && activated) {
			if (middleHeli.transform.position.x >180f) {
				activated = false;
				EZReplayManager.get.stop();
				EZReplayManager.get.play(0);
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
            if (GUI.Button(new Rect(20, 250, 130, 20), "Reload scene")) {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            GUIStyle style = GUI.skin.GetStyle("box");
            style.fontStyle = FontStyle.Bold;

            if (GUI.Button(new Rect(20, 280, 130, 20), "Save replay"))
                //EZReplayManager.get.saveToFile("example1.ezr");
                if (EZR_ReflectionLibrary.functionExist("saveToTextFile")) {
                    if (Application.platform != RuntimePlatform.WebGLPlayer) {
                        EZReplayManager.get.SendMessage("saveToTextFile", "example1.ezr", SendMessageOptions.RequireReceiver);
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

            if (GUI.Button(new Rect(20, 310, 130, 20), "Load replay"))
                //EZReplayManager.get.loadFromFile("example1.ezr");
                if (EZR_ReflectionLibrary.functionExist("loadFromTextFile")) {
                    if (Application.platform != RuntimePlatform.WebGLPlayer) {
                        EZReplayManager.get.SendMessage("loadFromTextFile", "example1.ezr", SendMessageOptions.RequireReceiver);
                    } else {
                        StartCoroutine(showWebGLInfo(5f));
                    }

                } else {
                    StartCoroutine(showLincensInfo(5f));
                }

        }
    }	
}
