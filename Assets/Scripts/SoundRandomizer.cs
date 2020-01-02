using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandomizer : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;
  //  public AudioListener audioListener;

    // Start is called before the first frame update
    void Start()
    {
    //    audioListener = GetComponent<AudioListener>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        PlayRandom();
    }
    void PlayRandom()
    {
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();


    }
}