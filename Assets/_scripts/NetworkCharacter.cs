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

    public float lerpSpeed = 15;

    // Start is called before the first frame update
    void Start()
    {
        pv = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, toPos, Time.smoothDeltaTime * lerpSpeed);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, toRot, Time.smoothDeltaTime * lerpSpeed);
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, toScale, Time.smoothDeltaTime * lerpSpeed);
        }
        else
        {

        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            toPos = (Vector3)stream.ReceiveNext();
            toRot = (Quaternion)stream.ReceiveNext();
            toScale = (Vector3)stream.ReceiveNext();
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
