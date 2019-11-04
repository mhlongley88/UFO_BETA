﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AirEnemy : Enemy
{
    [Flags]
    public enum EnemyState
    {
        Attacking = 0x01,
        Traveling = 0x02
    }
    //public Animation myAnimation;
    //public string[] animationNames;

    public float maxHealth = 40;
    public float health = 40;
    public LayerMask bulletMask;
    public LayerMask ufoMask;

    [HideInInspector]
    public Transform targetTransform;
    public Animator formationAnimator;
    public Vector2 minMaxSpeedMultiplier = new Vector2(1.0f, 3.0f);
    float speedMultiplier = 1.0f;
    public int volleyCount;
    public float fireRate;
    public float minFireWaitTime;
    public float maxFireWaitTime;
    private EnemyState state = EnemyState.Traveling;
    public MommaBullet bulletPrefab;
    public Transform firePoint;
    public float bulletDamage = -20.0f;
    public GameObject hitFx, deathFx;
    public bool lookAtTarget = true;
    public BoxCollider coll;
    public float bulletVelocity = 120.0f;

    // Start is called before the first frame update
    public void Start()
    {
        health = maxHealth;

        speedMultiplier = Random.Range(minMaxSpeedMultiplier.x, minMaxSpeedMultiplier.y);

        if (speedMultiplier < 1.0f) speedMultiplier = 1.0f;

        state = EnemyState.Traveling;
        formationAnimator = this.GetComponent<Animator>();
        formationAnimator.SetInteger("Formation", Random.Range(0, 3));
        formationAnimator.SetFloat("Speed", speedMultiplier);

        StartCoroutine(RandomlyPickAnEnemy());
        StartCoroutine(FireCoroutine());
    }
    // Update is called once per frame
    public void Update()
    {
        if(targetTransform != null && lookAtTarget)
        {
            Quaternion la = Quaternion.LookRotation(targetTransform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, la, Time.smoothDeltaTime * 1.7f);
        }
    }

    private void Fire()
    {
        MommaBullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Vector3 direction = transform.position + transform.forward * 5;
        if (!lookAtTarget && targetTransform != null)
            direction = (targetTransform.position);

        bullet.FireBullet(direction - transform.position, coll, bulletDamage, 1.0f, bulletVelocity);

        //Debug.DrawLine(transform.position, direction, Color.red, 20.0f);
    }

    PlayerStats chosenPlayer = null;
    IEnumerator RandomlyPickAnEnemy()
    {
        List<GameObject> values = Enumerable.ToList(PlayerManager.Instance.spawnedPlayerDictionary.Values);

        if (values.Count > 0)
        {
            if (chosenPlayer == null || targetTransform == null || (chosenPlayer != null && chosenPlayer.lives < 0))
            {
                var activePlayers = GameManager.Instance.GetActivePlayers();
                var chosenPlayerEnum = activePlayers[Random.Range(0, PlayerManager.Instance.spawnedPlayerDictionary.Count)];
                chosenPlayer = PlayerManager.Instance.players[chosenPlayerEnum];

                targetTransform = PlayerManager.Instance.spawnedPlayerDictionary[chosenPlayerEnum].transform;
            }
        }

        yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
        StartCoroutine(RandomlyPickAnEnemy());
    }

    private IEnumerator FireCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minFireWaitTime, maxFireWaitTime));

            for(int i = 0; i < volleyCount; i++)
            {
                Fire();
                yield return new WaitForSeconds(fireRate);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.gameObject;

        if (GameUtils.LayerMaskContains(obj.layer, ufoMask))
        {
            PlayerController pc = PlayerController.playerControllerByGameObject[obj];
            pc.DoDamage(bulletDamage);
            pc.ApplyForce(collision.contacts[0].point);

            Instantiate(hitFx, gameObject.transform.position, gameObject.transform.rotation);
        }

        if (GameUtils.LayerMaskContains(obj.layer, bulletMask))
        {
            Instantiate(hitFx, collision.contacts[0].point, gameObject.transform.rotation);

            health += obj.GetComponent<Bullet>().HealthDamage;
            if (health <= 0)
            {
                Instantiate(deathFx, transform.position, deathFx.transform.rotation);

                Destroy(gameObject);
            }
        }

    }
}
