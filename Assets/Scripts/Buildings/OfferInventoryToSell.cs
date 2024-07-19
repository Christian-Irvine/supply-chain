using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Goes ontop of any object which has items on it that it can sell. E.g shelf, fridge e.t.c
/// </summary>
public class OfferInventoryToSell : MonoBehaviour
{
    [SerializeField] private BuildingInventory inventory;
    public BuildingInventory Inventory { get => inventory; }

    void Start()
    {
        if (inventory == null)
        {
            inventory = GetComponent<BuildingInventory>();
        }

        if (inventory == null)
        {
            Debug.LogWarning("No BuildingInventory attached to GameObject, not offering to sell");
            return;
        }

        SellManager.Instance.OfferInventory(this);
    }

    private void OnDestroy()
    {
        SellManager.Instance.RemoveInventory(this);
    }
}
