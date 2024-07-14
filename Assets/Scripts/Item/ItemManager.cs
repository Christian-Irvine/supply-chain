using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    [SerializeField] private List<ItemDataSO> registeredItems = new List<ItemDataSO>();
    [SerializeField] private Dictionary<string, ItemDataSO> itemLookup = new Dictionary<string, ItemDataSO>();
    
    public List<ItemDataSO> RegisteredItems { get { return registeredItems; } }
    /// <summary>
    /// Use LookupItem() method when possible instead of referencing Dictionary
    /// </summary>
    public Dictionary<string, ItemDataSO> ItemLookup { get { return itemLookup; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        RegisterItemLookup();
    }

    private void Start()
    {

    }

    private void RegisterItemLookup()
    {
        registeredItems.ForEach(item =>
        {
            itemLookup.Add(item.id, item);
        });
    }

    public ItemDataSO LookupItem(string id)
    {
        try
        {
            return itemLookup[id];
        }
        catch
        {
            Debug.LogWarning($"Couldn't find item with id '{id}' in registry");
            return null;
        }
    }
}
