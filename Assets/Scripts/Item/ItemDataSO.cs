using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ScriptableObject class used for storing the data on items
/// </summary>
[CreateAssetMenu(fileName = "ItemDataSO", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemDataSO : ScriptableObject
{
    public string displayName;
    public string id;
    public ItemObject worldItem;
    [Tooltip("The cost to buy if item is purchaseable as a raw ingredient")] 
    public int value;
    public int maxStackSize = 2;
}
