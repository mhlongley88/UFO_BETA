using UnityEngine;

public class MommaBullet : Bullet
{


    public BabyBullet[] babyBulletArray;


    public override void FireBullet(Vector3 direction, Collider parentUFO, float healthDamage, float scaleDamage, float velocity)
    {
        transform.LookAt(transform.position + direction, Vector3.up);
        for (int i = 0; i < babyBulletArray.Length; i++)
        {
            babyBulletArray[i].FireBullet(direction, parentUFO, healthDamage, scaleDamage, velocity);
        }
        base.FireBullet(direction, parentUFO, healthDamage, scaleDamage, velocity);

    }


}
