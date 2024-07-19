using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class used for storing items inside of inventories
/// </summary>
public class ItemStack
{
    private ItemDataSO item;
    public ItemDataSO Item { get => item;  set => item = value;  }

    private int count;
    public int Count { get => count; set => count = Mathf.Max(0, value); }

    public ItemStack(ItemDataSO item, int count = 0)
    {
        Item = item;
        Count = count;
    }
}
