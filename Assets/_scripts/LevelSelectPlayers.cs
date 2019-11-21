using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LevelSelectPlayers : MonoBehaviour
{
    public PhotonView pv;
    public Player myPlayer;
    // Start is called before the first frame update
    void Start()
    {
        pv = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void SyncTransformsOnOtherInstances()
    {
        if(pv.IsMine)
        {
            switch (myPlayer)
            {
                case Player.One:
                    pv.RPC("SyncTransforms", RpcTarget.All, 1);
                    break;
                case Player.Two:
                    pv.RPC("SyncTransforms", RpcTarget.All, 2);
                    break;
                case Player.Three:
                    pv.RPC("SyncTransforms", RpcTarget.All, 3);
                    break;
                case Player.Four:
                    pv.RPC("SyncTransforms", RpcTarget.All, 4);
                    break;
                case Player.None:
                    break;
            }

            


        }
    }

    [PunRPC]
    void SyncTransforms(int n)
    {

        Player p = Player.None;
        switch (n)
        {
            case 1:
                p = Player.One;
                break;
            case 2:
                p = Player.Two;
                break;
            case 3:
                p = Player.Three;
                break;
            case 4:
                p = Player.Four;
                break;
            case 0:
                p = Player.None;
                break;
        }

        switch (p)
        {
            case Player.One:
                this.transform.SetParent(MainMenuUIManager.Instance.levelSelectCharacters.GetComponent<LevelSelectCharacters>().p1);
                this.transform.localPosition = MainMenuUIManager.Instance.levelSelectCharacters.GetComponent<LevelSelectCharacters>().p1.transform.localPosition;
                this.transform.localRotation = MainMenuUIManager.Instance.levelSelectCharacters.GetComponent<LevelSelectCharacters>().p1.transform.localRotation;
                this.transform.localScale = Vector3.one;
                break;
            case Player.Two:
                this.transform.SetParent(MainMenuUIManager.Instance.levelSelectCharacters.GetComponent<LevelSelectCharacters>().p2);
                this.transform.localPosition = MainMenuUIManager.Instance.levelSelectCharacters.GetComponent<LevelSelectCharacters>().p1.transform.localPosition;
                this.transform.localRotation = MainMenuUIManager.Instance.levelSelectCharacters.GetComponent<LevelSelectCharacters>().p1.transform.localRotation;
                this.transform.localScale = Vector3.one;
                break;
            case Player.Three:
                this.transform.SetParent(MainMenuUIManager.Instance.levelSelectCharacters.GetComponent<LevelSelectCharacters>().p3);
                this.transform.localPosition = MainMenuUIManager.Instance.levelSelectCharacters.GetComponent<LevelSelectCharacters>().p1.transform.localPosition;
                this.transform.localRotation = MainMenuUIManager.Instance.levelSelectCharacters.GetComponent<LevelSelectCharacters>().p1.transform.localRotation;
                this.transform.localScale = Vector3.one;
                break;
            case Player.Four:
                this.transform.SetParent(MainMenuUIManager.Instance.levelSelectCharacters.GetComponent<LevelSelectCharacters>().p4);
                this.transform.localPosition = MainMenuUIManager.Instance.levelSelectCharacters.GetComponent<LevelSelectCharacters>().p1.transform.localPosition;
                this.transform.localRotation = MainMenuUIManager.Instance.levelSelectCharacters.GetComponent<LevelSelectCharacters>().p1.transform.localRotation;
                this.transform.localScale = Vector3.one;
                break;
            case Player.None:
                //this.transform.SetParent(MainMenuUIManager.Instance.levelSelectCharacters.GetComponent<LevelSelectCharacters>().p1);
                break;
        }
        this.transform.SetParent(MainMenuUIManager.Instance.levelSelectCharacters.GetComponent<LevelSelectCharacters>().p1);
    }
}
