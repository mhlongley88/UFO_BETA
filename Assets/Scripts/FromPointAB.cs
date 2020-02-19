using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromPointAB : MonoBehaviour
{
    public Transform ina;
    public Transform inb;
    public Vector3 dist;

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
        Gizmos.color = Color.red;

        Gizmos.DrawLine(ina.position, inb.position);

        dist = inb.position - ina.position;
    }
}
