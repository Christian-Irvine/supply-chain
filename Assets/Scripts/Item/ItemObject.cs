using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The actual model display of an object which can be switched around where it is if needed
/// The Monobehaviour version of an item, like something the player is holding, or on a conveyor belt
/// </summary>
public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemDataSO item;
    public ItemDataSO Item { get => item; set => item = value; }
}
