using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    private float healthDamage;
    protected float scaleDamage;

    public Rigidbody myRigidbody;
    public Collider myCollider;

    public Vector3 forceVector = Vector3.forward;

    [SerializeField]
    private float lifeTime = 10f;

    [SerializeField]
    private GameObject BulletHitPFX;

    private Collider parentUFO;

    public float HealthDamage { get => healthDamage; }

    public bool destroyOnCollision = true;

    public void Start()
    {
        if(lifeTime >= 0)
        {
            Destroy(this.gameObject, lifeTime);
        }
    }

    public virtual void FireBullet(Vector3 direction, Collider parentUFO, float healthDamage, float scaleDamage, float velocity)
    {
        this.parentUFO = parentUFO;
        Physics.IgnoreCollision(myCollider, parentUFO, true);
        myCollider.enabled = true;
        myRigidbody.AddForce(direction.normalized * velocity, ForceMode.VelocityChange);
        this.healthDamage = healthDamage;
        this.scaleDamage = scaleDamage;

    }


    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider == parentUFO)
        {
            Physics.IgnoreCollision(myCollider, coll.collider);
        }
        else
        {
            if (gameObject.tag == "Bullet" && coll.gameObject.tag == "Player" && BulletHitPFX != null)
            {

                Instantiate(BulletHitPFX, gameObject.transform.position, gameObject.transform.rotation);
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
        Gizmos.DrawLine(transform.position, transform.TransformPoint(forceVector.normalized * 2f));
    }
}

