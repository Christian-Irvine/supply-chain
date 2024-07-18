using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildMenuManager : MonoBehaviour
{
    public static BuildMenuManager Instance;

    // Visual tree asset is what you use for referencing a UXML file
    [SerializeField] private VisualTreeAsset buildMenuItem;
    [SerializeField] private string buildMenuItemParentName;

    private UIDocument buildMenu;
    private VisualElement scrollContainer;
    TemplateContainer buildMenuTemplate;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);

    }

    private void OnEnable()
    {
        buildMenu = GetComponent<UIDocument>();

        buildMenuTemplate = buildMenuItem.Instantiate();
        scrollContainer = buildMenu.rootVisualElement.Q(buildMenuItemParentName);

    }

    private void AddInventoryItem()
    {
        scrollContainer.Add(buildMenuTemplate);
    }
}
