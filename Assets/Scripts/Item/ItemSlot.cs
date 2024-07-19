using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds a world item, this is where the position is set and stays on the object forever
/// For use on any object that holds a singlular item, like a conveyor, or a table, or the player
/// </summary>
public class ItemSlot : MonoBehaviour
{
    private ItemObject worldItem;
    [SerializeField] private Transform defaultItemDisplayPosition;
    [SerializeField] private bool hasStartingTestLog = false;

    public ItemObject WorldItem { get => worldItem; set {
            worldItem = value;
            SetupWorldItem();
        }
    }

    private void Start()
    {
        if (hasStartingTestLog)
        {
            WorldItem = Instantiate(ItemManager.Instance.LookupItem("log").worldItem);
            WorldItem.Item = ItemManager.Instance.LookupItem("log");
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

    public void DestroyWorldItem()
    {

    }
}
