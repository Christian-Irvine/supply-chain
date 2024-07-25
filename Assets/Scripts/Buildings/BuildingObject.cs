using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    [SerializeField] private BuildingDataSO buildingData;
    public BuildingDataSO BuildingData {  get => buildingData; set => buildingData = value; }
    
    private Vector3Int gridPosition;
    public Vector3Int GridPosition { get => gridPosition; set => gridPosition = value; }

    private BuildingFacing facing;
    public BuildingFacing Facing { get => facing; set => facing = value; }
}
