using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageItem
{
    private ItemDataSO itemData;
    public ItemDataSO ItemData { get => itemData; set => itemData = value; }

    private int countStored;
    public int CountStored { get => countStored; set => countStored = value; }

    private int assignedCount;
    public int AssignedCount { get => assignedCount; set => assignedCount = value; }

    public StorageItem(ItemDataSO itemData, int countStored = 0, int assignedCount = 0)
    {
        ItemData = itemData;
        CountStored = countStored;
        AssignedCount = assignedCount;
    }
}
