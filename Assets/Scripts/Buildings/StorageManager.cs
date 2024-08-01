using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance;

    // Because this is changed using OnDestroy() for the racks this will change when the game is stopped and might cause issues
    private List<StorageRack> storageRacks = new List<StorageRack>();

    private int storageCapacity;
    public int StorageCapacity { get => storageCapacity; }

    private List<StorageItem> storageItems = new List<StorageItem>();
    public List<StorageItem> StorageItems { get => storageItems; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        CalculateStorage();
        FillStorageItems();
    }

    public void AddStorageRack(StorageRack rack)
    {
        storageRacks.Add(rack);
        CalculateStorage();
    }

    public void RemoveStorageRack(StorageRack rack)
    {
        storageRacks.Remove(rack);
        CalculateStorage();
    }

    private void CalculateStorage()
    {
        storageCapacity = 0;

        storageRacks.ForEach(rack =>
        {
            storageCapacity += rack.StorageSpace;
        });

        Debug.Log(storageCapacity);
    }

    // Fills storage items with list of all items (this will be changed when loading a save)
    private void FillStorageItems()
    {
        ItemManager.Instance.RegisteredItems.ForEach(item =>
        {
            storageItems.Add(new StorageItem(item));
        });
    }
}
