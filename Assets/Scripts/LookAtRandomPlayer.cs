using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtRandomPlayer : MonoBehaviour
{
    [SerializeField]
    Transform target;
    public float switchTargetRate = 3.5f;
    float elapsedRate = 0.0f;

    public float delayToStartLooking = 0.0f;
    public float rotationSpeed = 4.0f;

    float delayElapsed = 0.0f;
    void Start()
    {
        delayElapsed = Time.time + delayToStartLooking;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.paused) return;

        if (delayElapsed > Time.time) return;

        var activePlayers = GameManager.Instance.GetActivePlayers();
        if (activePlayers.Count > 0 && (elapsedRate < Time.time || target == null))
        {
            var player = activePlayers[Random.Range(0, activePlayers.Count)];

            target = null;
            if (PlayerManager.Instance.players.ContainsKey(player))
            {
                if (PlayerManager.Instance.players[player].instance != null)
                    target = PlayerManager.Instance.players[player]?.instance?.transform;
            }

            elapsedRate = Time.time + switchTargetRate;
        }

        if (target)
        {
            Quaternion la = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
            Vector3 ea = la.eulerAngles;
            ea.x = ea.z = 0.0f;
            la.eulerAngles = ea;

            transform.rotation = Quaternion.Slerp(transform.rotation, la, Time.smoothDeltaTime * rotationSpeed);

            //transform.LookAt(target);
        }
    }
}
