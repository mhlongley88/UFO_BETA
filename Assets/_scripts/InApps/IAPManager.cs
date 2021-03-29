using System;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Step 1 create your products

    public string RemoveAds = "remove_ads";
    public string Gems150 = "Gems_150";
    public string Gems500 = "Gems_500";
    public string Gems2500 = "Gems_2500";
    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Step 2 choose if your product is a consumable or non consumable
        builder.AddProduct(RemoveAds, ProductType.NonConsumable);
        builder.AddProduct(Gems150, ProductType.Consumable);
        builder.AddProduct(Gems500, ProductType.Consumable);
        builder.AddProduct(Gems2500, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    //Step 3 Create methods
    public void BuyRemoveAds()
    {
        BuyProductID(RemoveAds);
    }
    public void BuyGems(int id)
    {
        switch (id)
        {
            case 150:
                //gems-150
                BuyProductID(Gems150);
                GameManager.Instance.AddGems(150); // Remove this line when InApps work on Mobile
                break;
            case 500:
                //gems-500
                BuyProductID(Gems500);
                GameManager.Instance.AddGems(500); // Remove this line when InApps work on Mobile
                break;
            case 2500:
                //gems-2500
                BuyProductID(Gems2500);
                GameManager.Instance.AddGems(2500); // Remove this line when InApps work on Mobile
                break;
        }
        
    }

    public void BuyCoins(int id)
    {
        switch (id)
        {
            case 0:
                //150 coins for 20 Gems
                if (GameManager.Instance.GetGems() >= 20)
                {
                    GameManager.Instance.AddGems(-20);
                    GameManager.Instance.AddCoins(150);
                }
                else
                {
                    //Display Not Enough Coins
                }
                break;
            case 1:
                //500 coins for 60 Gems
                if (GameManager.Instance.GetGems() >= 60)
                {
                    GameManager.Instance.AddGems(-60);
                    GameManager.Instance.AddCoins(500);
                }
                else
                {
                    //Display Not Enough Coins
                }
                break;
            case 2:
                //2500 coins for 280 Gems
                if (GameManager.Instance.GetGems() >= 280)
                {
                    GameManager.Instance.AddGems(-280);
                    GameManager.Instance.AddCoins(2500);
                }
                else
                {
                    //Display Not Enough Coins
                }
                break;
        }

    }

    public void BuyCharacter(int id, int characterPrice)
    {
        
        
    }

    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, RemoveAds, StringComparison.Ordinal))
        {
            Debug.Log("Remove Ads purchased");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Gems150, StringComparison.Ordinal))
        {
            GameManager.Instance.AddGems(150);
            Debug.Log("Gems += 150");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Gems500, StringComparison.Ordinal))
        {
            GameManager.Instance.AddGems(500);
            Debug.Log("Gems += 500");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Gems2500, StringComparison.Ordinal))
        {
            GameManager.Instance.AddGems(2500);
            Debug.Log("Gems += 2500");
        }
        else
        {
            GameManager.Instance.AddGems(150); // For testing on Mobile -- Till InApps are linked to proper account
            Debug.Log("Purchase Failed");
        }
        return PurchaseProcessingResult.Complete;
    }










    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
        TestSingleton();
    }

    void Start()
    {
        if (m_StoreController == null) { InitializePurchasing(); }
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;

        extensions.GetExtension<IAppleExtensions>().RestoreTransactions(result => {
            if (result)
            {
                // This does not mean anything was restored,
                // merely that the restoration process succeeded.
            }
            else
            {
                // Restoration failed.
            }
        });
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}