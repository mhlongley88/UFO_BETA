using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionWithinBoundaries : MonoBehaviour
{
    public Collider characterCollider;
    public Collider boundariesCollider;

    public Animator animator;
    public Vector2 bounds = new Vector2(20, 20);
    public float moveSpeedMultiplier = 1.0f;

    bool inside = true;

    void Update()
    {
        inside = boundariesCollider.bounds.Contains(characterCollider.bounds.center);
    }

    private void OnAnimatorMove()
    {
        if(Time.deltaTime > 0 && inside)
        {
            Vector3 deltaPosition = animator.deltaPosition;

            Vector3 v = transform.position;
            v += (deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
            v.y = transform.position.y;

            transform.position = deltaPosition;

            transform.position = v;
        }
    }
}
