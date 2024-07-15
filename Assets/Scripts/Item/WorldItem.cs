using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Monobehaviour version of an item, like something the player is holding, or on a conveyor belt
/// </summary>
public class WorldItem : MonoBehaviour
{
    [SerializeField] private string itemID;
    public string ItemID { get => itemID; set => itemID = value; }
}
