using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingFacing
{
    forward,
    right,
    backward,
    left
}

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public Grid Grid {  get { return grid; } }
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
    public void PlaceBuilding(GameObject prefab, Vector3 worldPosition, BuildingFacing direction = BuildingFacing.forward)
    {
        Vector3 position = Grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.y), Mathf.RoundToInt(worldPosition.z)));
        Quaternion rotation = Quaternion.Euler((int)direction * 90, 0, 0);

        Instantiate(prefab, position, rotation, transform);
    }
}
