using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public static string[] tags = { "Bullet" };

    private Rigidbody[] childRigidBodies;
    ObjectAbduct[] childsAbducts;
    public GameObject destroyedPFX;

    public void Awake()
    {
        childRigidBodies = GetComponentsInChildren<Rigidbody>();
        childsAbducts = GetComponentsInChildren<ObjectAbduct>();

        for (int i = 0; i < childsAbducts.Length; i++)
        {
            childsAbducts[i].notTrigger = true;
        }
    }


    // public void Start()
    // {
    //     myRigidbody = GetComponent<Rigidbody>();
    // }

    public static bool CheckTag(string tag)
    {
        bool found = false;
        for (int i = 0; i < tags.Length; i++)
        {
            if (tags[i] == tag)
            {
                found = true;
            }
        }
        return found;
    }

    public void Shatter()
    {
        if(destroyedPFX) destroyedPFX.SetActive(true);

        for (int i = 0; i < childsAbducts.Length; i++)
        {
            childsAbducts[i].enabled = true;

        }

        for (int i = 0; i < childRigidBodies.Length; i++)
        {

            childRigidBodies[i].isKinematic = false;
            childRigidBodies[i].transform.parent = null;

        }
       // gameObject.SetActive(false);
    }

}
