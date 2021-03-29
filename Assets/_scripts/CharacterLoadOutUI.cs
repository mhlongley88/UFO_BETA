using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterLoadOutUI : MonoBehaviour
{
    public TextMeshProUGUI priceText;
    
    public GameObject selectedCharacter_LoadOut, LevelLockedObj, BuyButtonLock;
    public Image DamageFG, RateOfFireFG, AccuracyFG, currLevelProgressFG, NormalWeaponImg, SuperWeaponImg;
    public Button BuyButton;
    public TextMeshProUGUI ufoNameText, ufoNameSubText, ufoLevelText, ufoDetailsText;
    public Transform characterModelContainer;
    public int levelUpPrice = 1000;
    public StoreItem[] storeItemCharacters;
    public List<StoreItemHolder> Cart;
    
    int currentCharacterIndex;
    // Start is called before the first frame update
    void Awake()
    {
        Cart = new List<StoreItemHolder>();
    }

    void RemoveAllChildren()
    {
        int count = characterModelContainer.childCount;
        for(int i = 0; i < count; i++)
        {
            Destroy(characterModelContainer.transform.GetChild(i).gameObject);
        }
    }

    public void DestroyCurrentSelection_OnDisable()
    {
        ResetTryProps();
        Destroy(selectedCharacter_LoadOut);
    }
    
    public void AddToCart(StoreItemHolder item)
    {
        Cart.Add(item);
    }

    public void AddUFOToCart(int id)
    {
        StoreItemHolder holder = ConvertToItemHolder(storeItemCharacters[id]);
        

        AddToCart(holder);
    }

    public StoreItemHolder ConvertToItemHolder(StoreItem item)
    {
        StoreItemHolder holder = new StoreItemHolder();
        holder.itemId = item.itemId;
        holder.requiredGems = item.requiredGems;
        holder.skin_characterId = item.skin_characterId;
        holder.type = item.type;

        return holder;
    }

    public void RemoveUFOFromCart()
    {
        StoreItemHolder skinItem = null;
        foreach (StoreItemHolder item in Cart)
        {
            if (item.type == StoreItem.ItemType.Character)
            {
                skinItem = item;
            }
        }

        RemoveFromCart(skinItem);
    }

    //public void AddSkinToCart(int id)
    //{
    //    AddToCart(storeItemCharacters[id]);
    //}

    public void RemoveSkinFromCart()
    {
        StoreItemHolder skinItem = null;
        foreach(StoreItemHolder item in Cart)
        {
            if(item.type == StoreItem.ItemType.Skin)
            {
                skinItem = item;
            }
        }
        RemoveFromCart(skinItem);
    }

    public void RemoveFromCart(StoreItemHolder item)
    {
        Cart.Remove(item);
    }

    public int GetTotalPrice()
    {
        int sum = 0;
        foreach(StoreItemHolder item in Cart)
        {
            //Debug.Log((int)item.requiredGems);
            sum += (int)item.requiredGems;
        }

        return sum;
    }

    public void EmptyCart()
    {
        Cart.Clear();
    }

    public void DisplayCharacterInfo_LoadOutScreen(int id)
    {
        RemoveAllChildren();
        currentCharacterIndex = id;// GameManager.Instance.selectedCharacterIndex;
        Destroy(selectedCharacter_LoadOut);

        selectedCharacter_LoadOut = Instantiate(GameManager.Instance.Characters[id].characterModel, Vector3.zero, Quaternion.identity, characterModelContainer);
        selectedCharacter_LoadOut.transform.localPosition = Vector3.zero;
        selectedCharacter_LoadOut.transform.localScale = Vector3.one;
        selectedCharacter_LoadOut.transform.localEulerAngles = Vector3.zero;

        

        var info = selectedCharacter_LoadOut.GetComponent<CharacterLevelSelectInfo>();
        info.UpdateValues(GameManager.Instance.GetUfoAttribute(id)/*UserPrefs.instance.GetUFOProps().ufoData.Find(item => item.ufoIndex == currentCharacterIndex)*/);
        DamageFG.fillAmount = info.Damage;
        RateOfFireFG.fillAmount = info.RateOfFire;
        AccuracyFG.fillAmount = info.Accuracy;
        ufoNameText.text = info.Name;
        ufoNameSubText.text = info.SubName;
        ufoDetailsText.text = info.Details;
        NormalWeaponImg.sprite = info.WeaponType;
        SuperWeaponImg.sprite = info.SpecialWeapon;

        UFOAttributes attr = GameManager.Instance.GetUfoAttribute(id);
        currLevelProgressFG.fillAmount = attr.ufoXP;
        ufoLevelText.text = attr.ufoLevel.ToString();

        info.currentSkinId = attr.currSkinId;
        info.ActivateSkin();
    }

    public void DisplayCharacterInfo_LoadOut_StoreScreen(int id)
    {
        RemoveAllChildren();
        currentCharacterIndex = id;
        Destroy(selectedCharacter_LoadOut);
        selectedCharacter_LoadOut = Instantiate(GameManager.Instance.Characters[currentCharacterIndex].characterModel, Vector3.zero, Quaternion.identity, characterModelContainer);
        selectedCharacter_LoadOut.transform.localPosition = Vector3.zero;
        selectedCharacter_LoadOut.transform.localScale = Vector3.one;
        selectedCharacter_LoadOut.transform.localEulerAngles = Vector3.zero;



        var info = selectedCharacter_LoadOut.GetComponent<CharacterLevelSelectInfo>();
        info.UpdateValues(GameManager.Instance.GetUfoAttribute(id)/*UserPrefs.instance.GetUFOProps().ufoData.Find(item => item.ufoIndex == id)*/);
        DamageFG.fillAmount = info.Damage;
        RateOfFireFG.fillAmount = info.RateOfFire;
        AccuracyFG.fillAmount = info.Accuracy;
        ufoNameText.text = info.Name;
        ufoNameSubText.text = info.SubName;
        ufoDetailsText.text = info.Details;
        NormalWeaponImg.sprite = info.WeaponType;
        SuperWeaponImg.sprite = info.SpecialWeapon;

        UFOAttributes attr = GameManager.Instance.GetUfoAttribute(id);
        currLevelProgressFG.fillAmount = attr.ufoXP;
        ufoLevelText.text = attr.ufoLevel.ToString();

        //if (!attr.isUnlocked)
        //{
        //    StoreItemHolder item = new StoreItemHolder();
        //    item = GameManager.Instance.GetTryProps();
        //    item.type = StoreItem.ItemType.CharacterAndSkin;
        //    item.requiredGems += attr.priceGems;

        //    GameManager.Instance.SetTryProps(item);
        //}

        UpdatePriceUI();
        //priceText.text = GameManager.Instance.GetTryProps().requiredGems.ToString();
        //MainMenuUIManager.Instance.HoldCharacterChoiceTemporarily(player, selectedCharacterIndex);
    }

    void UpdatePriceUI()
    {
        
        priceText.text = GetTotalPrice().ToString();
        if(GetTotalPrice() == 0)
        {
            BuyButtonLock.SetActive(true);
        }
        else
        {
            BuyButtonLock.SetActive(false);
        }
    }

    void UpdateLoadoutUI()
    {

    }

    public void EnableSkin(int skinId)
    {
        var info = selectedCharacter_LoadOut.GetComponent<CharacterLevelSelectInfo>();

        info.currentSkinId = skinId;
        info.ActivateSkin();

        info.ActivateSkin_LoadOutStore(skinId);

    }

    public void SelectCurrentCharacter()
    {
        GameManager.Instance.selectedCharacterIndex = currentCharacterIndex;
        
    }

    public void ResetTryProps()
    {
        GameManager.Instance.ResetTryProps();
    }

    public void PurchaseSelectedItem_Onclick()
    {

        //StoreItemHolder item = GameManager.Instance.GetTryProps();
        if (GameManager.Instance.GetGems() < GetTotalPrice()) {

            // Not enough gems -- Show something!

            return;
        }

        foreach (StoreItemHolder item in Cart)
        {
            if (item.type == StoreItem.ItemType.Character/* && !GameManager.Instance.GetUfoAttribute(item.itemId).isUnlocked*/)
            {
                //Debug.Log("purchasing character");
                GameManager.Instance.UnlockUFO_Purchase(item.itemId, (int)item.requiredGems);
            }
            else if (item.type == StoreItem.ItemType.Skin/* && !GameManager.Instance.GetUfoAttribute(item.skin_characterId).ownedSkinIds.Contains(item.itemId)*/)
            {
                //Debug.Log("purchasing skin");
                GameManager.Instance.PurchaseSkin(item.itemId, item.skin_characterId, (int)item.requiredGems);
            }
            //else if (item.type == StoreItem.ItemType.CharacterAndSkin/* && !GameManager.Instance.GetUfoAttribute(item.skin_characterId).ownedSkinIds.Contains(item.itemId)*/)
            //{
            //    GameManager.Instance.PurchaseSkin(item.itemId, item.skin_characterId, (int)item.requiredGems);
            //    GameManager.Instance.UnlockUFO_Purchase(item.skin_characterId, 0);
            //}
        }

        EmptyCart();
        GameManager.Instance.ResetTryProps();

        MainMenuUIManager.Instance.touchMenuUI.characterLoadOutStore.gameObject.SetActive(false);
        MainMenuUIManager.Instance.touchMenuUI.MainHub.SetActive(true);
        MainMenuUIManager.Instance.touchMenuUI.SpawnCharacterMainHub();
    }

    public void TryButton_OnClick()
    {
        //Load tutorial 
    }


    public void CharacterLevelUpViaCoins()
    {
        //Debug.Log(GameManager.Instance.GetCoins() < levelUpPrice || MaxLevelReached());
        if (GameManager.Instance.GetCoins() < levelUpPrice || MaxLevelReached()) return;
        UFOAttributes attr = GameManager.Instance.GetUfoAttribute(currentCharacterIndex);
        attr.ufoXP += 1f;// UnityEngine.Random.Range((float)(lowerBound/ upperBound), 1);

        GameManager.Instance.SetCoins((GameManager.Instance.GetCoins() - levelUpPrice));

        GameManager.Instance.SetUfoAttribute(currentCharacterIndex, attr);
        MainMenuUIManager.Instance.touchMenuUI.CharacterLevelUp(attr);
        //SpawnSelectedCharacter(GameManager.Instance.Characters[GameManager.Instance.LocalPlayerId].characterModel, GameManager.Instance.LocalPlayerId);
        MainMenuUIManager.Instance.touchMenuUI.characterLoadOut.DisplayCharacterInfo_LoadOutScreen(currentCharacterIndex);
    }

    bool MaxLevelReached()
    {
        UFOAttributes attr = GameManager.Instance.GetUfoAttribute(currentCharacterIndex);
        if (attr.Accuracy < 1 || attr.Damage < 1 || attr.RateOfFire < 1)
            return false;
        else
            return true;
    }

    public void SetTryPropsForTutorial()
    {
        UFOAttributes attr = GameManager.Instance.GetUfoAttribute(currentCharacterIndex);
        StoreItemHolder itemHolder = new StoreItemHolder();

        int characterIndex=0, skinIndex=-1;
        StoreItemHolder skinHolder = null, characterHolder = null;
        foreach(StoreItemHolder item in Cart)
        {
            if(item.type == StoreItem.ItemType.Character)
            {
                characterHolder = item;
            }
            else if (item.type == StoreItem.ItemType.Skin)
            {
                skinHolder = item;
            }
        }

        if(skinHolder != null)
        {
            characterIndex = skinHolder.skin_characterId;
            skinIndex = skinHolder.itemId;

            
        }
        else if (skinHolder != null)
        {
            characterIndex = skinHolder.itemId;
            skinIndex = -1;

            
        }
        itemHolder.requiredGems = attr.Skins[skinIndex].priceGems;
        itemHolder.type = StoreItem.ItemType.Skin;
        itemHolder.itemId = skinIndex;
        itemHolder.skin_characterId = currentCharacterIndex;
        GameManager.Instance.SetTryProps(itemHolder);
    }

    public void NextSkin()
    {
        var info = selectedCharacter_LoadOut.GetComponent<CharacterLevelSelectInfo>();
        UFOAttributes attr = GameManager.Instance.GetUfoAttribute(currentCharacterIndex);
        if(++info.currentSkinId >= info.Skins.Length)
        {
            info.currentSkinId = -1;
        }

        if ((info.currentSkinId != -1 &&  attr.Skins[info.currentSkinId].isUnlocked) || info.currentSkinId == -1)
        {
            info.ActivateSkin();
            GameManager.Instance.SetCurrentSkin(info.currentSkinId, currentCharacterIndex);
        }
        else 
        {
            NextSkin();
        }
    }

    public void PrevSkin()
    {
        var info = selectedCharacter_LoadOut.GetComponent<CharacterLevelSelectInfo>();
        UFOAttributes attr = GameManager.Instance.GetUfoAttribute(currentCharacterIndex);
        if (--info.currentSkinId < -1)
        {
            info.currentSkinId = info.Skins.Length;
        }

        info.ActivateSkin();

        //
        if ((info.currentSkinId != -1 && attr.Skins[info.currentSkinId].isUnlocked) || info.currentSkinId == -1)
        {
            info.ActivateSkin();
            GameManager.Instance.SetCurrentSkin(info.currentSkinId, currentCharacterIndex);
        }
        else
        {
            PrevSkin();
        }

    }

    public void NextSkin_Store()
    {
        var info = selectedCharacter_LoadOut.GetComponent<CharacterLevelSelectInfo>();
        RemoveSkinFromCart();
        //Debug.Log(currentCharacterIndex);
        UFOAttributes attr = GameManager.Instance.GetUfoAttribute(currentCharacterIndex);
        if (++info.currentSkinId >= info.Skins.Length)
        {
            info.currentSkinId = -1;
        }

        info.ActivateSkin();
        int characterPrice = attr.isUnlocked ? 0 : attr.priceGems;

        if ((info.currentSkinId != -1))
        {

            StoreItemHolder item = new StoreItemHolder();
            item.requiredGems = attr.Skins[info.currentSkinId].priceGems;
            item.type = StoreItem.ItemType.Skin;
            item.skin_characterId = currentCharacterIndex;
            item.itemId = info.currentSkinId;

            AddToCart(item);
            GameManager.Instance.SetTryProps(item);
            

        }
        
        UpdatePriceUI();
    }

    public void PrevSkin_Store()
    {
        var info = selectedCharacter_LoadOut.GetComponent<CharacterLevelSelectInfo>();
        RemoveSkinFromCart();

        UFOAttributes attr = GameManager.Instance.GetUfoAttribute(currentCharacterIndex);
        if (--info.currentSkinId < -1)
        {
            info.currentSkinId = info.Skins.Length-1;
        }

        info.ActivateSkin();
        Debug.Log(info.currentSkinId);
        //
        if ((info.currentSkinId != -1))
        {
            StoreItemHolder item = new StoreItemHolder();
            item.requiredGems = attr.Skins[info.currentSkinId].priceGems;
            item.type = StoreItem.ItemType.Skin;
            item.skin_characterId = currentCharacterIndex;
            item.itemId = info.currentSkinId;

            AddToCart(item);
            GameManager.Instance.SetTryProps(item);
            
        }
        
        UpdatePriceUI();
    }

    public void SaveSkinChoice()
    {
        var info = selectedCharacter_LoadOut.GetComponent<CharacterLevelSelectInfo>();
        GameManager.Instance.SetCurrentSkin(info.currentSkinId, currentCharacterIndex);
    }
}
