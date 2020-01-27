using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicIntroToLoop : MonoBehaviour
{
    public AudioSource musicIntro;
    public AudioSource musicLoop;
    void Start()
    {
        musicIntro.Play();
        musicLoop.PlayDelayed(musicIntro.clip.length);
    }

}