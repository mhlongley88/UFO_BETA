using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Photon.Pun;
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

        if(this.GetComponent<PhotonView>())
            pv = this.GetComponent<PhotonView>();

        
        if (pv != null && pv.IsMine)
        {
            StartCoroutine(RandomlyPickAnEnemy());
            StartCoroutine(FireCoroutine());
        }
        else if(pv == null)
        {
            StartCoroutine(RandomlyPickAnEnemy());
            StartCoroutine(FireCoroutine());
        }
        
    }
    // Update is called once per frame
    public void Update()
    {
        if(targetTransform != null && lookAtTarget && pv != null && pv.IsMine)
        {
            Quaternion la = Quaternion.LookRotation(targetTransform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, la, Time.smoothDeltaTime * 1.7f);
        }else if (targetTransform != null && lookAtTarget && pv == null)
        {
            Quaternion la = Quaternion.LookRotation(targetTransform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, la, Time.smoothDeltaTime * 1.7f);
        }
    }

    [PunRPC]
    private void Fire()
    {
        
        
            MommaBullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Vector3 direction = transform.position + transform.forward * 5;
            if (!lookAtTarget && targetTransform != null)
                direction = (targetTransform.position);

            bullet.FireBullet(direction - transform.position, coll, bulletDamage, 1.0f, bulletVelocity);
        

        //Debug.DrawLine(transform.position, direction, Color.red, 20.0f);
    }

    [PunRPC]
    private void Fire_RPC(Transform FireTransform)
    {
       // if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            if (pv.IsMine)
            {
                MommaBullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

                Vector3 direction = transform.position + transform.forward * 5;
                if (!lookAtTarget && targetTransform != null)
                    direction = (targetTransform.position);

                bullet.FireBullet(direction - transform.position, coll, bulletDamage, 1.0f, bulletVelocity);
            }
            else
            {
                MommaBullet bullet = Instantiate(bulletPrefab, FireTransform.position, FireTransform.rotation);

                Vector3 direction = FireTransform.position + FireTransform.forward * 5;
                if (!lookAtTarget && targetTransform != null)
                    direction = (targetTransform.position);

                bullet.FireBullet(direction - FireTransform.position, coll, bulletDamage, 1.0f, bulletVelocity);
            }
        }
        

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
                List<Player> activePlayers;// = GameManager.Instance.GetActivePlayers();
                if (LobbyConnectionHandler.instance.IsMultiplayerMode)
                {
                    activePlayers = GameManager.Instance.GetActivePlayersMul(false);
                }else
                {
                    activePlayers = GameManager.Instance.GetActivePlayers();
                }
                var chosenPlayerEnum = activePlayers[Random.Range(0, PlayerManager.Instance.spawnedPlayerDictionary.Count)];
               // if(chosenPlayerEnum != Player.None)
                chosenPlayer = PlayerManager.Instance.players[chosenPlayerEnum];
                targetTransform = PlayerManager.Instance.spawnedPlayerDictionary[chosenPlayerEnum].transform;// == null ? PlayerManager.Instance.spawnedPlayerDictionary[chosenPlayerEnum].transform: null;
            }
        }

        yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
        StartCoroutine(RandomlyPickAnEnemy());
    }
    PhotonView pv;
    private IEnumerator FireCoroutine()
    {
        while(true)
        {

            yield return new WaitForSeconds(Random.Range(minFireWaitTime, maxFireWaitTime));

            if (LobbyConnectionHandler.instance.IsMultiplayerMode)
            {
                
                for (int i = 0; i < volleyCount; i++)
                {
                    pv.RPC("Fire", RpcTarget.All);
                    yield return new WaitForSeconds(fireRate);
                }
            }
            else
            {
                for (int i = 0; i < volleyCount; i++)
                {
                    Fire();
                    yield return new WaitForSeconds(fireRate);
                }
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
                if (LobbyConnectionHandler.instance.IsMultiplayerMode)
                    Destroy(gameObject);
                else
                    this.GetComponent<PhotonView>().RPC("Death", RpcTarget.All);
            }
        }

    }
    
    [PunRPC]
    void Death()
    {
        if (this.GetComponent<PhotonView>().IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
    
}
