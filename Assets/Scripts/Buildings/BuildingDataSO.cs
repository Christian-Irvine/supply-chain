using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorType
{
    Factory,
    Store,
    Storage,
    FactoryJoining,
    StoreJoining
}

/// <summary>
/// The ScriptableObject class used for storing the data on buildings
/// </summary>
[CreateAssetMenu(fileName = "BuildingDataSO", menuName = "ScriptableObjects/BuildingData", order = 2)]
public class BuildingDataSO : ScriptableObject
{
    public string displayName;
    public string id;
    public GameObject prefab;
    public int purchaseCost;
    public Sprite sprite;
    public Vector2Int size;
    public bool isRotatable;
    public List<FloorType> floorTypes;
}
