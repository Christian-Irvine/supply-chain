using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellManager : MonoBehaviour
{
    public static SellManager Instance;

    private List<OfferInventoryToSell> sellingInventories;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
            
        Instance = this;
    }

    private IEnumerator Start()
    {
        yield return null;
    
    
    }

    public void OfferInventory(OfferInventoryToSell inventory)
    {
        sellingInventories.Add(inventory);
    }

    public void RemoveInventory(OfferInventoryToSell inventory)
    {
        sellingInventories.Remove(inventory);
    }
}
