using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class camMoveScript : MonoBehaviour
{
   // private float waypoints;

    public GameObject camObject;
   // public GameObject moveCamOffset;
    public GameObject moveHere;

   // private Vector3 moveVector;

    // Start is called before the first frame update
    void Start()
    {
      //  moveVector = Vector3 Transform.TransformVector(moveHere.position.x, moveHere.position.y, moveHere.position.z);
      // camObject.transform.DOLocalPath(Vector3[] waypoints, float duration, PathType pathType = Linear, PathMode pathMode = Full3D, int resolution = 10, Color gizmoColor = null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            camObject.transform.position = moveHere.transform.position;
            Debug.Log("hit " + this.gameObject.name);
        }
    }
}
