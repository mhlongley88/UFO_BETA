using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Boss : MonoBehaviour
{
    public static Boss instance;

    public Animator animator;
    public Animator motionTrackAnimator;
    public LookAtPlayer lookAtPlayer;
    public GameObject cmVCam6;
    public Transform annunakiObj;
    public Image lifeBar;
    public AudioSource bossDeathSFX;
    public AudioSource bossLOOP;
    public UnityEvent onDie = new UnityEvent();

    public int maxHealth = 40;
    public int health = 40;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        lifeBar.fillAmount = (float)health / (float)maxHealth;
    }

    public void OnTakeDamage()
    {
     //   Debug.Log("Boss took damage");
        if (health <= 0) return;

        health -= 2;
        if(health <= 0)
        {
            health = 0;

            if(animator) animator.SetTrigger("Die");
            cmVCam6.SetActive(true);

            GameObject.Find("CAMERA/TargetGroup1").GetComponent<CinemachineTargetGroup>().RemoveMember(annunakiObj);
            if(motionTrackAnimator) motionTrackAnimator.enabled = false;
            if(lookAtPlayer) lookAtPlayer.enabled = false;

            Debug.Log("Boss is Dead");
            bossDeathSFX.Play();
            bossLOOP.Stop();

            onDie.Invoke();

            if (ShowLevelTitle.levelStaticInt > 0)
            {
                LevelUnlockCheck.UnlockByBoss(ShowLevelTitle.levelStaticInt);
                PlayerManager.Instance.BossHasDied();
            }
        }
    }
}
