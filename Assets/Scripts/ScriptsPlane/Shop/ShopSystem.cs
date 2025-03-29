using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public int price;
        public Button buyButton;
        public TMP_Text itemNameText;
        public TMP_Text itemPriceText;
    }

    public ShopItem[] shopItems;
    public TMP_Text coinText;

    private int playerCoins = 1000; // เริ่มต้นมี 1000 coins

    void Start()
    {
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 1000);
        UpdateCoinUI();

        // ตั้งค่า UI ของแต่ละไอเทม
        foreach (var item in shopItems)
        {
            item.itemNameText.text = item.itemName;
            item.itemPriceText.text = item.price.ToString() + " Coins";
            item.buyButton.onClick.AddListener(() => BuyItem(item));
        }
        
    }


    void BuyItem(ShopItem item)
    {
        if (playerCoins >= item.price)
        {
            playerCoins -= item.price;
            UpdateCoinUI();
            Debug.Log("ซื้อ " + item.itemName + " เรียบร้อย!");
            // TODO: เพิ่มไอเทมเข้า Inventory
        }
        else
        {
            Debug.Log("เงินไม่พอ ซื้อ " + item.itemName + " ไม่ได้!");
        }
    }

    void UpdateCoinUI()
    {
        coinText.text = "Coins: " + playerCoins.ToString();
        PlayerPrefs.SetInt("PlayerCoins", playerCoins);
    }
}
