using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ScriptableObject class used for storing the data on objects
/// </summary>
[CreateAssetMenu(fileName = "ItemDataSO", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemDataSO : ScriptableObject
{
    public string displayName;
    public string id;
    public GameObject modelPrefab;
    [Tooltip("The cost to buy if item is purchaseable as a raw ingredient")] 
    public int purchaseCost;
    public int maxStackSize = 2;
}
