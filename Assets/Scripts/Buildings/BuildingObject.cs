using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    [SerializeField] private BuildingDataSO buildingData;
    public BuildingDataSO BuildingData {  get => buildingData; set => buildingData = value; }
    
    [SerializeField] private Vector2Int gridPosition;
    public Vector2Int GridPosition { get => gridPosition; set => gridPosition = value; }
}
