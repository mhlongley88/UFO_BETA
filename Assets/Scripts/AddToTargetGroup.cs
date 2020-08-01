using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AddToTargetGroup : MonoBehaviour
{
    const float normalWeight = 1.0f;
    const float preferredWeight = 3.0f;
    private CinemachineTargetGroup MyTargetGroup;
    private Transform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        float weight = normalWeight;

        PlayerController playerController;
        if(gameObject.TryGetComponent(out playerController))
        {
            if (PlayerBot.active && !PlayerBot.chosenPlayer.Contains(playerController.player))
            {
                weight = preferredWeight;
            }
        }

        myTransform = this.transform;
        MyTargetGroup = GameObject.Find("CAMERA/TargetGroup1").GetComponent<CinemachineTargetGroup>();
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            MyTargetGroup.AddMember(myTransform, weight, 0f);
        }
        else if(this.GetComponent<Photon.Pun.PhotonView>())
        {
            if (this.GetComponent<Photon.Pun.PhotonView>().IsMine)
            {
                MyTargetGroup.AddMember(myTransform, weight, 0f);
            }
            else
            {
                MyTargetGroup.AddMember(myTransform, weight*0.15f, 0f);
            }
            
        }
    }

    public void RemoveFromTargetGroup()
    {
        MyTargetGroup.RemoveMember(myTransform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
