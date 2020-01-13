﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameMenuController : MonoBehaviour {

    private static GameMenuController instance;
    public static GameMenuController Instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject menuCanvasObj;

    private float previousTimescale;
    private bool menuOpen;
  //  private GameObject gM;

    // Use this for initialization
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        //  DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape) && !menuOpen)
        //{
        //    ButtonToggleMenu();
        //}

    }

    public void SetState(bool state)
    {
        menuCanvasObj.SetActive(state);
        menuOpen = state;
    }

    public void ButtonToggleMenu()
    {
        GameManager.Instance.TogglePause();
        //if (!menuOpen)
        //{
        //  //  previousTimescale = Time.timeScale;//getting the current timescale
        // //   Time.timeScale = 0;//Pausing time
        //    menuCanvasObj.SetActive(true);

        //    menuOpen = true;
        //}
        //else
        //{
        //  //  Time.timeScale = previousTimescale;//unpausing time

        //    menuOpen = false;
        //}
    }

    //for testing/Debugging.
    public void DeletePlayerprefs()
    {
        PlayerPrefs.DeleteKey("graphicsPrefsSaved");
        PlayerPrefs.DeleteKey("FPSToggle");
        PlayerPrefs.DeleteKey("graphicsSlider");
        PlayerPrefs.DeleteKey("antiAliasSlider");
        PlayerPrefs.DeleteKey("shadowResolutionSlider");
        PlayerPrefs.DeleteKey("textureQualitySlider");
        PlayerPrefs.DeleteKey("anisotropicModeSlider");
        PlayerPrefs.DeleteKey("anisotropicLevelSlider");
        PlayerPrefs.DeleteKey("wantedResolutionX");
        PlayerPrefs.DeleteKey("wantedResolutionY");
        PlayerPrefs.DeleteKey("windowedModeToggle");
        PlayerPrefs.DeleteKey("vSyncToggle");

        PlayerPrefs.DeleteKey("audioPrefsSaved");
        PlayerPrefs.DeleteKey("mainVolumeF");
        PlayerPrefs.DeleteKey("fxVolumeF");
        PlayerPrefs.DeleteKey("musicVolumeF");
    }

    public void ButtonQuitGame()
    {
        Application.Quit();
    }
    public void ButtonReturnToMainMenu()
    {
      //  gM.GetComponent<GameManager>().ReturnMainMenu();
    }
}
