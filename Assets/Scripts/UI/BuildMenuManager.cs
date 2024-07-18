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
    [SerializeField] public UIDocument document;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        Instance = this;
    }

    public void EnableUI()
    {
        StartCoroutine(LoadMenuSlots());
        document.enabled = true;
    }

    public void DisableUI()
    {
        document.enabled = false;
    }

    private IEnumerator LoadMenuSlots()
    {
        yield return null;

        Debug.Log("Loading Menu Slots!");

        UIDocument buildMenu = GetComponent<UIDocument>();

        foreach (BuildingDataSO building in BuildingManager.Instance.RegisteredBuildings)
        {
            BuildMenuSlot newSlot = new BuildMenuSlot(building, buildMenuSlotTemplate);

            buildMenu.rootVisualElement.Q(buildMenuItemParentName).Add(newSlot.button);
        }
    }
}
