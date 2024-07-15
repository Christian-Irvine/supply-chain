using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    [SerializeField] private string buildingID;
    public string BuildingID {  get => buildingID; set => buildingID = value; }
    
    [SerializeField] private Vector2Int gridPosition;
    public Vector2Int GridPosition { get => gridPosition; set => gridPosition = value; }


    void Start()
    {
        
    }
  
    void Update()
    {
        
    }
}
