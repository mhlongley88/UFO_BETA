using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public static List<Waypoint> AllWaypoints = new List<Waypoint>();

    // Start is called before the first frame update
    void Awake()
    {
        AllWaypoints.Add(this);
    }

    private void OnDestroy()
    {
        AllWaypoints.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
