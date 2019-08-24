﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPiece : MonoBehaviour
{

    public Destructable buildingParent;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnCollisionEnter(Collision otherCollision)
    {
        if (Destructable.CheckTag(otherCollision.collider.tag))
        {
            if (buildingParent != null)
            {
                buildingParent.Shatter();
            }
        }
    }
}
