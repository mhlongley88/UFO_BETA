using UnityEngine;
using System.Collections;

public class ChangeCam : MonoBehaviour {

    public Camera LiveCam;
    public Camera ReplayCam;

    void Awake() {
        LiveCam.enabled = true;
        ReplayCam.enabled = false;
    }

    public void __EZR_live_ready() {

        if (GetComponent<Camera>() == LiveCam) {
            GetComponent<Camera>().enabled = true;
        }

        if (GetComponent<Camera>() == ReplayCam) {
            GetComponent<Camera>().enabled = false;
        }

    }

    public void __EZR_replay_ready() {


        if (GetComponent<Camera>() == ReplayCam) {
            GetComponent<Camera>().enabled = true;
        }

        if (GetComponent<Camera>() == LiveCam) {
            GetComponent<Camera>().enabled = false;
        }


    }

}
