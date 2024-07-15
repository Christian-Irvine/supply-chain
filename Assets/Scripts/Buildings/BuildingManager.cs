using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    [SerializeField] private List<BuildingDataSO> registeredBuildings = new List<BuildingDataSO>();
    [SerializeField] private Dictionary<string, BuildingDataSO> buildingLookup = new Dictionary<string, BuildingDataSO>();

    /// <summary>
    /// Use LookupBuilding() method when possible instead of referencing Dictionary
    /// </summary>
    public Dictionary<string, BuildingDataSO> BuildingLookup { get => buildingLookup; }
    public List<BuildingDataSO> RegisteredBuildings { get => registeredBuildings; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);

        Instance = this;

        RegisterBuildingLookup();
    }

    void Start()
    {
        
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

    public BuildingDataSO LookupItem(string id)
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
}
