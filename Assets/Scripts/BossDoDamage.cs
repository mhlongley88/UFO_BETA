using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoDamage : MonoBehaviour
{
    public LayerMask ufoMask;
    public float damage = -25.0f;

    public GameObject BulletHitPFX;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;

        if(GameUtils.LayerMaskContains(obj.layer, ufoMask))
        {
            PlayerController pc = PlayerController.playerControllerByGameObject[obj];
            pc.DoDamage(damage);
            pc.ApplyForce(collision.contacts[0].point);

            Instantiate(BulletHitPFX, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}
