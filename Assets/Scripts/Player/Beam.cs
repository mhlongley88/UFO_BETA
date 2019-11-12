using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public float beamForce = 10f;
    public Transform abductionPoint;
    public PlayerController playerController;

    public float scaleModifier = 10f;

    private Vector3 beamDirection;

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("canAbduct"))
        {
            other.isTrigger = true;
            other.GetComponent<Rigidbody>().isKinematic = false;
            beamDirection = (abductionPoint.position - other.transform.position).normalized;
            other.attachedRigidbody.AddForce(
				beamDirection * (beamForce + /*playerController.GetScaleDelta() */ scaleModifier), ForceMode.Force
			);
        }
    }

}
