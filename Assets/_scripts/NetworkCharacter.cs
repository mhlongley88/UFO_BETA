using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class NetworkCharacter : MonoBehaviour, IPunObservable, IPunInstantiateMagicCallback
{
    PhotonView pv;

    Vector3 toPos, toScale;
    Quaternion toRot;

    public float lerpSpeed = 10;
    public float lerpRotSpeed = 10;

    public bool checkOwnerConnStatus = false;
    bool ownerDisconnected = false;
    float disconnectTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        pv = this.GetComponent<PhotonView>();
        //lerpSpeed = 5f;
        //lerpRotSpeed = 20f;
    }

    public void StartCheckingConnection()
    {
        StartCoroutine(CheckOwnerConnection());
    }

    IEnumerator CheckOwnerConnection()
    {
        yield return new WaitForSeconds(1f);

        if (disconnectTimer > 1f)
        {
            
            if (LobbyConnectionHandler.instance.IsNextMasterCandidate())
            {
                ownerDisconnected = false;
                disconnectTimer = 0;
                PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                //this.GetComponent<PlayerBot>().allowClientControl = true;
                //StartCoroutine(DisableBotClientControl(this.GetComponent<PlayerBot>(), 5f));
            }
            
            //pv.TransferOwnership(PhotonNetwork.LocalPlayer);
        }
        else
        {
            ownerDisconnected = true;
        }
        //Debug.Log("Checking Conn" + disconnectTimer);
        StartCoroutine(CheckOwnerConnection());
    }

    //IEnumerator DisableBotClientControl(PlayerBot bot, float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    bot.allowClientControl = false;
    //}

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine)
        {
            //if(checkOwnerConnStatus)
            //{
            //    if (ownerDisconnected)
            //    {
            //        disconnectTimer += Time.deltaTime;
                    
            //    }
            //    else
            //    {
            //        disconnectTimer = 0;
            //    }
            //}
            this.transform.position = Vector3.Lerp(this.transform.position, toPos, Time.smoothDeltaTime * lerpSpeed);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, toRot, Time.smoothDeltaTime * lerpRotSpeed);
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, toScale, Time.smoothDeltaTime * lerpSpeed);
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            toPos = (Vector3)stream.ReceiveNext();
            toRot = (Quaternion)stream.ReceiveNext();
            toScale = (Vector3)stream.ReceiveNext();
            //ownerDisconnected = false;
        }
        else
        {
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);
            stream.SendNext(this.transform.localScale);
        }
    }

    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject;
    }

}
