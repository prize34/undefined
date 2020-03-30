using UnityEngine;
using Undefined.Items;
using Undefined.Items.Shop;
using System.Collections.Generic;

public class HUDShop : MonoBehaviour {

        #region Variables

    [Header("Shop Menu")]
    [SerializeField] private Transform itemsTransform;
    [SerializeField] private GameObject itemsPrefab;

    [Header("Item Details")]
    [SerializeField] private HUDShopItemDetails itemDetails;

    // Script side variables
    private ShopDetails myDetails;
    private PlayerInventory inventory;

    private bool emptyShop;

    private int selectedItem;
    private List<int> deletedItems = new List<int>();

        #endregion

    void Start() {

        inventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
        selectedItem = -1;

    }

    public void Setup(ShopDetails details) {

        myDetails = details;

        RefreshShop();

    }

    public void BuyItem() {

        if(selectedItem == -1) return;

        ItemShop item = myDetails.sellingItems[selectedItem];

        if(inventory.Gold < item.value) return;

        inventory.Gold -= item.value;
        item.sellingItem.OnBuy(inventory.gameObject);
        inventory.AddItem(item.sellingItem);

        RefreshShop();
        

    }

    void RefreshShop() {

        // Deletes all item prefabs inside the shop items panel
        for(int i = 0; i < itemsTransform.childCount; i++) {

            Destroy(itemsTransform.GetChild(i).gameObject);

        }

        ShowItems();

        if(deletedItems.Exists(i => i == selectedItem)) selectedItem = -1;

        if(selectedItem == -1)
            itemDetails.ShowItem(myDetails, emptyShop, inventory.Gold);
        else
            ShowSelectedItem(selectedItem);

    }

    void ShowItems() {
        
        int totalItems = 0;
        deletedItems = new List<int>();

        // Loop each item that the shop sells
        for(int i = 0; i < myDetails.sellingItems.Length; i++) {

            ItemObject sellingItem = myDetails.sellingItems[i].sellingItem;

            // Check if the player already have this passive item.
            // Passive items have a limit of 1 in inventory
            if(sellingItem.type == ItemType.Passive) {
                if(inventory.HasItem(sellingItem)) {
                    deletedItems.Add(i);
                    continue;
                }
            }

            // Creates the prefabs
            GameObject itemObj = Instantiate(itemsPrefab, itemsTransform);
            itemObj.GetComponent<HUDShopItemInfo>().Setup(sellingItem, i, this);
            totalItems++;
            
        }

        emptyShop = totalItems <= 0;

    }

    ///<summary>Shows a specific item that the shop sells</summary>
    ///<param name="shopListIndex">The index of the item in ShopDetails</param>
    public void ShowSelectedItem(int shopListIndex) {

        selectedItem = shopListIndex;
        itemDetails.ShowItem(myDetails.sellingItems[shopListIndex], inventory.Gold);

    }

}