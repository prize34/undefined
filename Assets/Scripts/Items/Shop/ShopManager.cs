﻿using UnityEngine;
using Undefined.Items.Shop;

[RequireComponent(typeof(Collider2D))]
public class ShopManager : MonoBehaviour
{
    
    #region Variables

    [Header("Shop Info")]
    [SerializeField] private ShopDetails details;

    #endregion
    
    public void OpenShop() {

        if(details == null) return;

        HUDManager.instance.ShowShop(details);

    }

}
