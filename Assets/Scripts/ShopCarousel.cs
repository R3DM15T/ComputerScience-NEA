using TMPro;
using UnityEngine;

using static ShopCarousel;

public class ShopCarousel : MonoBehaviour
{
    public enum CosmeticType
    {
        Skin
    }

    [System.Serializable]
    public class CosmeticItem
    {
        public string itemName;
        public int price;
        public int saveID;
    }
    public CosmeticType cosmeticType;
    public CosmeticItem[] items;
    [SerializeField] private CosmeticManager previewCosmeticManager;
    [SerializeField] private CosmeticManager playerCosmeticManager;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI buttonText;

    private int currentIndex = 0;


    private void Start()
    {
        ResetToFirstItem();
    }


    public void NextItem()
    {
        if (items.Length == 0)
        {
            return;
        }
        currentIndex = currentIndex + 1;
        if (currentIndex >= items.Length)
        {
            currentIndex = 0;
        }
        ShowItem();
    }


    public void PreviousItem()
    {
        if (items.Length == 0)
        {
            return;
        }

        currentIndex = currentIndex - 1;

        if (currentIndex < 0)
        {
            currentIndex = items.Length - 1;
        }

        ShowItem();
    }



    public void ShowItem()
    {
        if (items.Length == 0)
        {
            return;
        }

        CosmeticItem item = items[currentIndex];
        itemNameText.text = item.itemName;
        priceText.text = item.price + "G";

        if (previewCosmeticManager != null)
        {
            previewCosmeticManager.PreviewSkin(item.saveID);
        }
        UpdateButton();
    }



    private string GetOwnedKey()
    {
        CosmeticItem item = items[currentIndex];
        return "Skin_" + item.saveID;
    }


    private bool IsOwned()
    {
        if (items[currentIndex].saveID == 0)
        {
            return true;
        }

        return PlayerPrefs.GetInt(GetOwnedKey(), 0) == 1;
    }



    private bool IsEquipped()
    {
        int saveID = items[currentIndex].saveID;
        return PlayerPrefs.GetInt("EquippedSkin", 0) == saveID;
    }


    private void UpdateButton()
    {
        if (IsEquipped())
        {
            buttonText.text = "EQUIPPED";
        }
        else if (IsOwned())
        {
            buttonText.text = "EQUIP";
        }
        else
        {
            buttonText.text = "BUY";
        }
    }


    public void OnMainButtonPressed()
    {
        if (items.Length == 0)
        {
            return;
        }

        CosmeticItem item = items[currentIndex];
        if (IsEquipped())
        {
            return;
        }

        if (IsOwned())
        {
            EquipCurrentItem();
            return;
        }


        PlayerMoney money = FindObjectOfType<PlayerMoney>();

        if (money == null)
        {
            Debug.LogError("the shop cant find the playermoney.");
            return;
        }


        if (money.TryRemoveMoney(item.price))
        {
            string ownedKey = GetOwnedKey();

            PlayerPrefs.SetInt(ownedKey, 1);
            PlayerPrefs.Save();

            EquipCurrentItem();
        }
        else
        {
            buttonText.text = "NOT ENOUGH G";

            CancelInvoke(nameof(RefreshButton));
            Invoke(nameof(RefreshButton), 1f);
        }
    }



    private void EquipCurrentItem()
    {
        CosmeticItem item = items[currentIndex];


        PlayerPrefs.SetInt("EquippedSkin", item.saveID);
        PlayerPrefs.Save();

        if (playerCosmeticManager != null)
        {
            playerCosmeticManager.EquipSkin(item.saveID);
        }


        if (previewCosmeticManager != null)
        {
            previewCosmeticManager.PreviewSkin(item.saveID);
        }


        UpdateButton();
    }


    private void RefreshButton()
    {
        UpdateButton();
    }

    public void ResetToFirstItem()
    {
        currentIndex = 0;
        ShowItem();
    }

}
