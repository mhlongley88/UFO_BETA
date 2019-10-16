using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossTakeDamage : MonoBehaviour
{
    public LayerMask bulletMask;
    public UnityEvent OnBossTakeDamage = new UnityEvent();

    private void OnCollisionEnter(Collision collision)
    {
        if(GameUtils.LayerMaskContains(collision.gameObject.layer, bulletMask))
        {
            OnBossTakeDamage.Invoke();
        }
    }
}
