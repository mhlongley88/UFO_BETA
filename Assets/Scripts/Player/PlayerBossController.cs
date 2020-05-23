using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBossController : MonoBehaviour
{
    Rigidbody myRigidbody;

    [SerializeField]
    private NormalWeapon normalWeapon;
    [SerializeField]
    private SuperWeapon superWeapon;

    private Weapon currentWeapon;
    public Weapon CurrentWeapon { get { return normalWeapon; } }

    public float maxSpeed = 30.0f;
    public float acceleration = 10.0f;

    private float horizontalInput;
    private float verticalInput;

    Vector2 moveDirection;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void ApplyExternalInput(Vector3 axis, Quaternion lookAt)
    {
        horizontalInput = axis.x;
        verticalInput = axis.z;

      //  transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, Time.smoothDeltaTime * 6.0f);
    }

    private void FixedUpdate()
    {
        moveDirection = Vector2.Lerp(moveDirection, new Vector2(horizontalInput, verticalInput), acceleration * Time.fixedDeltaTime);

        //if (horizontalInput != 0 || verticalInput != 0)
        //    myRigidbody.velocity = (transform.position + new Vector3(moveDirection.x, 0.0f, moveDirection.y) / 2.0f * maxSpeed) - transform.position;
        //else
        //    myRigidbody.velocity *= 0.989f;       
    }

    public void ActivateBeam()
    {
       // beam.SetActive(true);
     //   isAbducting = true;
    }

    public void DeactivateBeam()
    {
      //  beam.SetActive(false);
      //  isAbducting = false;
    }

    public bool IsSuperWeaponReady()
    {
        return false;
    }

    public void tryToBoost()
    {
    }

    public void ToggleSuperWeapon(bool activate)
    {
    }
}
