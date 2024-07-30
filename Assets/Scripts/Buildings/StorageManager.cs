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
}
