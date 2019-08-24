using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbductCache : MonoBehaviour
{

    private List<GameObject> abductedObjects = new List<GameObject>();

    public void AddObject(GameObject obj)
    {
        abductedObjects.Add(obj);
    }

}
