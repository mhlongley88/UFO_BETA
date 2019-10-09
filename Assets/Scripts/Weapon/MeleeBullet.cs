using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeBullet : MonoBehaviour
{
    private float healthDamage;
    protected float scaleDamage;

    public Rigidbody myRigidbody;
    public Collider myCollider;

    //public Vector3 forceVector = Vector3.forward;

   // [SerializeField]
    //private float lifeTime = 10f;

    [SerializeField]
    private GameObject BulletHitPFX;

    private Collider parentObject;

    public float HealthDamage;

    public bool destroyOnCollision = true;

    public void Start()
    {
     /*   if(lifeTime >= 0)
        {
            Destroy(this.gameObject, lifeTime);
        }*/
    }

    public virtual void Punch(Vector3 direction, Collider parentObject, float healthDamage, float scaleDamage, float velocity)
    {
        this.parentObject = parentObject;
        Physics.IgnoreCollision(myCollider, parentObject, true);
        myCollider.enabled = true;
        myRigidbody.AddForce(direction.normalized * velocity, ForceMode.VelocityChange);
        this.healthDamage = healthDamage;
        this.scaleDamage = scaleDamage;

    }


    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider == parentObject)
        {
            Physics.IgnoreCollision(myCollider, coll.collider);
        }
        else
        {
            if (gameObject.tag == "BulletFist" && coll.gameObject.tag == "Player" && BulletHitPFX != null)
            {

                Instantiate(BulletHitPFX, gameObject.transform.position, gameObject.transform.rotation);
                coll.rigidbody.AddExplosionForce(140.0f, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), 100.0f);
            }
            if(destroyOnCollision)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
       // Gizmos.DrawLine(transform.position, transform.TransformPoint(forceVector.normalized * 2f));
    }
}

