using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    [SerializeField] private List<BuildingDataSO> registeredBuildings = new List<BuildingDataSO>();
    private Dictionary<string, BuildingDataSO> buildingLookup = new Dictionary<string, BuildingDataSO>();
    /// <summary>
    /// Use LookupBuilding() method when possible instead of referencing Dictionary
    /// </summary>
    public Dictionary<string, BuildingDataSO> BuildingLookup { get => buildingLookup; }
    public List<BuildingDataSO> RegisteredBuildings { get => registeredBuildings; }

    private BuildingDataSO pickedBuilding;
    public BuildingDataSO PickedBuilding { get => pickedBuilding; set
        {   
            pickedBuilding = value;
            UpdateGhost();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);

        Instance = this;

        RegisterBuildingLookup();
    }

    void Start()
    {
        PickedBuilding = LookupBuilding("oven");
    }

    void Update()
    {
        
    }

    private void RegisterBuildingLookup()
    {
        registeredBuildings.ForEach(item =>
        {
            buildingLookup.Add(item.id, item);
        });
    }

    public BuildingDataSO LookupBuilding(string id)
    {
        try
        {
            return buildingLookup[id];
        }
        catch
        {
            Debug.LogWarning($"Couldn't find building with id '{id}' in registry");
            return null;
        }
    }

    private void UpdateGhost()
    {
        if (PickedBuilding != null)
        {
            PlacementGhost.Instance.GhostModel.SetActive(true);
            PlacementGhost.Instance.SetScale(pickedBuilding.size);
        }
        else
        {
            PlacementGhost.Instance.GhostModel.SetActive(false);
            PlacementGhost.Instance.SetScale(Vector2Int.one);
        }
    }
}
