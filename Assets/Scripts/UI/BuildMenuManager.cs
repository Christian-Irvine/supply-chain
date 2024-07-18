using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildMenuManager : MonoBehaviour
{
    public static BuildMenuManager Instance;

    // Visual tree asset is what you use for referencing a UXML file
    [SerializeField] private VisualTreeAsset buildMenuSlotTemplate;
    [SerializeField] private string buildMenuItemParentName;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
    }

    private void OnEnable()
    {
        StartCoroutine(LoadMenuSlots());
    }

    private IEnumerator LoadMenuSlots()
    {
        yield return null;

        UIDocument buildMenu = GetComponent<UIDocument>();

        foreach (BuildingDataSO building in BuildingManager.Instance.RegisteredBuildings)
        {
            BuildMenuSlot newSlot = new BuildMenuSlot(building, buildMenuSlotTemplate);

            buildMenu.rootVisualElement.Q(buildMenuItemParentName).Add(newSlot.button);
        }
    }

    private void AddInventoryItem()
    {
        
    }
}
