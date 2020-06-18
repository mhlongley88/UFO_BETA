using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePauseHandle : MonoBehaviour
{
    AudioSource[] sources;
    bool valuePaused;

    // Start is called before the first frame update
    void Awake()
    {
        sources = GetComponentsInChildren<AudioSource>(true);

        valuePaused = GameManager.Instance.paused;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.paused != valuePaused)
        {
            valuePaused = GameManager.Instance.paused;

            for (int i = 0; i < sources.Length; i++)
            {
                var src = sources[i];

                if (valuePaused)
                    src.Pause();
                else
                    src.Play();
            }
        }
    }
}
