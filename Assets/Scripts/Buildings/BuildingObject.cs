using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BuildingObject : MonoBehaviour
{
    public UnityEvent CheckNeighbors = new UnityEvent();

    [SerializeField] private BuildingDataSO buildingData;
    public BuildingDataSO BuildingData {  get => buildingData; set => buildingData = value; }
    
    [SerializeField]private Vector3Int gridPosition;
    public Vector3Int GridPosition { get => gridPosition; set => gridPosition = value; }

    private BuildingFacing facing;
    public BuildingFacing Facing { get => facing; set => facing = value; }

    private IEnumerator Start()
    {
        // Waiting a frame for buildings to sort out their event listeners
        yield return null;

        UpdateNeighbors();
    }

    // Should be called after loading structures on save (before first tick) and when building is placed beside it. 
    // Is for when this object should check the things around it
    public void CheckForNeighbors()
    {
        // Event is listened to elsewhere like in conveyor to see what is around it.
        CheckNeighbors?.Invoke();
    }

    private void OnDestroy()
    {
        // This might not work as I don't know if this object still has a collider before it is actually destroyed
        UpdateNeighbors();
    }

    // Is used for when the neighbors around this object should check their neighbors
    private void UpdateNeighbors()
    {
        List<Collider> neighbors = GridManager.Instance.GetNeighborsOfBuilding(this);

        neighbors.ForEach(neighbor =>
        {
            BuildingObject building = neighbor.GetComponent<BuildingObject>();

            if (building != null)
            {
                Debug.Log(building.name);
                building.CheckForNeighbors();
            }
        });
    }
}
