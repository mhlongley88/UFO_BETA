using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Waypoint[] waypoints;

    public Transform[] spawnPoints;

    private static EnemyManager instance;

    public static EnemyManager Instance
    {
        get
        {
            return instance;
        }
    }
    public void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        waypoints = (Waypoint[])Component.FindObjectsOfTypeAll(typeof(Waypoint));
    }

    public Vector3 GetNextWaypointPosition()
    {
        return waypoints[Random.Range(0, waypoints.Length)].transform.position;
    }


}
