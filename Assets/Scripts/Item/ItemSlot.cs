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
    public Transform DefaultItemDisplayPosition {  get => defaultItemDisplayPosition; }
    [SerializeField] private bool setNewItemDetails = true;
    [SerializeField] private bool hasStartingTestLog = false;

    [SerializeField] private bool canBePulledFrom = true;
    public bool CanBePulledFrom { get => canBePulledFrom; }
    [SerializeField] private float itemScaleSize = 0.8f;

    public ItemObject WorldItem { get => worldItem; set {
            worldItem = value;
            if (setNewItemDetails) SetupWorldItem();
            else if (worldItem != null) worldItem.transform.SetParent(transform, true);
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
            WorldItem.transform.localScale = Vector3.one * itemScaleSize;
        }
    }

    public void DestroyWorldItem()
    {
        Destroy(WorldItem.gameObject);
        worldItem = null;
    }

    public void CreateWorldItem(ItemDataSO item, bool setupItem = false)
    {
        WorldItem = Instantiate(item.worldItem);
        WorldItem.Item = item;
        WorldItem.transform.SetParent(transform, false);
        WorldItem.transform.localScale = Vector3.one * itemScaleSize;
        // Checks if setNewItemDetails is false because it would do it anyway if its true
        if (setupItem && !setNewItemDetails) SetupWorldItem();
    }
}
