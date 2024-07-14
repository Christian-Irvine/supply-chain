using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class used for storing items inside of inventories
/// </summary>
public class ItemStack
{
    private ItemDataSO itemData;
    public ItemDataSO ItemData {  get { return itemData; } }

    private int count;
    public int Count { get { return count; } set { count = Mathf.Max(0, value); } }

    public ItemStack(ItemDataSO itemData, int count = 0)
    {
        this.itemData = itemData;
        this.count = Mathf.Max(0, count);
    }
}
