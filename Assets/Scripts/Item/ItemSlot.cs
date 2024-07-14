using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For use on any object that holds a singlular item, like a conveyor, or a table
/// </summary>
public class ItemSlot : MonoBehaviour
{
    private WorldItem worldItem;
    public WorldItem WorldItem { get => worldItem; set => worldItem = value; }
}
