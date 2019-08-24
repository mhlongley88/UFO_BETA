using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Helicopter : Enemy
{
    [Flags]
    public enum EnemyState
    {
        Attacking = 0x01,
        Traveling = 0x02
    }
    //public Animation myAnimation;
    //public string[] animationNames;

    public Transform targetTransform;
    private Vector3 targetWaypoint;
    public float maxSpeed;
    public float acceleration;
    public int volleyCount;
    public float fireRate;
    public float minFireWaitTime;
    public float maxFireWaitTime;
    private EnemyState state = EnemyState.Traveling;
    private Rigidbody myRigidbody;


    // Start is called before the first frame update
    public void Start()
    {
        //myAnimation.Play(animationNames[Random.Range(0, animationNames.Length)]);
        List<GameObject> values = Enumerable.ToList(PlayerManager.Instance.spawnedPlayerDictionary.Values);
        if(values.Count > 0)
        {
            targetTransform = values[Random.Range(0, values.Count)].transform;
        }
        targetWaypoint = EnemyManager.Instance.GetNextWaypointPosition();
        state = EnemyState.Traveling;
        myRigidbody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    public void Update()
    {
        if(targetTransform != null)
        {
            transform.LookAt(targetTransform, Vector3.up);
        }
    }

    public void FixedUpdate()
    {
        Vector3 targetVector = targetWaypoint - transform.position;
        if(IsTraveling() && Vector3.Project(myRigidbody.velocity, targetVector.normalized).sqrMagnitude < maxSpeed*maxSpeed)
        {
            myRigidbody.AddForce((targetVector.normalized) * acceleration * Time.fixedDeltaTime, ForceMode.Force);
        }
    }

    public bool IsTraveling()
    {
        return (state & EnemyState.Traveling) != 0;
    }

    private void Fire()
    {

    }

    private IEnumerator FireCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minFireWaitTime, maxFireWaitTime));
            for(int i = 0; i < volleyCount; i++)
            {
                Fire();
                yield return new WaitForSeconds(fireRate);
            }
        }
    }
}
