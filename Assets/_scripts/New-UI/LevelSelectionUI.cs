using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSelectionUI : MonoBehaviour
{
    public int levelIndex;
    public bool isUnlocked;
    public GameObject LockedPanel;
    public Image SelectionOutline, LevelImg;
    public int priceGems;
    public Vector3 ScrollViewPos;
    public string LevelUnlockStr;


    public LayoutElement layoutElement;

    private void Awake()
    {
        layoutElement = this.GetComponent<LayoutElement>();
    }
    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (UserPrefs.instance.GetBool(LevelUnlockStr))
        {
            LockedPanel.SetActive(false);
        }
        else
        {
            LockedPanel.SetActive(true);
        }
    }

    public void PurchaseLevel()
    {
        if (GameManager.Instance.GetGems() >= priceGems && !UserPrefs.instance.GetBool(LevelUnlockStr))
        {
            UserPrefs.instance.SetBool(LevelUnlockStr, true);
            GameManager.Instance.AddGems(-priceGems);
        }
        UpdateUI();
    }
}
