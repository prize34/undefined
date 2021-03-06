﻿using UnityEngine;
using UnityEngine.UI;
using Undefined.Items;

using TMPro;

public class HUDItemInfo : MonoBehaviour
{
        #region Variables

    [Header("UI")]
    [SerializeField] private Image itemImage = null;
    [SerializeField] private TextMeshProUGUI itemQuantity = null;

    [Header("Info")]
    public int inventoryIndex;
    public ItemObject myItem;

    // Script side
    HUDInventory invManager;


        #endregion

    void Start() {

        GetComponent<Button>().onClick.AddListener(ShowMyItemDetails);

    }

    void ShowMyItemDetails() {

        invManager.ShowSelectedItem(myItem, inventoryIndex);

    }

    public void Setup(InventoryItem item, int index, HUDInventory invetoryManager) {

        inventoryIndex = index;
        myItem = item.itemObj;
        invManager = invetoryManager;

        itemImage.sprite = myItem.icon;

        if(myItem.type == ItemType.Active)
            itemQuantity.text = item.quantity == 0 ? "" : $"{item.quantity}";
        else
            itemQuantity.text = "";

    }

}
