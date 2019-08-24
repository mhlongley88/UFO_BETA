using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AveragePosition : MonoBehaviour
{

    Vector3 posOfAll = Vector3.zero;


    [Space(10)]
    public List<Transform> targets = new List<Transform>();

    private Vector3 pos = Vector3.zero;

    public Transform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculatePosition();
    }

    public void CalculatePosition()
    {
        for (int i = 1; i < targets.Count; i++)
        {
            Vector3 pos = targets[i].position;
        }

        myTransform.position = pos / targets.Count;

    }

    public void AddPlayer(Transform t)
    {
        targets.Add(t);
    }

    public void RemovePlayer(Transform t)
    {
        targets.Remove(t);
    }

    }
