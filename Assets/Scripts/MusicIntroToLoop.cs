using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicIntroToLoop : MonoBehaviour
{
    public AudioSource musicIntro;
    public AudioSource musicLoop;
    void Start()
    {
        Init();
    }

    public void Init()
    {
        musicIntro.Play();
        musicLoop.PlayDelayed(musicIntro.clip.length);
    }

    public void StopAll()
    {
        musicIntro.Stop();
        musicLoop.Stop();
    }

}