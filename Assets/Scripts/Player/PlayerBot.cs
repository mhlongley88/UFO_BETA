using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBot : MonoBehaviour
{
    PlayerController playerController;
    public static Player chosenPlayer;
    public static Transform adversaryTransform;
    public static bool active;

    Vector3 destination;

    bool moving = true;

    float movingRate = 0.0f;
    float shootingRate = 0.0f;

    bool followingPlayer;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        chosenPlayer = playerController.player;

        destination = transform.position;
        playerController.allowLocalProcessInput = false;
    }

    private void OnDestroy()
    {
        //GameManager.Instance.RemovePlayerFromGame(chosenPlayer);
    }

    void Update()
    {
        if(movingRate < Time.time)
        {
            moving = !moving;

            movingRate = Time.time + (moving ? Random.Range(2.7f, 6) : Random.Range(1.5f, 3.0f));

            if(moving)
                followingPlayer = Random.value > 0.45f;
        }

        var adversaryPlayer = GameManager.Instance.GetActivePlayers().Find(it => it != chosenPlayer);
        var adversaryObject = PlayerManager.Instance.players[adversaryPlayer].instance;

        if (moving)
        {
            if ((destination - transform.position).magnitude < 1)
                FindNewPoint();
        }

        Quaternion lookDir = Quaternion.LookRotation(destination - transform.position, Vector3.up);
        if (adversaryObject)
        {
            lookDir = Quaternion.LookRotation(adversaryObject.transform.position - transform.position, Vector3.up);
            if (followingPlayer)
                destination = adversaryObject.transform.position;
        }

        playerController.ApplyExternalInput(moving ? (destination - transform.position).normalized : Vector3.zero, lookDir);

        if(adversaryObject && shootingRate < Time.time)
        {
            playerController.CurrentWeapon.UpdateShootDirection(transform.forward);
            playerController.CurrentWeapon.Fire();

            shootingRate = Time.time + Random.Range(0.25f, 1.4f);

            if (Random.value > 0.55f)
                shootingRate = Time.time + Random.Range(0.08f, 0.2f);
        }
    }

    Vector3[] dirs = new Vector3[4] { Vector3.right, Vector3.left, Vector3.forward, Vector3.back };
    void FindNewPoint()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, dirs[Random.Range(0, 4)], out hit, 10000.0f, GameManager.Instance.boundaryMask))
        {
            destination = (hit.point + hit.normal) * Random.Range(0.6f, 1.0f);
        }
    }
}
