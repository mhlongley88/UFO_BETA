using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    public enum ItemType
    {
        Gem, Coin, Character, Stage, Skin, CharacterAndSkin
    }
    public ItemType type;
    public int itemId;
    public int skin_characterId;
    public float requiredGems;
    public string LevelUnlockStr;
    public GameObject purchasedPanel;

    private void OnEnable()
    {
        UpdateUI();   
    }

    void UpdateUI()
    {
        if (type != ItemType.Coin && type != ItemType.Gem)
            IsAlreadyPurchased();
    }

    void EnableCharacterLoadOut_Store()
    {
        
        GameManager.Instance.SetTryProps(this);
        if(type == ItemType.Character)
        {
            MainMenuUIManager.Instance.touchMenuUI.SpawnCharacterLoadOut_Store(itemId, this);
        }
        else if (type == ItemType.Skin)
        {
            //Debug.Log(skin_characterId + "---" + itemId);
            
            MainMenuUIManager.Instance.touchMenuUI.SpawnCharacterLoadOut_Store(skin_characterId, itemId, this);
        }
    }

    public void IsAlreadyPurchased()
    {
        bool isPurchased = false;
        
        if (type == ItemType.Character && GameManager.Instance.GetUfoAttribute(itemId).isUnlocked)
        {
             isPurchased = true;
        }else if(type == ItemType.Skin && GameManager.Instance.GetUfoAttribute(skin_characterId).Skins[itemId].isUnlocked)
        {
             isPurchased = true;
        }else if(type == ItemType.Stage && UserPrefs.instance.GetBool(LevelUnlockStr))
        {
            isPurchased = true;
        }

        if (isPurchased) {
            purchasedPanel.SetActive(true);
        }
        else
        {
            purchasedPanel.SetActive(false);
        }
    }

    public void BuyItem_Skin()
    {
        //Debug.Log(!GameManager.Instance.GetUfoAttribute(skin_characterId).Skins[itemId].isUnlocked);
        if (/*GameManager.Instance.GetGems() >= requiredGems &&*/ !GameManager.Instance.GetUfoAttribute(skin_characterId).Skins[itemId].isUnlocked)
            EnableCharacterLoadOut_Store();
        //if (GameManager.Instance.GetGems() >= requiredGems)
        //{
        //    GameManager.Instance.PurchaseSkin(itemId, skin_characterId, (int)requiredGems);
        //}
        //UpdateUI();
    }

    public void BuyItem_Character()
    {
        if(/*GameManager.Instance.GetGems() >= requiredGems &&*/ !GameManager.Instance.GetUfoAttribute(itemId).isUnlocked)
            EnableCharacterLoadOut_Store();
        //if (GameManager.Instance.GetGems() >= requiredGems)
        //{
        //    GameManager.Instance.UnlockUFO_Purchase(itemId, (int)requiredGems);
        //}
        //UpdateUI();
    }

    public void BuyItem_Stage()
    {
        //
        if (GameManager.Instance.GetGems() >= requiredGems && !UserPrefs.instance.GetBool(LevelUnlockStr))
        {
            UserPrefs.instance.SetBool(LevelUnlockStr, true);
            GameManager.Instance.AddGems(-(int)requiredGems);
        }
        UpdateUI();
    }

    public void BuyItem_Gems(int amt)
    {
        IAPManager.instance.BuyGems(amt);
    }

    public void BuyItem_Coins(int amt)
    {
        if (GameManager.Instance.GetGems() >= requiredGems)
        {
            GameManager.Instance.AddGems(-(int)requiredGems);
            GameManager.Instance.AddCoins(amt);
        }
        else
        {
            //Display Not Enough Coins
        }
    }
}

public class StoreItemHolder
{
    public StoreItem.ItemType type;
    public int itemId;
    public int skin_characterId;
    public float requiredGems;
    public string LevelUnlockStr;
}