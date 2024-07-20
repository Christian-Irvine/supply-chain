using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellManager : MonoBehaviour
{
    public static SellManager Instance;

    [SerializeField] private int maxSellInventoryIterationAttempts;
    private List<OfferInventoryToSell> sellingInventories = new List<OfferInventoryToSell>();

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
    
        while (true)
        {
            yield return new WaitForSeconds(5);

            SellRandomItem();
        }
    }

    public void OfferInventory(OfferInventoryToSell inventory)
    {
        sellingInventories.Add(inventory);
    }

    public void RemoveInventory(OfferInventoryToSell inventory)
    {
        sellingInventories.Remove(inventory);
    }

    private void SellRandomItem()
    {
        OfferInventoryToSell randomInventory;
        List<ItemStack> inputStacks;

        int count = 0;
        int maxAttempts = Mathf.Min(maxSellInventoryIterationAttempts, sellingInventories.Count);
        if (maxAttempts == 0) return; 

        do
        {
            randomInventory = sellingInventories[Random.Range(0, sellingInventories.Count)];
            inputStacks = randomInventory.Inventory.InputStacks;
            if (inputStacks.Count > 0) break;

            count++;
        } while (count < maxAttempts);

        if (count >= maxAttempts) return;

        int itemStackIndex = Random.Range(0, inputStacks.Count);

        ItemDataSO item = inputStacks[itemStackIndex].Item;

        randomInventory.Inventory.ChangeInputStackCount(item, -1);

        GameManager.Instance.Money += item.purchaseCost;
    }
}
