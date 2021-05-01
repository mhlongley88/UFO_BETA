using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EventItem : MonoBehaviour
{
    public enum ItemType
    {
        Gems, Character, Map, RewardBox
    }
    public UnityEvent redeemReward;
    public ItemType itemType;
    public int GemsAmount, CharacterId, SkinId, MapId;
    public GameObject lockedObject;
    public string redeemedString;
    public bool isBrawlPassItem;
    public bool isFinalReward;

    private Animator animator;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
    }
    public void TryRedeem()
    {

        MainMenuUIManager.Instance.touchMenuUI.RewardScreen.isBrawlReward = isBrawlPassItem;

        redeemReward.Invoke();
    }

    public void MakeInteractable()
    {
        lockedObject.SetActive(false);
        
    }
    public void LockIt()
    {
        lockedObject.SetActive(true);
    }
    public void TogglePickUpAnim(bool en)
    {
        animator.SetBool("en", en);
    }
}
