using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyOnCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnColliderEnter(Collider coll)
    {

        if(coll.gameObject.tag == "Player")
        {
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
 
    }
}
