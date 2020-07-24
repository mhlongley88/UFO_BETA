using Cinemachine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Boss : MonoBehaviour
{
    public static bool hadJustBeenDefeated;
    public static bool defeatedBefore;

    public static Boss instance;

    public bool showCongratulationsScreenAfter = false;

    public Animator animator;
    public Animator motionTrackAnimator;
    public LookAtPlayer lookAtPlayer;
    public GameObject cmVCam6;
    public Transform annunakiObj;
    public Image lifeBar;
    public AudioSource bossDeathSFX;
    public AudioSource bossLOOP;
    public UnityEvent onDie = new UnityEvent();
    public bool invincible = false;
    public int maxHealth = 40;
    public int health = 40;
    public float delayToShowDeathAnim = 4.0f;
    public bool addExtraHealth = false;
    public int extraHealth = 100;
    public int addExtraHealthWhenNumPlayersBigger = 2;

    bool checkedExtraHealth = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;

        if (LevelUnlockCheck.IsUnlockedByBoss(ShowLevelTitle.levelStaticInt))
        {
            defeatedBefore = true;
        }
        else
            defeatedBefore = false;
    }

    void Update()
    {
        if(!checkedExtraHealth)
        {
            if (addExtraHealth && GameManager.Instance.GetActivePlayers().Count > addExtraHealthWhenNumPlayersBigger && !PlayerBot.active)
            {
                maxHealth += extraHealth;
                health = maxHealth;
            }

            checkedExtraHealth = true;
        }

        if(lifeBar) lifeBar.fillAmount = (float)health / (float)maxHealth;
    }

    public void OnTakeDamage(float v)
    {
        OnTakeDamage();
    }

    public void OnTakeDamage()
    {
     //   Debug.Log("Boss took damage");
        if (health <= 0 || invincible) return;

        health -= 2;
        CheckHealth();
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            health = 0;

            if (animator) animator.SetTrigger("Die");
            if (cmVCam6 != null) cmVCam6.SetActive(true);

            if (annunakiObj != null)
                GameObject.Find("CAMERA/TargetGroup1").GetComponent<CinemachineTargetGroup>().RemoveMember(annunakiObj);

            if (motionTrackAnimator) motionTrackAnimator.enabled = false;
            if (lookAtPlayer) lookAtPlayer.enabled = false;

            Debug.Log("Boss is Dead");
            if (bossDeathSFX != null) bossDeathSFX.Play();
            if (bossLOOP != null) bossLOOP.Stop();

            onDie.Invoke();

            if (ShowLevelTitle.levelStaticInt > 0)
            {
                // Register Boss had just been defeated
                if(showCongratulationsScreenAfter) hadJustBeenDefeated = true;

                LevelUnlockCheck.UnlockByBoss(ShowLevelTitle.levelStaticInt);
                PlayerManager.Instance.BossHasDied();
            }
        }
    }
}
 