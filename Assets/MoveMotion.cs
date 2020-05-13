using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMotion : MonoBehaviour
{
    public Animator animator;
    public float speed = 1.0f;

    private void OnAnimatorMove() 
    {
        animator.applyRootMotion = false;
        if (Time.deltaTime > 0)
		{
			Vector3 v = (animator.deltaPosition * speed) / Time.deltaTime;

            transform.position += v * Time.deltaTime;
		}
    }
}
