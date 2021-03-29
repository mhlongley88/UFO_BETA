using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraTransitions;
using DG.Tweening;
using System;

public class InstantReplay : MonoBehaviour
{

    public PlayerStats[] players;

    //public EZReplayManager ezrm;
    public GameObject vCam1;
    //   public GameObject replayCam;

    public GameObject mainCam;

    public GameObject ResultsScreen;
    private PlayerManager playerManager;
    //public CameraTransitionsAssistant camToReplay;
    public GameObject camToResults;

    public GameObject rpa;
    public GameObject conqueredUiFx;

    public GameObject UIObject;

    private GameObject[] playerObjects;

    private AudioSource myAudioSource;
    public AudioClip transitionSFX;

    // public GameObject trackMe;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GameObject.Find("/LevelObjects/CAMERA/AudioManager").GetComponent<AudioSource>();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        //vCam1.SetActive(true);
        //     replayCam.SetActive(false);
        //   EZReplayManager.get.mark4Recording(trackMe);
        //    EZReplayManager.get.record();
    }

    bool startedRecording = false;
    // Update is called once per frame
    void Update()
    {
        var playerCount = PlayerManager.Instance.players.Count;
        int playersLeft = 0;
        int playerLeftWithOneLife = 0;
        foreach (Player i in PlayerManager.Instance.players.Keys)
        {
            int lives = PlayerManager.Instance.players[i].lives;
            if (lives > 0)
            {
                playersLeft++;
                if (lives == 1)
                    playerLeftWithOneLife++;
            }
        }

        if (playersLeft == 2 && playerLeftWithOneLife > 0 && !startedRecording)
        {
            //   ezrm.record();
            startedRecording = true;
            Debug.Log("should be recording");
        }

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    //KillCamRec();
        //    ezrm.record();
        //    Debug.Log("should be recording");
        //}

        //if (GameManager.Instance.gameOver)
        //{
        //    DOVirtual.DelayedCall(2.0f, () =>
        //    {
        //        playerManager.gameHasEnded = true;
        //        camToResults.SetActive(true);
        //        ResultsScreen.SetActive(true);
        //    });
        //    // startedRecording = false;
        //}

        // if (playerManager.gameHasEnded)
        //{
        // if (Input.GetButtonDown("CharacterConfirm") /*|| ezrm.getCurrentPosition() >= ezrm.maxPositions*/)
        //{
        // ezrm.stop();

        // Transition + Show Results Screeen
        //{
        //  camToResults.ExecuteTransition();
        //ezrm.gameObject.SetActive(false);
        // mainCam.SetActive(false);
        // ResultsScreen.SetActive(true);
        // playerObjects = GameObject.FindGameObjectsWithTag("Player");
        //playerObjects[0].SetActive(false);
        //  playerObjects[1].SetActive(false);
        // }
        // }
        //}

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Debug.Log("should be stopped");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // camToResults.ExecuteTransition();
            // //ezrm.gameObject.SetActive(false);
            //// mainCam.SetActive(false);
            // ResultsScreen.SetActive(true);
            // playerObjects = GameObject.FindGameObjectsWithTag("Player");
            // playerObjects[0].SetActive(false);
            // playerObjects[1].SetActive(false);
        }
    }

    void KillCamRec()
    {
        //Debug.Log("should be recoriding KillCam");
        // ezrm.record();
        //new WaitForSeconds(5.0f);
        // ezrm.stop();
    }

}
