using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingFacing
{
    forward,
    left,
    backward,
    right
}

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public Grid Grid {  get => grid; }
    [SerializeField] private Grid grid;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
    }

    /// <summary>
    /// Places a building in the world at grid co ordinates from world co ordinates
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="intPosition"></param>
    /// <param name="direction"></param>
    public void PlaceBuilding(GameObject prefab, Vector3 gridPosition, Vector3 rotatedOffsets, BuildingFacing direction = BuildingFacing.forward)
    {

        Vector3 position = GridToWorldPosition(gridPosition);  // Grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(gridPosition.x), Mathf.RoundToInt(gridPosition.y), Mathf.RoundToInt(gridPosition.z)));
        Quaternion rotation = Quaternion.Euler(0, (int)direction * 90, 0);

        Debug.Log("worldPosition" + position);

        GameObject building = Instantiate(prefab, position, rotation, transform);

        BuildingObject buildingObject = building.GetComponent<BuildingObject>();

        if (buildingObject == null) return;

        buildingObject.GridPosition = WorldToGridPosition(position);
        buildingObject.Facing = direction;
    }

    public Vector3Int WorldToGridPosition(Vector3 worldPosition)
    {
        return Grid.WorldToCell(worldPosition);
    }

    public Vector3 GridToWorldPosition(Vector3 gridPosition)
    {
        return Grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(gridPosition.x), Mathf.RoundToInt(gridPosition.y), Mathf.RoundToInt(gridPosition.z)));
    }
}
