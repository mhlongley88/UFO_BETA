using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossTakeDamage : MonoBehaviour
{
    public LayerMask bulletMask;
    public UnityEvent OnBossTakeDamage = new UnityEvent();
    public GameObject hitFx;

    private void OnCollisionEnter(Collision collision)
    {
        if(GameUtils.LayerMaskContains(collision.gameObject.layer, bulletMask))
        {
            OnBossTakeDamage.Invoke();
            Instantiate(hitFx, collision.contacts[0].point, gameObject.transform.rotation);
        }
    }
}
