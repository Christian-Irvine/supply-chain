using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class used for storing items inside of inventories
/// </summary>
public class ItemStack
{
    private string itemID;
    public string ItemID { get => itemID;  set => itemID = value;  }

    private int count;
    public int Count { get => count; set => count = Mathf.Max(0, value); }

    public ItemStack(string itemID, int count = 0)
    {
        ItemID = itemID;
        Count = count;
    }

    public ItemStack(ItemDataSO itemData, int count = 0)
    {
        ItemID = itemData.id;
        Count = count;
    }
}
