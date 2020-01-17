using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtRandomPlayer : MonoBehaviour
{
    [SerializeField]
    Transform target;
    public float switchTargetRate = 3.5f;
    float elapsedRate = 0.0f;

    public float rotationSpeed = 4.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var activePlayers = GameManager.Instance.GetActivePlayers();
        if (activePlayers.Count > 0 && elapsedRate < Time.time)
        {
            var player = activePlayers[Random.Range(0, activePlayers.Count)];
            target = PlayerManager.Instance.players[player].instance.transform;

            elapsedRate = Time.time + switchTargetRate;
        }

        if (target)
        {
            Quaternion la = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, la, Time.smoothDeltaTime * rotationSpeed);

            //transform.LookAt(target);
        }
    }
}
