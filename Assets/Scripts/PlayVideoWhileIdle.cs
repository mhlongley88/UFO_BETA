using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayVideoWhileIdle : MonoBehaviour
{
    public static bool playingIdleVideo = false;

    public VideoPlayer videoPlayer;
    public GameObject rawImagePlaying;

    public float maxSecondsIdle = 4.0f;
    float timeElapsed = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        rawImagePlaying.SetActive(false);
        timeElapsed = Time.time + maxSecondsIdle;

        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        timeElapsed = Time.time + maxSecondsIdle;
    }

    IEnumerator SetFalseToPlayingVideoAfterAFrame()
    {
        yield return null;
        playingIdleVideo = false;
    }

    void Update()
    {
        if (Input.anyKeyDown || Input.GetButtonDown("Restart"))
        {
            timeElapsed = Time.time + maxSecondsIdle;
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Stop();


                StartCoroutine(SetFalseToPlayingVideoAfterAFrame());

                rawImagePlaying.SetActive(false);
            }
        }

        if(MainMenuUIManager.Instance.currentMenu != MainMenuUIManager.Menu.Splash)
        {
            timeElapsed = Time.time + maxSecondsIdle;
            return;
        }

        if (videoPlayer.isPlaying) return;

        if(timeElapsed < Time.time)
        {
            videoPlayer.Play();
            rawImagePlaying.SetActive(true);

            playingIdleVideo = true;
        }
    }
}
