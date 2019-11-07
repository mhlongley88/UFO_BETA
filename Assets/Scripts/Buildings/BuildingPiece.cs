using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPiece : MonoBehaviour
{
    public Destructable buildingParent;
    ObjectAbduct oa;

    Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        TryGetComponent(out oa);

        if(oa)
        {
            oa.enabled = false;
        }
    }

    private void Update()
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
