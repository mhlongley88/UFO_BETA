using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreThisColliderWithParentPlayer : MonoBehaviour
{
    [HideInInspector]
    public Collider theCollider;

    void Awake()
    {
        theCollider = GetComponent<Collider>();
    }
}
