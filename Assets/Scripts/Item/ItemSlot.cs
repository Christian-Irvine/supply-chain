using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For use on any object that holds a singlular item, like a conveyor, or a table
/// </summary>
public class ItemSlot : MonoBehaviour
{
    private WorldItem worldItem;
    [SerializeField] private Transform defaultItemDisplayPosition;
    [SerializeField] private bool hasStartingTestLog = false;

    public WorldItem WorldItem { get => worldItem; set {
            worldItem = value;
            SetupWorldItem();
        }
    }

    private void Start()
    {
        if (hasStartingTestLog)
        {
            WorldItem = Instantiate(ItemManager.Instance.LookupItem("log").worldItem);
            WorldItem.ItemID = "log";
        }
    }

    private void SetupWorldItem()
    {
        if (worldItem != null) 
        { 
            worldItem.transform.SetParent(transform, false);
            worldItem.transform.position = defaultItemDisplayPosition.position;
        }
    }
}
