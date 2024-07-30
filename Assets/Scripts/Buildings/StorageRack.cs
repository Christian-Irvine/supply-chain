using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageRack : MonoBehaviour
{
    [SerializeField] private int storageSpace;
    public int StorageSpace { get =>  storageSpace; }

    private void Awake()
    {
        StorageManager.Instance.AddStorageRack(this);
    }

    // Using this might cause issues just be aware of it
    private void OnDestroy()
    {
        StorageManager.Instance.RemoveStorageRack(this);
    }
}
