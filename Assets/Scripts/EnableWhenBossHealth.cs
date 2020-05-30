using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class EnableWhenBossHealth : MonoBehaviour
{
    public Boss boss;
    public GameObject objectToEnable;
    public float percentage = 0.5f;
    public UnityEvent OnProcess = new UnityEvent();
    public Transform resetBossPosTo;

    Vector3 firstPos;
    Quaternion firstRot;
    Vector3 firstScale;

    private void OnEnable()
    {
        firstPos = boss.transform.position;
        firstRot = boss.transform.rotation;
        firstScale = boss.transform.localScale;
    }

    void Update()
    {
        if (boss.health <= boss.maxHealth * percentage)
        {
            boss.invincible = true;
            boss.gameObject.GetComponent<PlayerBot>().enabled = false;

            boss.gameObject.GetComponent<PlayerController>().ToggleSuperWeapon(false);

            var seq = DOTween.Sequence();
            seq.Append(boss.transform.DOMove(firstPos, 2.8f));
            seq.Join(boss.transform.DORotateQuaternion(firstRot, 2.8f));
         //   seq.Join(boss.transform.DOScale(resetBossPosTo.localScale, 2.8f));
            seq.AppendCallback(() =>
            {
                objectToEnable.SetActive(true);
                OnProcess.Invoke();

                boss.invincible = false;
                boss.gameObject.GetComponent<PlayerBot>().enabled = true;
            });

            enabled = false;
        }
    }
}
