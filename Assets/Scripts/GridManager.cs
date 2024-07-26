using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] LayerMask inverseNeighborMask;

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

    public List<Collider> GetNeighborsOfBuilding(BuildingObject buildingObject)
    {
        Vector3 collisionPosition = new Vector3(buildingObject.GridPosition.x, 1.5f, buildingObject.GridPosition.z);
        Vector3 collisionScale = new Vector3(((float)buildingObject.BuildingData.size.x / 2) + 0.5f, 3, ((float)buildingObject.BuildingData.size.y / 2) + 0.5f);

        Collider[] collisions = Physics.OverlapBox(collisionPosition, collisionScale, Quaternion.identity, ~inverseNeighborMask);
        List<Collider> neighbors = collisions.ToList();

        return neighbors;
    }
}
