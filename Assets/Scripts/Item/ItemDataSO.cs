using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public GameObject modelPrefab;
    [Tooltip("The cost to buy if item is purchaseable as a raw ingredient")] 
    public int purchaseCost;
}
