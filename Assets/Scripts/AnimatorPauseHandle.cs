using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPauseHandle : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.Instance.paused)
            animator.speed = 0;
        else
            animator.speed = 1;
    }
}
