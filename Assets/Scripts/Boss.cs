using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Boss : MonoBehaviour
{
    public Animator animator;
    public Animator motionTrackAnimator;
    public LookAtPlayer lookAtPlayer;
    public GameObject cmVCam6;
    public Transform annunakiObj;

    public int maxHealth = 40;
    public int health = 40;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        
    }

    public void OnTakeDamage()
    {
        Debug.Log("Boss took damage");

        health -= 2;
        if(health <= 0)
        {
            animator.SetTrigger("Die");
            cmVCam6.SetActive(true);

            GameObject.Find("CAMERA/TargetGroup1").GetComponent<CinemachineTargetGroup>().RemoveMember(annunakiObj);
            motionTrackAnimator.enabled = false;
            lookAtPlayer.enabled = false;

            Debug.Log("Boss is Dead");
        }
    }
}
