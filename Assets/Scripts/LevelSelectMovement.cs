using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LevelSelectMovement : MonoBehaviour
{
    public int playersAmount = 4;

    public float radius = 0.6f;
    public float translateSpeed = 180.0f;
    public float rotateSpeed = 360.0f;

    float angle = 0.0f;
    [SerializeField]
    Vector3 direction = Vector3.one;
    Quaternion rotation = Quaternion.identity;
    PhotonView pv;

    public Transform limitUp, limitDown;
    Quaternion oldRotation;

    private void OnEnable()
    {
        transform.position = this.gameObject.transform.position;
        pv = this.GetComponent<PhotonView>();
        //if (LobbyConnectionHandler.instance.IsMultiplayerMode) //&& PhotonNetwork.IsMasterClient)
        //{
        //    //this.gameObject.AddComponent<PhotonView>();
        //    this.gameObject.AddComponent<NetworkCharacter>();
        //    List<Component> comp = new List<Component>();
        //    comp.Add(this.gameObject.GetComponent<NetworkCharacter>());
        //    this.GetComponent<PhotonView>().Synchronization = ViewSynchronization.UnreliableOnChange;
        //    this.GetComponent<PhotonView>().ObservedComponents = comp;
        //}
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {


            //List<Component> comp = new List<Component>();
            // comp.Add(this.gameObject.GetComponent<NetworkCharacter>());
            this.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Off;
            this.GetComponent<PhotonView>().ObservedComponents = null;
            this.GetComponent<NetworkMapSelection>().enabled = false;
        }
        else
        {
            pv.Synchronization = ViewSynchronization.UnreliableOnChange;
            List<Component> comp = new List<Component>();
            comp.Add(this.gameObject.GetComponent<NetworkMapSelection>());
            this.GetComponent<PhotonView>().ObservedComponents = comp;
            this.GetComponent<NetworkMapSelection>().enabled = true;
        }
    }

    private void Start()
    {
        ShowLevelTitle.OnLevelIsHovered.AddListener(OnSelectTitleLevel);
    }

    void OnSelectTitleLevel(Transform t)
    {
        Vector3 target = t.position + t.up * (t.position - transform.position).magnitude;

        rotation = Quaternion.LookRotation(target - transform.parent.position, Vector3.up);
        UpdatePositionRotation();
    }

    void Update()
    {
        if (LobbyConnectionHandler.instance.IsMultiplayerMode && pv.IsMine)
        {
            direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));

            for (int i = 0; i < playersAmount; i++)
            {
                float h = Input.GetAxisRaw("P" + (i + 1) + "_Horizontal");
                float v = Input.GetAxisRaw("P" + (i + 1) + "_Vertical");

                bool right = h > .2f;
                bool left = h < -.2f;

                bool up = v > .6f;
                bool down = v < -.6f;

                if (left) TranslateMul(translateSpeed, 0);
                if (right) TranslateMul(-translateSpeed, 0);

                if (up) TranslateMul(0, translateSpeed);
                if (down) TranslateMul(0, -translateSpeed);
            }

            // Rotate with left/right arrows
            if (Input.GetKey(KeyCode.A)) TranslateMul(translateSpeed, 0);
            if (Input.GetKey(KeyCode.D)) TranslateMul(-translateSpeed, 0);

            // Translate forward/backward with up/down arrows
            if (Input.GetKey(KeyCode.W)) TranslateMul(0, translateSpeed);
            if (Input.GetKey(KeyCode.S)) TranslateMul(0, -translateSpeed);

         //   Translate left/ right with A/ D.Bad keys but quick test.
            //if (Input.GetKey(KeyCode.A)) Translate(translateSpeed, 0);
            //if (Input.GetKey(KeyCode.D)) Translate(-translateSpeed, 0);

            UpdatePositionRotation();
        }
        //else if(LobbyConnectionHandler.instance.IsMultiplayerMode && !pv.IsMine)
        //{
        //    direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
        //    Debug.Log("Client End Trying");
        //    for (int i = 0; i < playersAmount; i++)
        //    {
        //        float h = Input.GetAxisRaw("P" + (i + 1) + "_Horizontal");
        //        float v = Input.GetAxisRaw("P" + (i + 1) + "_Vertical");

        //        bool right = h > .2f;
        //        bool left = h < -.2f;

        //        bool up = v > .6f;
        //        bool down = v < -.6f;

        //        if (left) TranslateMul(translateSpeed, 0);
        //        if (right) TranslateMul(-translateSpeed, 0);

        //        if (up) TranslateMul(0, translateSpeed);
        //        if (down) TranslateMul(0, -translateSpeed);
        //    }

        //    // Rotate with left/right arrows
        //    if (Input.GetKey(KeyCode.A)) TranslateMul(translateSpeed, 0);
        //    if (Input.GetKey(KeyCode.D)) TranslateMul(-translateSpeed, 0);

        //    // Translate forward/backward with up/down arrows
        //    if (Input.GetKey(KeyCode.W)) TranslateMul(0, translateSpeed);
        //    if (Input.GetKey(KeyCode.S)) TranslateMul(0, -translateSpeed);

        //    // Translate left/right with A/D. Bad keys but quick test.
        //    //if (Input.GetKey(KeyCode.A)) Translate(translateSpeed, 0);
        //    //if (Input.GetKey(KeyCode.D)) Translate(-translateSpeed, 0);

        //    //   UpdatePositionRotation();
        //}
        else if(!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));

            int leftCount = 0;
            int rightCount = 0;
            int upCount = 0;
            int downCount = 0;

            for (int i = 0; i < playersAmount; i++)
            {
                float h = Input.GetAxisRaw("P" + (i + 1) + "_Horizontal");
                float v = Input.GetAxisRaw("P" + (i + 1) + "_Vertical");

                bool right = h > .2f;
                bool left = h < -.2f;

                bool up = v > .6f;
                bool down = v < -.6f;

                if (left) leftCount++;
                if (right) rightCount++;

                if (up) upCount++;
                if (down) downCount++;
            }

            // Rotate with left/right arrows
            if (Input.GetKey(KeyCode.A)) leftCount++;
            if (Input.GetKey(KeyCode.D)) rightCount++;

            // Translate forward/backward with up/down arrows
            if (Input.GetKey(KeyCode.W)) upCount++;
            if (Input.GetKey(KeyCode.S)) downCount++;

            if (leftCount > 0)  Translate(translateSpeed, 0);
            if (rightCount > 0) Translate(-translateSpeed, 0);
            if (upCount > 0)    Translate(0, translateSpeed);
            if (downCount > 0) Translate(0, -translateSpeed);
            // Translate left/right with A/D. Bad keys but quick test.
            //if (Input.GetKey(KeyCode.A)) Translate(translateSpeed, 0);
            //if (Input.GetKey(KeyCode.D)) Translate(-translateSpeed, 0);

            UpdatePositionRotation();
        }
    }

    void Rotate(float amount)
    {
        angle += amount * Mathf.Deg2Rad * Time.deltaTime;
    }

    void Translate(float x, float y)
    {
        oldRotation = rotation;
        var perpendicular = new Vector3(-direction.y, direction.x);
        var verticalRotation = Quaternion.AngleAxis(y * Time.deltaTime, perpendicular);
        var horizontalRotation = Quaternion.AngleAxis(x * Time.deltaTime, direction);
        rotation *= horizontalRotation * verticalRotation;
    }

    void TranslateMul(float x, float y)
    {
        //if (pv.IsMine)
        //{
        //    var perpendicular = new Vector3(-direction.y, direction.x);
        //    var verticalRotation = Quaternion.AngleAxis(y * Time.deltaTime, perpendicular);
        //    var horizontalRotation = Quaternion.AngleAxis(x * Time.deltaTime, direction);
        //    rotation *= horizontalRotation * verticalRotation;
        //    pv.RPC("ForceMovementFromOtherInstances", RpcTarget.MasterClient, x, y);
        //}
        //else
        {
            pv.RPC("ForceMovementFromOtherInstances", RpcTarget.MasterClient, x, y);
        }

        

    }


    void UpdatePositionRotation()
    {
        var oldPos = transform.localPosition;
        var oldRot = transform.rotation;

        transform.localPosition = rotation * Vector3.forward * radius;
        transform.rotation = rotation * Quaternion.LookRotation(direction, Vector3.forward);

        if (transform.position.y >= limitUp.position.y || transform.position.y <= limitDown.position.y)
        {
            transform.localPosition = oldPos;
            transform.rotation = oldRot;
            rotation = oldRotation;
        }
    }

    [PunRPC]
    void ForceMovementFromOtherInstances(float x, float y)
    {
        oldRotation = rotation;
        var perpendicular = new Vector3(-direction.y, direction.x);
        var verticalRotation = Quaternion.AngleAxis(y * Time.deltaTime, perpendicular);
        var horizontalRotation = Quaternion.AngleAxis(x * Time.deltaTime, direction);
        rotation *= horizontalRotation * verticalRotation;
    }

}