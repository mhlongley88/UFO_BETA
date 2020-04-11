using System.Collections;
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

    public static int levelStaticInt;

    public static OnLevelTitleSelected OnLevelIsHovered = new OnLevelTitleSelected(); 

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
            PlayerPrefs.SetInt("PlayedLevel" + levelNum, 1);
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            OnLevelIsHovered.Invoke(transform);

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
    }
}
