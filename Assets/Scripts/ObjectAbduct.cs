using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAbduct : MonoBehaviour
{

    public float speed = 20.0f;
    private Rigidbody rb;

    private bool canAbduct;
    private PlayerController playerController;
    public float playerScaleAdd = 0.3f;
    public float playerEnergyAdd = 0.1f;
    public GameObject AbductedPFX;

    public static bool dropped;

    public bool notTrigger = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(!notTrigger) rb.isKinematic = true;
        canAbduct = false;
        //abductionPoint = null;
        StartCoroutine(ItemHasBeenDropped(this.gameObject, 2.0f));
    }



    void OnTriggerEnter(Collider other)
    {
        //if (notTrigger) return;

        if (other.CompareTag("ItemCollector"))
        {
            //rb.isKinematic = false;
            playerController = other.gameObject.GetComponentInParent<PlayerController>();
            //            ufo.GetComponentInChildren<PlayerAbductCache>().AddObject(this.gameObject);

            playerController.AddAbductedObject(this.gameObject, playerScaleAdd, playerEnergyAdd);
            Instantiate(AbductedPFX, gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!notTrigger) return;

        if (other.gameObject.CompareTag("ItemCollector"))
        {
            //rb.isKinematic = false;
            playerController = other.gameObject.GetComponentInParent<PlayerController>();
            //            ufo.GetComponentInChildren<PlayerAbductCache>().AddObject(this.gameObject);

            playerController.AddAbductedObject(this.gameObject, playerScaleAdd, playerEnergyAdd);
            Instantiate(AbductedPFX, gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    public static IEnumerator ItemHasBeenDropped(GameObject go, float waitTime)
    {

        //go = this.gameObject;
        //this.canAbduct == false;
        go.gameObject.layer = 17;
        yield return new WaitForSeconds(waitTime);
        go.gameObject.layer = 9;
        //this.canAbduct == true;

    }

}