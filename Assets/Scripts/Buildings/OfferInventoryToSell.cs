using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfferInventoryToSell : MonoBehaviour
{
    public BuildingInventory inventory;

    void Start()
    {
        inventory = GetComponent<BuildingInventory>();
        if (inventory == null) Debug.LogWarning("No BuildingInventory attached to GameObject");

        SellManager.Instance.OfferInventory(this);
    }

}
