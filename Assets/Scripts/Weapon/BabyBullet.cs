
using UnityEngine;

public class BabyBullet : Bullet
{

    public override void FireBullet(Vector3 direction, Collider parentUFO, float healthDamage, float scaleDamage, float velocity)
    {
        transform.parent = null;
        base.FireBullet(transform.forward, parentUFO, healthDamage, scaleDamage, velocity);
    }

}