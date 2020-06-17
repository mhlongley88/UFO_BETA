using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPauseHandle : MonoBehaviour
{
    ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (GameManager.Instance.paused)
        {
            if (!ps.isPaused)
                ps.Pause(true);
        }
        else
        {
            if (ps.isPaused)
            {
                //ps.Pause(false);
                ps.Play(true);
            }
        }
    }
}
