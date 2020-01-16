using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionBetween : MonoBehaviour
{
    public Collider coll1;
    public Collider coll2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Physics.IgnoreCollision(coll1, coll2, true);
    }
}
