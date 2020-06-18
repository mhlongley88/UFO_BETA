using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionWithinBoundaries : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;
    public Vector2 bounds = new Vector2(20, 20);
    public float moveSpeedMultiplier = 1.0f;

    private void OnAnimatorMove()
    {
        if(Time.deltaTime > 0)
        {
            Vector3 v = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = rb.velocity.y;
            rb.velocity = v;
        }
    }
}
