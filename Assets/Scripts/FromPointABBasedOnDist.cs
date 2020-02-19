using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromPointABBasedOnDist : MonoBehaviour
{
    public FromPointAB fpAB;
    public Transform a;
    public Transform b;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Vector3 ap = a.position;
        ap += fpAB.dist;
        b.position = ap;
    }
}
