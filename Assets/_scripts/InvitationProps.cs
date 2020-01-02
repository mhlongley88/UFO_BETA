using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InvitationProps : MonoBehaviour
{
    public Text invitationByName, timerText;
    public Button AcceptInviteButton;
    public string InvitationBy, RoomId;
    float timer = 10;
    bool isExpired = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InvitationWait());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator InvitationWait()
    {
        yield return new WaitForSeconds(1);
        if(!isExpired && timer > 0)
        {
            timer--;
            timerText.text = timer.ToString();
            StartCoroutine(InvitationWait());
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }


    private void OnDisable()
    {
        Destroy(this.gameObject);
    }



}
