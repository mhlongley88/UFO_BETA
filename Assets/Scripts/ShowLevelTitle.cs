using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShowLevelTitle : MonoBehaviour
{

    public GameObject levelTitle;
    public int levelNum;
    private AudioSource myAudioSource;
    public AudioClip levelHoverSFX;

    public static int levelStaticInt;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
      //  levelTitle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
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
            levelTitle.SetActive(false);
            //levelTitle.GetComponent<DOTweenAnimation>().DOPlayById("Disappear");
            //levelTitle.GetComponent<DOTweenAnimation>().DORewindAllById("Appear");
            levelStaticInt = 0;
        }
    }
}
