using UnityEngine;

public class MeleeBulletTop : MeleeBullet
{

    public BabyBullet[] babyBulletArray;

    private void Update()
    {
       
    }

     public override void Punch(Vector3 direction, Collider parentObject, float healthDamage, float scaleDamage, float velocity)
     {
         transform.LookAt(transform.position + direction, Vector3.up);
         for (int i = 0; i < babyBulletArray.Length; i++)
         {
             babyBulletArray[i].FireBullet(direction, parentObject, healthDamage, scaleDamage, velocity);
         }
         base.Punch(direction, parentObject, healthDamage, scaleDamage, velocity);

     }

   /* private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "BulletFist")
        {

        }
    }*/

}
