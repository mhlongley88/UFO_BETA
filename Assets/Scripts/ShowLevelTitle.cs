﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

[Serializable]
public class OnLevelTitleSelected : UnityEvent<Transform> { }

public class ShowLevelTitle : MonoBehaviour
{

    public GameObject levelTitle;
    public int levelNum;
    private AudioSource myAudioSource;
    public AudioClip levelHoverSFX;

    public UnityEvent OnGetHovered = new UnityEvent();
    public UnityEvent OnGetUnHovered = new UnityEvent();
    public static int levelStaticInt;

    public static OnLevelTitleSelected OnLevelIsHovered = new OnLevelTitleSelected();

    const string PlayedLevelPrefsKey = "PlayedLevel";

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        //  levelTitle.SetActive(false);

        OnLevelIsHovered.AddListener(DisableTitle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if(levelStaticInt == levelNum)
        {
            UserPrefs.instance.SetInt(PlayedLevelPrefsKey + levelNum, 1);
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            OnLevelIsHovered.Invoke(transform);
            OnGetHovered.Invoke();
            //levelTitle.GetComponent<DOTweenAnimation>().DOPlayById("Appear");
            levelTitle.SetActive(true);
            Debug.Log("hit!");
            levelStaticInt = levelNum;
            myAudioSource.PlayOneShot(levelHoverSFX);
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            DisableTitle(null);
        }
    }

    void DisableTitle(Transform t)
    {
        levelTitle.SetActive(false);
        //levelTitle.GetComponent<DOTweenAnimation>().DOPlayById("Disappear");
        //levelTitle.GetComponent<DOTweenAnimation>().DORewindAllById("Appear");
        levelStaticInt = 0;

        OnGetUnHovered.Invoke();

        if (MainMenuUIManager.Instance.tryTutorialScreen.activeInHierarchy)
            MainMenuUIManager.Instance.tryTutorialScreen.SetActive(false);
    }
}
